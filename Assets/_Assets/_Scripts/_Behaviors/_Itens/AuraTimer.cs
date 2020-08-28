using UnityEngine;
using System.Collections;

public class AuraTimer : MonoBehaviour 
{
	float time = 0;
	bool expanding = true;
    bool staying = false;
	bool paused = false;

	void Start () 
	{
		//! Não pode testar se Instance == null, pois is faz criar um novo.
		if(GameObject.FindGameObjectWithTag("gamemanager") != null)
			GameManager.Instance.onPause += TooglePause;
	}

	void TooglePause(bool isPaused)
	{
		paused = isPaused;
	}

	void Update () 
	{
		if(paused) return;

        if (!staying)
        {
            if (expanding)
            {
                time += Time.deltaTime * 2;
                if (time >= 2) StartCoroutine(Stay());//expanding = false;
            }
            else
            {
                time -= Time.deltaTime * 2;
                if (time <= 0.5) StartCoroutine(Stay());//expanding = true;
            }
        }

        GetComponent<Renderer>().material.SetFloat("_AllPower", time * 2);
	}

    IEnumerator Stay()
    {
        staying = true;

        yield return new WaitForSeconds(1.0f);
        staying = false;
        expanding = !expanding;
    }
}
