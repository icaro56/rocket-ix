using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AchievementInfo : MonoBehaviour 
{

    private bool showingUp = false;

    public void ShowUp(string title)
    {
        if(!showingUp)
        {
            showingUp = true;
            transform.FindChild("AchievementHolder").transform.FindChild("Title").GetComponent<Text>().text = title;
            GetComponent<Animation>().Play();
            StartCoroutine(Done());
        }
        else
        {
            StartCoroutine(WaitToShow(title));
        }
    }

    //Depois da animação libera a flag para mostrar outro achievement
    IEnumerator Done()
    {
        yield return new WaitForSeconds(4.0f);
        showingUp = false;
    }

    //Chamado se já tiver mostrando algum outro achievement
    IEnumerator WaitToShow(string title)
    {
        yield return new WaitForSeconds(2.0f);
        ShowUp(title);
    }


    
}
