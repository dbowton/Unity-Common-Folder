using UnityEngine;

public class ObjectiveTester : MonoBehaviour
{
	private void Start()
	{
		if (ObjectiveManager.instance != null)
			ObjectiveManager.instance.Data.SetFunction("press space", () => ObjectiveManager.instance.Data.RestartObjective("press space"));
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && ObjectiveManager.instance != null)
			ObjectiveManager.instance.Data.UpdateObjective("press space", 1);
	}
}
