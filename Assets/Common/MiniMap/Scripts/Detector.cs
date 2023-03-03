using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Detector : MonoBehaviour
{
	[SerializeField] float range;
	[SerializeField] int partitions = 4;
	[SerializeField] List<SensorDetection> detections = new List<SensorDetection>();

	[SerializeField] GameObject UI;
	private List<GameObject> detectedObjects = new List<GameObject>();

	[System.Serializable]
	public class SensorDetection
	{		
		public string objectType = "";
		[HideInInspector] public Type searchType;

		public Sprite indicator;
	}

	private void Start()
	{
		for (int i = 0; i < detections.Count;)
		{
			Type type = Type.GetType(detections[i].objectType);

			if (type == null)
			{
				print(detections[i].objectType + " Not Found");
				detections.RemoveAt(i);
				continue;
			}

			detections[i].searchType = type;
			i++;
		}
	}

	void Detect()
	{	
		foreach (var go in detectedObjects)
			Destroy(go);

		detectedObjects.Clear();

		var collisions = Physics.OverlapSphere(transform.position, range);

		Vector2 UISizeDelta = UI.GetComponent<RectTransform>().sizeDelta;

		foreach (var collision in collisions)
		{
			foreach (var recordedType in detections)
			{
				if (collision.TryGetComponent(recordedType.searchType, out _))
				{
					//	Create Blip
					GameObject NewObj = new GameObject();
					NewObj.name = "detected_" + collision.name;

					//	Assign Sprite
					Image img = NewObj.AddComponent<Image>();
					Sprite sprite = recordedType.indicator;
					img.sprite = sprite;

					//	Assign Components
					NewObj.GetComponent<RectTransform>().sizeDelta = UISizeDelta * 0.9f;
					NewObj.GetComponent<RectTransform>().SetParent(UI.transform, false);
					NewObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

					// Set Position
					Vector3 pos = collision.transform.position - transform.position;
					pos.y = 0;

					Vector3 modForward = transform.forward - (Vector3.up * transform.forward.y);
					Vector3 modObject = pos - Vector3.up * pos.y;

					float angle = Vector3.SignedAngle(modForward, modObject, Vector3.up);

					float partitionAngle = 360f / partitions;
					float partitionIndex = angle / partitionAngle;
					angle = partitionAngle * Mathf.RoundToInt(partitionIndex);

					NewObj.GetComponent<RectTransform>().localEulerAngles = Vector3.forward * -angle;

					// Add Blip
					detectedObjects.Add(NewObj);
					break;
				}
			}

		}
	}

	void Update()
	{
		Detect();
	}
}
