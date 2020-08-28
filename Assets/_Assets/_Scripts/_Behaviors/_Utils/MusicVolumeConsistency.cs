using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicVolumeConsistency : MonoBehaviour 
{
	public float offset = 1.0f;

	void Start () 
	{
        // TODO change when AudioManager is implemented.
        GetComponent<AudioSource>().volume = offset * ServiceLocator.GetGameData().GetOverallMusic();
	}
}