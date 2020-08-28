using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;


[RequireComponent(typeof(Button))]
public class ButtonAudioClick : MonoBehaviour 
{
	//AudioSource clip;
	Button button;

	void Start()
	{
        // TODO change when AudioManager is implemented.
        //GameData gd = ServiceLocator.GetGameData();
        //clip = GetComponent<AudioSource>();
        //clip.clip = gd.clipButtonClick;
        //clip.volume = gd.GetOverallVolume();

		button = GetComponent<Button>();
		button.onClick.AddListener(new UnityAction(Clicked));
	}

	public void Clicked()
	{
        ServiceLocator.GetAudioManager().Register(AudioManager.Clips.BUTTONCLICK);
		//clip.Play();
	}
}
