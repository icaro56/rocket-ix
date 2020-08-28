using UnityEngine;
using System.Collections;

public class AchievementsEvent : MonoBehaviour {

    public void ClickButton()
    {
        GooglePlayManager gm = ServiceLocator.GetGooglePlayManager();
        if (gm.UserIsLogged())
        {
            gm.showAchievementGooglePlayUI();
        }
        else
        {
            gm.signinAndShowAchievements();
        }
    }
}
