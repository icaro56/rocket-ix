using UnityEngine;
using System.Collections;

using System.Runtime.CompilerServices;
[assembly: InternalsVisibleToAttribute("ServiceLocator")]

public class AchievementManager
{
    public static BaseAchievement[] achievements;

    static bool initialized = false;

    AchievementInfo info;

    internal AchievementManager(GameObject achievementHolder)
    {
        Transform t = achievementHolder.transform;
        if (!initialized)
        {
            achievements = new BaseAchievement[t.childCount];
            for (int i = 0; i < t.childCount; i++)
            {
                achievements[i] = t.GetChild(i).GetComponent<BaseAchievement>();
            }

            initialized = true;
            ServiceLocator.GetGooglePlayManager().loadAchievements(achievements);
        }
    }

    public void Setup()
    {
        BaseAchievement.configureRocket();
        info = GameObject.FindGameObjectWithTag("achievementholder").GetComponent<AchievementInfo>() as AchievementInfo;
    }

    public BaseAchievement[] getAchievements()
    {
        return achievements;
    }
		
    public void CheckAchievementOf(AchievementType type)
    {
        for (int i = 0; i < achievements.Length; i++)
        {
            if (achievements[i].type == type && !achievements[i].Done())
            {
                //Se fez o achievement
                if (achievements[i].Check())
                {
                    //Mostramos ele
					info.ShowUp( achievements[i].title);

                    achievements[i].activateGooglePlayAchievement();
                }
            }
        }
    }
}
