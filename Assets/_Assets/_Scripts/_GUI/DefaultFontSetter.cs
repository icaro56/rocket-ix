using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DefaultFontSetter : MonoBehaviour 
{
	void Start () 
	{
        Text text = GetComponent<Text>();
		text.font = ServiceLocator.GetGameData().GetDefaultFont();
	}
}
