using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
	public static SceneTransitions instance = null;

	private void Awake()
	{
		if (instance = null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
			Destroy(this);
	}

	public static void LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}
}
