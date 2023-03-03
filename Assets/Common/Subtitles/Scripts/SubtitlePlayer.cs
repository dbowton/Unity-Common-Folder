using UnityEngine;
using UnityEngine.Events;

public class SubtitlePlayer : MonoBehaviour
{
	public static SubtitlePlayer instance = null;
	AudioSource audioSource;
	TMPro.TMP_Text textField;
	string text;

	float timer = 0f;
	int maxChars;

	UnityEvent endFunction;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			audioSource = gameObject.AddComponent<AudioSource>();
			gameObject.name = "SubtitlePlayer";
		}
		else
			Destroy(this);
	}

	bool playing = false;
	void Update()
	{
		if (playing)
		{
			timer = Mathf.Min(audioSource.clip.length, timer + Time.deltaTime);
			int endIndex = Mathf.RoundToInt(timer / audioSource.clip.length * text.Length);
			int startIndex = Mathf.Max(0, endIndex - maxChars);
			if (!audioSource.isPlaying)
			{
				endIndex = text.Length;
				playing = false;

				endFunction?.Invoke();
				endFunction = null;
			}

			textField.text = text[startIndex..endIndex];
		}
	}

	public void Play(TMPro.TMP_Text textField, Color color, AudioClip clip, string text, int maxChars, UnityEvent startFunction, UnityEvent endFunction)
	{
		this.endFunction?.Invoke();
		startFunction?.Invoke();

		this.textField = textField;
		this.text = text;

		this.textField.enableAutoSizing = true;
		this.textField.color = color;

		timer = 0f;
		this.maxChars = (maxChars <= 0) ? text.Length : maxChars;
		this.endFunction = endFunction;

		audioSource.Stop();
		audioSource.clip = clip;
		audioSource.Play();
		playing = true;
	}
}
