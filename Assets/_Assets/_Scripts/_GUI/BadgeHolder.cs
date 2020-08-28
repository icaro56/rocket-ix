using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BadgeHolder : MonoBehaviour 
{
    public string achievementName;

	void Start () 
	{
        int hasDone = ShipStatusData.load("achievement_" + achievementName, -1);

        if (hasDone == 0)
        {
            transform.FindChild("Background").GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 0.4f);
        }
        else if (hasDone == -1)
        {
            Debug.LogWarning(achievementName + " nao existe!");
        }
        else
        {
			transform.FindChild("Background").GetComponent<Image>().color = new Color(1f, 0.6f, 0.0f, 0.8f);
        }
	}
}
