using UnityEngine;
using System.Collections;
using System;

public class ShopManager : MonoBehaviour
{
	#region Members
	public LabelPlayerPrefs fragmentsHolder;
	public ButtonUpgrade[] UpgradeButtons;

	int totalFragments;
    private static ShopManager instance = null;
    private static object _lock = new object();
    #endregion

    public static ShopManager Instance
    {
        get
        {
            lock (_lock)
            {
                if (instance == null)
                {
                    instance = (ShopManager)FindObjectOfType(typeof(ShopManager));

                    if (FindObjectsOfType(typeof(ShopManager)).Length > 1)
                    {
                        Debug.LogError("[Singleton] Something went really wrong " +
                            " - there should never be more than 1 singleton!" +
                            " Reopening the scene might fix it.");
                        return instance;
                    }

                    if (instance == null)
                    {
                        GameObject singleton = new GameObject();
                        instance = singleton.AddComponent<ShopManager>();
                        singleton.name = "(singleton) " + typeof(ShopManager).ToString();

                        Debug.Log("[Singleton] An instance of " + typeof(ShopManager) +
                            " is needed in the scene, so '" + singleton +
                            "' was created with DontDestroyOnLoad.");
                    }
                }

                return instance;
            }
        }
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            print("Destroying Singleton of " + typeof(ShopManager).ToString() + " beacause already there is an instance in the scene");
            Destroy(this.gameObject);
        }
    }


    void Start()
	{
        updateFragmentHolder();
    }

    public void updateFragmentHolder()
    {
        if (fragmentsHolder != null)
        {
            totalFragments = ShipStatusData.GetTotalFragments();
            fragmentsHolder.UpdateValue();
        }
    }

	public bool IsPurchasable(int value)
	{
        totalFragments = ShipStatusData.GetTotalFragments();
        return (totalFragments-value >= 0);
	}

	public void Purcharsed(int payed)
	{
        totalFragments = ShipStatusData.GetTotalFragments();
        // update current fragments
        totalFragments -= payed;
        ShipStatusData.SetTotalFragments(totalFragments);

        StartCoroutine(fragmentsHolder.UpdateValueIteratively(totalFragments));

        foreach (ButtonUpgrade b in UpgradeButtons)
        {
            b.UpdateStatus();
        }
        
        GooglePlayManager gm = ServiceLocator.GetGooglePlayManager();
        gm.setIsTotalFragmentLocalMoreUpdate(true);
        gm.SaveGame();
    }

	public void NoFunds()
	{
		StartCoroutine(fragmentsHolder.Blink());
	}
}
