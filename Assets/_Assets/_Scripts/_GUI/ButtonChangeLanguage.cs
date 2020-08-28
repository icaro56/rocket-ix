using UnityEngine;
using System.Collections;

public class ButtonChangeLanguage : MonoBehaviour
{
    public string value;

    public void Clicked()
    {
        ShipStatusData.SetLanguage(value);
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 25;
        style.normal.textColor = Color.white;
        GUI.Label(new Rect(10, 10, 100, 100), ShipStatusData.GetLanguage(), style);
        
    }
}
