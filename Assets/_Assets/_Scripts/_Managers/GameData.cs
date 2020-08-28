using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using System.Runtime.CompilerServices;
[assembly: InternalsVisibleToAttribute("ServiceLocator")]

public class GameData
{
	public XMLRocketAttributes attributes;
	public Font defaultFont;

	float overallVolume;
	float overallMusic;


	private static bool AllDataLoaded = false;

	internal GameData ()
	{
		if(!AllDataLoaded)
        {
            LoadAll();
        }
	}

    void LoadAll()
    {
        LoadOptions();
        LoadAttributes();
        LoadAssets();
        AllDataLoaded = true;
    }

	public Font GetDefaultFont()
	{
		return defaultFont;
	}
    
	void LoadOptions()
	{
		overallVolume = PlayerPrefs.GetFloat("OverallVolume", 1.0f);
		overallMusic = PlayerPrefs.GetFloat("OverallMusic", 1.0f);
	}

	void LoadAttributes()
	{
        TextAsset textXML = Resources.Load<TextAsset>("Data/RocketAttributes");
		if (textXML == null)
            Debug.Log("Error at RocketAttributes: ");
        else
            attributes = XMLRocketAttributes.LoadFromText(textXML.text);
    }

    void LoadAssets()
    {
        defaultFont = Resources.Load<Font>("Fonts/Coalition_v2.");
    }

    public int GetPriceByName(string attName, int attLevel)
	{
		switch(attName)
		{
		case "ShipSpeedLevel":
			return attributes.Speed[attLevel].Price;
		case "ShipHandlingLevel":
			return attributes.Handling[attLevel].Price;
		case "ShipStructLevel":
			return attributes.Life[attLevel].Price;
		case "ShipRateShotLevel":
			return attributes.Rate[attLevel].Price;
		case "ShipMagneticLevel":
			return attributes.Magnetic[attLevel].Price;
		}
		return 0;
	}


	public void SetOverallVolume(float vol)
	{
        Debug.Log("Volume setado");
		overallVolume = vol;
		PlayerPrefs.SetFloat("OverallVolume", vol);
	}

	public float GetOverallVolume()
	{
		return overallVolume;
	}

	public void SetOverallMusic(float vol)
	{
        Debug.Log("Music setado");
		overallMusic = vol;
		PlayerPrefs.SetFloat("OverallMusic", vol);
	}

	public float GetOverallMusic()
	{
		return overallMusic;
	}
}
