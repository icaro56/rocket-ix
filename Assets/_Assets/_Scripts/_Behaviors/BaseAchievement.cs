using UnityEngine;
using System.Collections;

public enum AchievementType
{ 
    Fragment, Time, Distance, GameOver, Upgrade
}

public class BaseAchievement : MonoBehaviour 
{
    public string title;
    protected static RocketBehavior rocket;
    public AchievementType type;
    protected string myGooglePlayId;

    public string GetGooglePlayId()
    {
        return myGooglePlayId;
    }
		
    protected void Setup()
    {
        //RETIRAR ISSO 
        //ShipStatusData.save("achievement_" + title, 0);

        //rocket = GameObject.FindGameObjectWithTag("rocket").GetComponent<RocketBehavior>();
    }

    public static void configureRocket()
    {
        rocket = GameObject.FindGameObjectWithTag("rocket").GetComponent<RocketBehavior>();
    }

    public virtual bool Check()
    {
        return false;
    }

    public virtual bool Done()
    {
        return (ShipStatusData.load("achievement_" + title, 0) == 1);
    }

    public virtual void ForceCheck()
    {
        ShipStatusData.save("achievement_" + title, 1);
    }

    public void activateGooglePlayAchievement()
    {
        if (myGooglePlayId.Length > 0)
            ServiceLocator.GetGooglePlayManager().achievementReportProgress(myGooglePlayId, 100.0f);
    }
}
