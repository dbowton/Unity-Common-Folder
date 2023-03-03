using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
	[SerializeField] ObjectiveData data;
	public ObjectiveData Data { get { return data; } }
	public static ObjectiveManager instance = null;

	public GameObject canvas;

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(this);
	}

	public void ObjectiveComplete(string key)
	{
		print("Objective: " + key + " - Complete");
	}
}
