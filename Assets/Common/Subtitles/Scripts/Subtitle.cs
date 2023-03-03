using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Subtitle
{
    [SerializeField] TMPro.TMP_Text textField;
    [SerializeField] Color color = Color.white;
    [SerializeField] AudioClip clip;
    [SerializeField][Tooltip("Value of Zero or less will show all characters")] int maxChars = 0;
    [SerializeField][TextArea(3,7)] string text;
    [SerializeField] UnityEvent OnStart;
    [SerializeField] UnityEvent OnOver;

    public void Play()
    {
        if (SubtitlePlayer.instance == null)
            new GameObject().AddComponent<SubtitlePlayer>();
        
        SubtitlePlayer.instance.Play(textField, color, clip, text, maxChars, OnStart, OnOver);
    }
}
