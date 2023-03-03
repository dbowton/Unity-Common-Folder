using System.Text;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SceneTester : MonoBehaviour
{
	[SerializeField] GameObject buttonPrefab;
	[SerializeField] GameObject buttonUICanvas;

	private void Start()
	{
		for (int i = 0; i < SceneManagement.GetSceneCount(); i++)
		{
			int x = i;
			GameObject go = Instantiate(buttonPrefab, buttonUICanvas.transform);
			go.GetComponent<Button>().onClick.AddListener(() => SceneManagement.LoadScene(x, 2.5f, 1.25f));

			string name = SceneManagement.GetSceneName(i).Replace("_Demo", "").Replace("_", " ");

			StringBuilder builder = new StringBuilder();
			for (int j = 0; j < name.Length - 1; j++)
			{
				if (Char.IsUpper(name[j]) && !Char.IsUpper(name[j + 1]) && builder.Length > 0) builder.Append(' ');
				builder.Append(name[j]);
			}
			builder.Append(name[^1]);
			name = builder.ToString();

			go.GetComponentInChildren<TMPro.TMP_Text>().text = name;
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
				Application.Quit();
#endif
		}
	}
}
