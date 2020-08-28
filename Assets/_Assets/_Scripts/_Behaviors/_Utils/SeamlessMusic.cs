using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SeamlessMusic : MonoBehaviour 
{
	AudioSource audioSrc;
	float playingTime;
	bool quit = false;

	void Start () 
	{
        audioSrc = GetComponent<AudioSource>();
        audioSrc.time = PlayerPrefs.GetFloat("BackgroundMusicTime", 0);
	}

	void Update()
	{
        playingTime = audioSrc.time;
	}

	void OnDestroy()
	{
		PlayerPrefs.SetFloat("BackgroundMusicTime", quit ? 0 : playingTime);
	}

	void OnApplicationQuit()
	{
		quit = true;
	}
}
