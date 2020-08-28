using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderListener : MonoBehaviour 
{
    public string PrefsName;
	Slider slider;

    GameData gameData;

	void Start()
	{
        gameData = ServiceLocator.GetGameData();

		slider = gameObject.GetComponent<Slider>();

		if(PrefsName.Equals("MusicValue"))
		{
            slider.value = gameData.GetOverallMusic();
		}
		else
		{
            slider.value = gameData.GetOverallVolume();
		} 
	}

	public void OnSliderChange()
    {
        ShipStatusData.save(PrefsName, slider.value);
		if(PrefsName.Equals("MusicValue"))
            gameData.SetOverallMusic(slider.value);
		else
            gameData.SetOverallVolume(slider.value);
	}
}
