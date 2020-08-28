using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GooglePlayLogin : MonoBehaviour
{
    private Text myText;

    GooglePlayManager googlePlayManager;

    void Start()
    {
        myText = GetComponentInChildren<Text>();
        googlePlayManager = ServiceLocator.GetGooglePlayManager();

        if (googlePlayManager.UserIsLogged())
        {
            myText.text = "Logout Google Play";
        }
        else
        {
            myText.text = "Login Google Play";
        }
    }

    public void SigninRoutineCallback(bool success)
    {
        if (success)
        {
            Debug.Log("Logou com sucesso");
            myText.text = "Logout Google Play";
            googlePlayManager.SaveGame();
            ShipStatusData.SetWasLoggedOnGoogle(1);
        }
        else
        {
            Debug.Log("Falha ao logar");
            myText.text = "Login Google Play";
            ShipStatusData.SetWasLoggedOnGoogle(0);
        }
    }

    public void Clicked()
    {
        Debug.Log("Clicado");

        if (!googlePlayManager.UserIsLogged())
        {
            Debug.Log("Tentando logar");

            googlePlayManager.signin(SigninRoutineCallback, false);
        }
        else
        {
            Debug.Log("Deslogando");
            myText.text = "Login Google Play";
            googlePlayManager.signout();
        }
            
    }
}
