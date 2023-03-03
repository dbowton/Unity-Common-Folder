using System.Collections.Generic;
using UnityEngine;

public class SubtitleDemo : MonoBehaviour
{
    [SerializeField] List<Subtitle> subtitles;
    int index = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            PlayNext();
    }

    public void PlayNext()
    {
		subtitles[index].Play();
		index++;
		if (index >= subtitles.Count) index = 0;
	}
}
