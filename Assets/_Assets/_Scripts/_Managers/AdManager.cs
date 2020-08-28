using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using System.Runtime.CompilerServices;
using System;

[assembly: InternalsVisibleToAttribute("ServiceLocator")]

public class AdManager
{

    static bool initialized = false;

    [SerializeField]
    string gameId = "111777";

    [SerializeField]
    string rewardedAdName = "rewardedVideoZone";

    [SerializeField]
    bool isTest = false;

    internal AdManager()
    {
        if (!initialized)
        {
            Advertisement.Initialize(gameId, isTest);

            initialized = true;
        }
    }

    /*void TooglePause(bool isPaused)
    {
        if (initialized && isPaused)
        {
            ShowAd();
        }
    }*/

    //anúncios sem recompensa
    public bool ShowAd()
    {
        if (initialized && Advertisement.IsReady())
        {
            Advertisement.Show();
            return true;
        }

        return false;
    }

    //com recompensa
    public bool ShowRewardedAd(Action<ShowResult> callback)
    {
        if (initialized && Advertisement.IsReady(rewardedAdName))
        {
            ShowOptions options = new ShowOptions();
            options.resultCallback = callback;

            Advertisement.Show(rewardedAdName, options);
            return true;
        }

        return false;
    }

    void SampleAdCallbackHandler(ShowResult result)
    {
        switch(result)
        {
            case ShowResult.Finished:
                Debug.Log("Ad Finished. Rewarding player");
                break;
            case ShowResult.Skipped:
                Debug.Log("Ad Skipped.");
                break;
            case ShowResult.Failed:
                Debug.Log("Ad Failed");
                break;
        }
    }
}
