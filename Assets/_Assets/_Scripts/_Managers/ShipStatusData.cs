using UnityEngine;
using System.Collections;

public class ShipStatusData 
{

    private static string SHIP_SPEED_LEVEL = "ShipSpeedLevel";
    private static string SHIP_HANDLING_LEVEL = "ShipHandlingLevel";
    private static string SHIP_STRUCT_LEVEL = "ShipStructLevel";
    private static string SHIP_RATE_SHOT_LEVEL = "ShipRateShotLevel";
    private static string SHIP_MAGNETIC_LEVEL = "ShipMagneticLevel";
    private static string TOTAL_FRAGMENTS = "TotalFragments";
    private static string SHIP_LIFE = "ShipLife";
    private static string LANGUAGE = "Language";
    private static string WAS_LOGGED_ON_GOOGLE = "WasLoggedOnGoogle";
    private static string BEST_RECORD = "BestRecord";
    private static string BEST_RECORD_UPLOAD = "BestRecordUpload";

    public static void save(string attribute, int value)
    {
        PlayerPrefs.SetInt(attribute, value);
    }

    public static void save(string attribute, string value)
    {
        PlayerPrefs.SetString(attribute, value);
    }

    public static void save(string attribute, float value)
    {
        PlayerPrefs.SetFloat(attribute, value);
    }

    private static int load(string attribute)
    {
        return load(attribute, 0);
    }

    private static string load(string attribute, string value)
    {
        return PlayerPrefs.GetString(attribute, value);
    }

    public static int load(string attribute, int defaultValue)
    {
        return PlayerPrefs.GetInt(attribute, defaultValue);
    }

    public static void SetSpeedLevel(int value)
    {
        save(SHIP_SPEED_LEVEL, value);
    }

    public static int GetSpeedLevel()
    {
        return load(SHIP_SPEED_LEVEL);
    }

    public static void SetHandlingLevel(int value)
    {
        save(SHIP_HANDLING_LEVEL, value);
    }

    public static int GetHandlingLevel()
    {
        return load(SHIP_HANDLING_LEVEL);
    }

    public static void SetStructLevel(int value)
    {
        save(SHIP_STRUCT_LEVEL, value);
    }

    public static int GetStructLevel()
    {
        return load(SHIP_STRUCT_LEVEL);
    }

    public static void SetRateShotLevel(int value)
    {
        save(SHIP_RATE_SHOT_LEVEL, value);
    }

    public static int GetRateShotLevel()
    {
        return load(SHIP_RATE_SHOT_LEVEL);
    }

    public static void SetMagneticLevel(int value)
    {
        save(SHIP_MAGNETIC_LEVEL, value);
    }

    public static int GetMagneticLevel()
    {
        return load(SHIP_MAGNETIC_LEVEL);
    }

    public static void SetTotalFragments(int value)
    {
        save(TOTAL_FRAGMENTS, value);
    }

    public static int GetTotalFragments()
    {
        return load(TOTAL_FRAGMENTS);
    }

    public static void SetBestRecord(int value)
    {
        save(BEST_RECORD, value);
    }

    public static int GetBestRecord()
    {
        return load(BEST_RECORD);
    }

    public static void SetBestRecordUpload(int value)
    {
        save(BEST_RECORD_UPLOAD, value);
    }

    public static int GetBestRecordUpload()
    {
        return load(BEST_RECORD_UPLOAD);
    }

    public static void SetLife(int value)
    {
        save(SHIP_LIFE, value);
    }

    public static int GetLife()
    {
        return load(SHIP_LIFE, 3);
    }

    public static void SetWasLoggedOnGoogle(int value)
    {
        save(WAS_LOGGED_ON_GOOGLE, value);
    }

    public static int GetWasLoggedOnGoogle()
    {
        return load(WAS_LOGGED_ON_GOOGLE, 0);
    }

    public static void SetLanguage(string value)
    {
        save(LANGUAGE, value);
    }

    public static string GetLanguage()
    {
        return load(LANGUAGE, "-");
    }
}
