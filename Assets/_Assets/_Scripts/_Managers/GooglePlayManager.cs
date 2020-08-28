using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using System.Runtime.CompilerServices;
[assembly: InternalsVisibleToAttribute("ServiceLocator")]

public class GooglePlayManager
{
    static bool initialized = false;
    ISavedGameMetadata currentGame = null;

    private BaseAchievement[] localAchievementsList = null;

    private static bool isTotalFragmentLocalMoreUpdated = false;

    internal GooglePlayManager()
    {
        if (!initialized)
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();

            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = false;
            PlayGamesPlatform.Activate();

            initialized = true;

            //if (UserIsLogged())
            if (ShipStatusData.GetWasLoggedOnGoogle() == 1)
            {
                signin(SigninRoutineCallback, true);
            }
        }
    }

    public void setIsTotalFragmentLocalMoreUpdate(bool opt)
    {
        isTotalFragmentLocalMoreUpdated = opt;
    }


    public bool UserIsLogged()
    {
        return PlayGamesPlatform.Instance.localUser.authenticated;
    }


    //new void Awake()
    //{

    //    if (!initialized)
    //    {
    //        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();

    //        PlayGamesPlatform.InitializeInstance(config);
    //        PlayGamesPlatform.DebugLogEnabled = true;
    //        PlayGamesPlatform.Activate();

    //        initialized = true;

    //        //if (UserIsLogged())
    //        if (ShipStatusData.GetWasLoggedOnGoogle() == 1)
    //        {
    //            Instance.signin(SigninRoutineCallback, true);
    //        }
    //    }

    //}

    private void SigninRoutineCallback(bool success)
    {
        if (success)
        {
            Debug.Log("Logou com sucesso");
            SaveGame();
            ShipStatusData.SetWasLoggedOnGoogle(1);
        }
        else
        {
            Debug.Log("Falha ao logar");
            ShipStatusData.SetWasLoggedOnGoogle(0);
        }
    }

    public void signin(Action<bool> callback, bool silent)
    {
        if (initialized)
        {
            PlayGamesPlatform.Instance.Authenticate(callback, silent);
        }
    }

    private void SigninRoutineRankingCallback(bool success)
    {
        if (success)
        {
            Debug.Log("Logou com sucesso");
            showLeaderboardGooglePlayUI();
            SaveGame();
            ShipStatusData.SetWasLoggedOnGoogle(1);

        }
        else
        {
            Debug.Log("Falha ao logar");
            ShipStatusData.SetWasLoggedOnGoogle(0);
        }
    }

    public void signinAndShowRanking()
    {
        if (initialized)
        {
            PlayGamesPlatform.Instance.Authenticate(SigninRoutineRankingCallback);
        }
    }

    private void SigninRoutineAchievementsCallback(bool success)
    {
        if (success)
        {
            Debug.Log("Logou com sucesso");
            showAchievementGooglePlayUI();
            SaveGame();
            ShipStatusData.SetWasLoggedOnGoogle(1);
        }
        else
        {
            Debug.Log("Falha ao logar");
            ShipStatusData.SetWasLoggedOnGoogle(0);
        }
    }

    public void signinAndShowAchievements()
    {
        if (initialized)
        {
            PlayGamesPlatform.Instance.Authenticate(SigninRoutineAchievementsCallback);
        }
    }

    public void signout()
    {
        if (initialized && UserIsLogged())
        {
            PlayGamesPlatform.Instance.SignOut();
            ShipStatusData.SetWasLoggedOnGoogle(0);
        }
    }

    public void achievementReportProgress(string achievementId, float percent)
    {
        if (initialized && UserIsLogged())
        {
            Social.ReportProgress(achievementId, percent, (bool success) =>
            {
                if (success)
                    Debug.Log("Achievement status changed");
                else
                    Debug.Log("Failed report progress");
            });
        }
    }

    private void OnLoadAchievementsCallback(IAchievement[] list)
    {
        if (list != null && list.Length > 0)
        {
            for (int i = 0; i < list.Length; i++)
            {
                string achievementId = list[i].id;
                bool isCompleted = list[i].completed;
                BaseAchievement localAchievement = null;

                for (int j = 0; j < localAchievementsList.Length; j++)
                {
                    if (localAchievementsList[j].GetGooglePlayId().Equals(achievementId))
                    {
                        localAchievement = localAchievementsList[j];
                        break;
                    }
                }

                if (localAchievement != null)
                {
                    if (isCompleted != localAchievement.Done())
                    {
                        if (isCompleted)
                        {
                            localAchievement.ForceCheck();
                        }
                        else if (localAchievement.Done())
                        {
                            achievementReportProgress(achievementId, 100.0f);
                        }
                    }
                }
            }
        }
    }

    public void loadAchievements(BaseAchievement[] list)
    {
        if (initialized && UserIsLogged())
        {
            localAchievementsList = list;
            Social.LoadAchievements(OnLoadAchievementsCallback);

            //Se comando acima não funcionar usar esse.
            //PlayGamesPlatform.Instance.LoadAchievements
        }
    }

    public void publishScore(long score)
    {
        if (initialized && UserIsLogged())
        {
            Social.ReportScore(score, Rocket_IX.GPGSIds.leaderboard_ranking, (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Score published successfully");
                    ShipStatusData.SetBestRecordUpload(1);
                }
                else {
                    Debug.Log("Failed to publish score");
                    ShipStatusData.SetBestRecordUpload(0);
                }
            });
        }
    }

    public void publishScoreAndShowUIRanking(long score)
    {
        if (initialized && UserIsLogged())
        {
            Social.ReportScore(score, Rocket_IX.GPGSIds.leaderboard_ranking, (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Score published successfully");
                    ShipStatusData.SetBestRecordUpload(1);
                    Social.ShowLeaderboardUI();
                }
                else {
                    Debug.Log("Failed to publish score");
                    ShipStatusData.SetBestRecordUpload(0);
                    Social.ShowLeaderboardUI();
                }
            });
        }
    }

    public void showLeaderboardGooglePlayUI()
    {
        if (initialized && UserIsLogged())
        {
            if (ShipStatusData.GetBestRecordUpload() == 0)
            {
                int totalScore = ShipStatusData.GetBestRecord();
                publishScoreAndShowUIRanking(totalScore);
            }
            else
            {
                Social.ShowLeaderboardUI();
            }
        }
    }

    public void showAchievementGooglePlayUI()
    {
        if (initialized && UserIsLogged())
        {
            //necessário deixar o AchievementManager como persitente entre cenas.
            BaseAchievement[] achievements = ServiceLocator.GetAchievementManager().getAchievements();
            if (achievements != null && achievements.Length > 0)
                loadAchievements(achievements);

            Social.ShowAchievementsUI();
        }
    }

    public void SaveGame()
    {
        if (initialized && UserIsLogged())
        {
            //PlayGamesPlatform.Instance.localUser.authenticated
            WriteUpdateShipData();
        }
        else
        {
            isTotalFragmentLocalMoreUpdated = false;
        }
            
    }

    private void WriteUpdateShipData()
    {
        // Read the current data and kick off the callback chain
        Debug.Log("(Rocket IX) Saved Game: Reading");
        ReadSavedGame("file_rocketix_ship_status");
    }

    /*public void ShowSelectSaveUI()
    {
        uint maxNumToDisplay = 1;
        bool allowCreateNew = false;
        bool allowDelete = true;

        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.ShowSelectSavedGameUI("Select saved game",
            maxNumToDisplay,
            allowCreateNew,
            allowDelete,
            OnSavedGameSelected);
    }


    public void OnSavedGameSelected(SelectUIStatus status, ISavedGameMetadata game)
    {
        if (status == SelectUIStatus.SavedGameSelected)
        {
            // handle selected game save
            Debug.Log("Sucesso. Conseguiu criar um savedata");
            savedGameMetadata = game;
            ReadSavedGame(savedGameMetadata.Filename);
        }
        else {
            // handle cancel or error
            Debug.Log("Falha ao criar um savedata");
        }
    }*/

    private void ReadSavedGame(string filename)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.OpenWithAutomaticConflictResolution(filename, 
                                                            DataSource.ReadCacheOrNetwork,
                                                            ConflictResolutionStrategy.UseLongestPlaytime,
                                                            OnSavedGameRead);
    }

    private void OnSavedGameRead(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        Debug.Log("(Rocket IX) Saved Game Read: " + status.ToString());
        if (status == SavedGameRequestStatus.Success)
        {
            // handle reading or writing of saved game.
            currentGame = game;
            PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(game,
                                                OnSavedGameDataBinaryRead);
        }
        else {
            // handle error
            isTotalFragmentLocalMoreUpdated = false;
        }
    }

    private void OnSavedGameDataBinaryRead(SavedGameRequestStatus status, byte[] data)
    {
        Debug.Log("(Rocket IX) Saved Game Binary Read: " + status.ToString());

        if (status == SavedGameRequestStatus.Success)
        {
            ShipStatusDataSerialized shipStatusDataSerialized = new ShipStatusDataSerialized();
            try
            {
                shipStatusDataSerialized = (ShipStatusDataSerialized)ByteArrayToObject(data);
            }
            catch (Exception e)
            {
                Debug.Log("(Rocket IX) Saved Game Write: convert exception: " + e.Message);
            }

            //handling level
            if ( shipStatusDataSerialized.handlingLevel >= ShipStatusData.GetHandlingLevel())
            {
                ShipStatusData.SetHandlingLevel(shipStatusDataSerialized.handlingLevel);
            }
            else
            {
                shipStatusDataSerialized.handlingLevel = ShipStatusData.GetHandlingLevel();
            }

            //life
            if (shipStatusDataSerialized.life >= ShipStatusData.GetLife())
            {
                ShipStatusData.SetLife(shipStatusDataSerialized.life);
            }
            else
            {
                shipStatusDataSerialized.life = ShipStatusData.GetLife();
            }

            //magnetic Level
            if (shipStatusDataSerialized.magneticLevel >= ShipStatusData.GetMagneticLevel())
            {
                ShipStatusData.SetMagneticLevel(shipStatusDataSerialized.magneticLevel);
            }
            else
            {
                shipStatusDataSerialized.magneticLevel = ShipStatusData.GetMagneticLevel();
            }

            //rate Shot Level
            if (shipStatusDataSerialized.rateShotLevel >= ShipStatusData.GetRateShotLevel())
            {
                ShipStatusData.SetRateShotLevel(shipStatusDataSerialized.rateShotLevel);
            }
            else
            {
                shipStatusDataSerialized.rateShotLevel = ShipStatusData.GetRateShotLevel();
            }

            //speed Level
            if (shipStatusDataSerialized.speedLevel >= ShipStatusData.GetSpeedLevel())
            {
                ShipStatusData.SetSpeedLevel(shipStatusDataSerialized.speedLevel);
            }
            else
            {
                shipStatusDataSerialized.speedLevel = ShipStatusData.GetSpeedLevel();
            }

            //struct Level
            if (shipStatusDataSerialized.structLevel >= ShipStatusData.GetStructLevel())
            {
                ShipStatusData.SetStructLevel(shipStatusDataSerialized.structLevel);
            }
            else
            {
                shipStatusDataSerialized.structLevel = ShipStatusData.GetStructLevel();
            }

            //best recorde
            if (shipStatusDataSerialized.bestRecord >= ShipStatusData.GetBestRecord())
            {
                ShipStatusData.SetBestRecordUpload(shipStatusDataSerialized.bestRecord);
            }
            else
            {
                shipStatusDataSerialized.bestRecord = ShipStatusData.GetBestRecord();
            }

            if (!isTotalFragmentLocalMoreUpdated)
            {
                if (shipStatusDataSerialized.totalFragments >= ShipStatusData.GetTotalFragments())
                {
                    ShipStatusData.SetTotalFragments(shipStatusDataSerialized.totalFragments);
                }
                else
                {
                    shipStatusDataSerialized.totalFragments = ShipStatusData.GetTotalFragments();
                }
            }
            else
            {
                shipStatusDataSerialized.totalFragments = ShipStatusData.GetTotalFragments();
            }

            //retornando para opção padrão após tentativa de atualização.
            isTotalFragmentLocalMoreUpdated = false;

            byte[] newData = ObjectToByteArray(shipStatusDataSerialized);

            // Write new data
           /* Debug.Log("(Rocket IX) Old Score: " + score.ToString());
            Debug.Log("(Rocket IX) mHits: " + newScoreFake.ToString());
            Debug.Log("(Rocket IX) New Score: " + newScore.ToString());*/
            WriteSavedGame(currentGame, newData);
        }
        else {
            // handle error
            isTotalFragmentLocalMoreUpdated = false;
        }
    }

    private void WriteSavedGame(ISavedGameMetadata game, byte[] savedData)
    {

        SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder()
            .WithUpdatedPlayedTime(TimeSpan.FromMinutes(game.TotalTimePlayed.Minutes + 1))
            .WithUpdatedDescription("Saved at: " + System.DateTime.Now);

        // You can add an image to saved game data (such as as screenshot)
        // byte[] pngData = <PNG AS BYTES>;
        // builder = builder.WithUpdatedPngCoverImage(pngData);

        SavedGameMetadataUpdate updatedMetadata = builder.Build();

        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.CommitUpdate(game, updatedMetadata, savedData, OnSavedGameWriten);
    }

    private void OnSavedGameWriten(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        Debug.Log("(Rock IX) Saved Game Write: " + status.ToString());

        if (status == SavedGameRequestStatus.Success)
        {
            // handle reading or writing of saved game.
            ShopManager.Instance.updateFragmentHolder();
        }
        else {
            // handle error
        }
    }

    private static byte[] ObjectToByteArray(System.Object obj)
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

    private static System.Object ByteArrayToObject(byte[] arrBytes)
    {
        using (var memStream = new MemoryStream())
        {
            var binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var obj = binForm.Deserialize(memStream);
            return obj;
        }
    }
}


