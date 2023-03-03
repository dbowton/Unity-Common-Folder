using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Sensor : MonoBehaviour
{
    [SerializeField] float range;
	[SerializeField] float heightDif = 5f;
	[SerializeField] List<SensorDetection> detections = new List<SensorDetection>();

    [SerializeField] GameObject UI;
    private List<GameObject> detectedObjects = new List<GameObject>();

	private void OnValidate()
	{
		foreach (var x in detections)
		{
			if (x.default_sprite == null) return;
			if (x.above_sprite == null) x.above_sprite = x.default_sprite;
			if (x.below_sprite == null) x.below_sprite = x.default_sprite;
		}
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

	[System.Serializable]
    public class SensorDetection
    {
		public string objectType = "";
		[HideInInspector] public Type searchType;

        public Sprite default_sprite;
        public Sprite above_sprite;
        public Sprite below_sprite;
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
				if(collision.TryGetComponent(recordedType.searchType, out Component foundType))
				{
					//	Create Blip
					GameObject NewObj = new GameObject();
					NewObj.name = "detected_" + collision.name;
					Image img = NewObj.AddComponent<Image>();

					NewObj.GetComponent<RectTransform>().sizeDelta = UISizeDelta * 0.15f;
					NewObj.GetComponent<RectTransform>().SetParent(UI.transform, false);

					// Assign Sprite
					Sprite sprite = recordedType.default_sprite;
					if (transform.position.y - heightDif > collision.transform.position.y)
						sprite = recordedType.below_sprite;

					if (transform.position.y + heightDif < collision.transform.position.y)
						sprite = recordedType.above_sprite;

					img.sprite = sprite;

					// Set Position
					Vector3 pos = collision.transform.position - transform.position;
					pos.y = 0;

					Vector3 modForward = transform.forward - (Vector3.up * transform.forward.y);
					Vector3 modObject = pos - Vector3.up * pos.y;

					float angle = (Vector3.SignedAngle(modForward, modObject, Vector3.up) - 90) * Mathf.Deg2Rad;

					pos = Vector3.right * pos.magnitude;
					pos = new Vector3(Mathf.Cos(angle) * pos.x, -Mathf.Sin(angle) * pos.x, 0);

					pos *= ((UISizeDelta.x / 2f) - (NewObj.GetComponent<RectTransform>().sizeDelta.x / 2 * 1.5f)) / range;

					NewObj.GetComponent<RectTransform>().anchoredPosition = pos;

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
