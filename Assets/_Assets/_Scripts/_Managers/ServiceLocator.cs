using UnityEngine;
using System.Collections;

public class ServiceLocator : Singleton<ServiceLocator> 
{
    static GameData sGameData;
    static AchievementManager sAchievementMgr;
    static GooglePlayManager sGooglePlayMgr;
    static EventSystem sEventSystem;
    static AudioManager sAudioManager;
    static AdManager sAdManager;

    protected override void Init () 
    {
        DontDestroyOnLoad(this);

        sGameData = new GameData();
        sGooglePlayMgr = new GooglePlayManager();
        sEventSystem = new EventSystem();

        // Service locator must hold all achievements as AchievementManager will grab their references from it.
        sAchievementMgr = new AchievementManager(this.gameObject);

        sAudioManager = new AudioManager();
        sAudioManager.Init();

        sAdManager = new AdManager();
    }

    public static GameData GetGameData()
    {
        Debug.Assert(sGameData != null, "sGameData is null.");

        return sGameData;
    }

    public static AchievementManager GetAchievementManager()
    {
        Debug.Assert(sGameData != null, "sAchievementMgr is null.");

        return sAchievementMgr;
    }

    public static GooglePlayManager GetGooglePlayManager()
    {
        Debug.Assert(sGooglePlayMgr != null, "sGooglePlayMgr is null.");

        return sGooglePlayMgr;
    }

    public static EventSystem GetEventSystem()
    {
        Debug.Assert(sEventSystem != null, "sEventSystem is null.");

        return sEventSystem;
    }

    public static AudioManager GetAudioManager()
    {
        Debug.Assert(sAudioManager != null, "sAudioManager is null.");

        return sAudioManager;
    }

    public static AdManager GetAdManager()
    {
        Debug.Assert(sAdManager != null, "sAdManager is null.");

        return sAdManager;
    }
}
