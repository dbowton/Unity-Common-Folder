using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagement : MonoBehaviour
{
	public static SceneManagement instance = null;

	[SerializeField] GameObject fadeScreenPrefab;
	static GameObject fadeScreen;

	private void Awake()
	{
		print("awake");
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
			Destroy(this);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			LoadScene(0, 1.25f, 0.625f);
		}

		if (fadeOut && preppedScene != null)
		{
			if (totalFadeOutTime > 0 && workingTime == 0)
			{
				Destroy(fadeScreen);
				fadeScreen = Instantiate(fadeScreenPrefab);
				DontDestroyOnLoad(fadeScreen);
			}

			workingTime += Time.deltaTime;
			if (workingTime >= totalFadeOutTime)
			{
				fadeOut = false;
				SceneManager.LoadScene(preppedScene);
				preppedScene = null;
				workingTime = 0;
				totalFadeOutTime = 0;
			}
			else
			{
				Color color = fadeScreen.GetComponentInChildren<Image>().color;
				color.a = Mathf.Min(workingTime / totalFadeOutTime, 1);
				fadeScreen.GetComponentInChildren<Image>().color = color;
			}
		}
		else if(totalFadeInTime > 0)
		{
			if (fadeScreen == null)
				fadeScreen = Instantiate(fadeScreenPrefab);

			workingTime += Time.deltaTime;
			if (workingTime >= totalFadeInTime)
			{
				Destroy(fadeScreen);
				fadeOut = true;
				totalFadeInTime = 0;
			}
			else
			{
				Color color = fadeScreen.GetComponentInChildren<Image>().color;
				color.a = 1 - Mathf.Min(workingTime / totalFadeInTime, 1);
				fadeScreen.GetComponentInChildren<Image>().color = color;
			}
		}
	}

	static bool fadeOut = true;
	static float totalFadeOutTime = 0f;
	static float totalFadeInTime = 0f;
	static float workingTime = 0f;

	static string preppedScene = null;

	public static void LoadScene(string sceneName, float fadeOut = 0f, float fadeIn = 0f)
	{
		if (preppedScene != null) return;
		if (fadeOut == 0)
			Destroy(fadeScreen);
		SceneManagement.fadeOut = true;
		workingTime = 0;
		totalFadeOutTime = fadeOut;
		totalFadeInTime = fadeIn;
		preppedScene = sceneName;
	}

	public static void LoadScene(int index, float fadeOut = 0f, float fadeIn = 0f)
	{
		if (preppedScene != null) return;
		if(fadeOut == 0)
			Destroy(fadeScreen);
		SceneManagement.fadeOut = true;
		workingTime = 0;
		totalFadeOutTime = fadeOut;
		totalFadeInTime = fadeIn;
		preppedScene = GetSceneName(index);
	}

	public static int GetSceneCount() 
	{
		return SceneManager.sceneCountInBuildSettings;
	}

	public static List<string> GetSceneNames()
	{
		List<string> names = new List<string>();

		for(int i = 0; i < SceneManager.sceneCountInBuildSettings; i++) 
			names.Add(GetSceneName(i));

		return names;
	}

	public static string GetSceneName(int index) 
	{
		string path = SceneUtility.GetScenePathByBuildIndex(index);
		int slash = path.LastIndexOf('/');
		string name = path.Substring(slash + 1);
		int dot = name.LastIndexOf('.');
		return name.Substring(0, dot);
	}
}
