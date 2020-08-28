using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Advertisements;

public class GameManager : Singleton<GameManager> 
{
	#region Events

	public event Action onGameOver;
	public event Action<bool> onPause;
    public event Action onReborn;

	#endregion 
    
	#region Members
	public GameObject pauseButton;
	public GameObject pauseScreen;
	public GameObject gameoverScreen;
    public GameObject watchVideoButton;
	GameOverHUD gameoverHUD;

	Animator pauseScreenAnim;
	Animator pauseButtonAnim;

	//bool gameOver = false;
	bool paused = false;

	#endregion

	void Start()
	{
		if(gameoverScreen != null)
		{
			gameoverHUD = gameoverScreen.GetComponent<GameOverHUD>();
			gameoverScreen.SetActive(false);
		}

		if(pauseButton != null)
			pauseButtonAnim = pauseButton.GetComponent<Animator>();

		if(pauseScreen != null)
		{
			pauseScreenAnim = pauseScreen.GetComponent<Animator>();
			pauseScreenAnim.SetTrigger("Resumed");
		}
	}

	public void TooglePause()
	{
		paused = !paused;

		pauseButtonAnim.SetTrigger(paused ? "Paused" : "Resumed");
		pauseScreenAnim.SetTrigger(paused ? "Paused" : "Resumed");

        if (onPause != null)
        {
            onPause(paused);
        }

        //if (paused)
        //    ServiceLocator.GetAdManager().ShowAd();
    }

    // Chamado quando nave é destruída.
    public void ShipDestroyed(int Score, int Fragments)
    {
        pauseButton.SetActive(false);
        gameoverScreen.SetActive(true);
        gameoverHUD.SetValues(Score, Fragments);
        // Coloca os objetos em pausa, enquanto jogador decide o que fazer.
        if (onPause != null)
            onPause(true);
    }

    // Inutilizado.
	public void GameOver(int Score, int Fragments)
    {
        //gameOver = true;

		pauseButton.SetActive(false);
		gameoverScreen.SetActive(true);
		gameoverHUD.SetValues(Score, Fragments);

        if (onGameOver != null)
			onGameOver();
    }

    // Chamado para reviver a nave depois que assistir o video
    public void Reborn()
    {
        pauseButton.SetActive(true);
        gameoverScreen.SetActive(false);
        //if (onPause != null)
        //    onPause(false);

        if (onReborn != null)
            onReborn();
    }

    // Função que é chamado ao clicar no botao WatchVideo
    public void WatchVideo()
    {
        ServiceLocator.GetAdManager().ShowRewardedAd(WatchAdCallbackHandler);
    }

    void WatchAdCallbackHandler(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Reborn();
                // Só deixa assistir um video por gameplay
                watchVideoButton.SetActive(false);
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
