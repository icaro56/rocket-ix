using UnityEngine;
using System.Collections;

public class RankingEvent : MonoBehaviour 
{
    public void ClickButton()
    {
        GooglePlayManager gm = ServiceLocator.GetGooglePlayManager();
        if (gm.UserIsLogged())
        {
            gm.showLeaderboardGooglePlayUI();
        }
        else
        {
            gm.signinAndShowRanking();
        }

       //ServiceLocator.GetAdManager().ShowAd();
    }
}
