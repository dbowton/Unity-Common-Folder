using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
public class GPS : MonoBehaviour
{
	[SerializeField] float startDistanceFromPlayer = 2.5f;
	[SerializeField] GameObject target;

	LineRenderer lineRenderer;

	[SerializeField] float height;

	Vector3 destination = Vector3.zero;

	private void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
		path = new NavMeshPath();
	}

	NavMeshPath path;
	void Update()
	{
		transform.position = target.transform.position;

		if (NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path))
		{
			List<Vector3> waypoints = FindPointAlongPath(path.corners.ToList(), startDistanceFromPlayer);
			lineRenderer.positionCount = waypoints.Count;
			for (int i = 0; i < waypoints.Count; i++)
			{
				Vector3 wp = waypoints[i];
				wp.y += height;
				waypoints[i] = wp;
			}

			lineRenderer.SetPositions(waypoints.ToArray());
		}
		else
			lineRenderer.enabled = false;
	}

	public void SetLocation(Vector3 location)
	{
		destination = location;
	}

	List<Vector3> FindPointAlongPath(List<Vector3> path, float distanceToTravel)
	{
		if (distanceToTravel < 0)
		{
			return path;
		}

		for (int i = 0; i < path.Count - 1; i++)
		{
			if (distanceToTravel <= Vector3.Distance(path[i], path[i + 1]))
			{
				Vector3 directionToTravel = path[i + 1] - path[i];
				directionToTravel.Normalize();

				path.RemoveRange(0, i);
				path[0] = path[0] + (directionToTravel * distanceToTravel);

				return path;
			}
			else
			{
				distanceToTravel -= Vector3.Distance(path[i], path[i + 1]);
			}
		}

		return new List<Vector3>();
	}

	private void OnEnable()
	{
		lineRenderer.enabled = true;
	}

	private void OnDisable()
	{
		lineRenderer.enabled = false;
	}
}
