using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RocketBehavior : MonoBehaviour 
{

#region Eventos
	public event Action<AchievementType> onCheckAchievement;
    public event Action<int> onScoreUpdate;

#endregion

    public RocketWallet wallet;
    public RocketInterface hud;
    public RocketNavigation nav;

#region Atributos da Nave
    private float _speed;
    public float speed
    { get { return _speed; } }
    
    private float _handling;
    public float handling
    { get { return _handling; } }

    private int _life;
    public int life
    { get { return _life; } }

    private int _maxLife;
    public int maxLife
    { get { return _maxLife; } }

    private int _magneticSize;
    public int magneticSize
    { get { return _magneticSize; } }

   private float _rate;
   public float rate
   { get { return _rate; } }
#endregion

    public float timePlaying = 0;
    public int totalScore = 0;

	public ShieldBehavior shield;
    public GameObject bullet;
    public Transform shotOrigin;
    public GameObject myPoolerObject;
    private NewObjectPoolerScript myPoolerScript;

    private Material _material;
	   
    bool gameOver = false;
	bool paused = false;
	bool shielded = false;

    //float deltaTime = 0.0f;

	void Awake () 
	{
        myPoolerScript = myPoolerObject.GetComponent<NewObjectPoolerScript>();
        hud = GetComponent<RocketInterface>();
        nav = GetComponent<RocketNavigation>();
        wallet = new RocketWallet();

        GameData gd = ServiceLocator.GetGameData();

        _speed = gd.attributes.Speed[ShipStatusData.GetSpeedLevel()].Value;
        nav.speed = speed;
       
        _handling = gd.attributes.Handling[ShipStatusData.GetHandlingLevel()].Value;
        nav.handling = handling;

        _life = (int)gd.attributes.Life[ShipStatusData.GetStructLevel()].Value;
        _maxLife = life;
        
        _rate = gd.attributes.Rate[ShipStatusData.GetRateShotLevel()].Value;
        hud.UpdateDPS(rate);

        _magneticSize = (int)gd.attributes.Magnetic[ShipStatusData.GetMagneticLevel()].Value;

        _material = GetComponent<Renderer>().sharedMaterials[0];
    }

    void Start()
    {
        StartCoroutine(GenerateBullet());

        AchievementManager achievemgr = ServiceLocator.GetAchievementManager();
        achievemgr.Setup();
    }

    void OnEnable()
    {
        AchievementManager achievemgr = ServiceLocator.GetAchievementManager();
        onCheckAchievement += achievemgr.CheckAchievementOf;
        wallet.onCheckAchievement += achievemgr.CheckAchievementOf;
        nav.onCheckAchievement += achievemgr.CheckAchievementOf;

        if (onCheckAchievement != null)
            onCheckAchievement(AchievementType.Upgrade);

        wallet.onFragmentsAmountChanged += hud.UpdateFrag;

        GameManager.Instance.onReborn += OnReborn;
        GameManager.Instance.onGameOver += OnGameOver;
        GameManager.Instance.onPause += delegate(bool p) { paused = p; };
    }

    void OnDisable()
    {
        AchievementManager achievemgr = ServiceLocator.GetAchievementManager();
        onCheckAchievement -= achievemgr.CheckAchievementOf;
        wallet.onCheckAchievement -= achievemgr.CheckAchievementOf;
        nav.onCheckAchievement -= achievemgr.CheckAchievementOf;

        wallet.onFragmentsAmountChanged -= hud.UpdateFrag;

        GameManager.Instance.onReborn -= OnReborn;
        GameManager.Instance.onGameOver -= OnGameOver;
        GameManager.Instance.onPause -= delegate(bool p){ paused = p; };
    }

    void FixedUpdate()
    {
        if (!gameOver && !paused)
        {
            nav.Move();

            int curScore = (wallet.fragments * 10) + ((int)timePlaying * 10);
            hud.UpdateScore(curScore);
            if (onScoreUpdate != null)
                onScoreUpdate(curScore);
        }
    }

	void Update () 
    {
        if (!gameOver && !paused)
        {
            timePlaying += Time.deltaTime;

            if (Input.GetMouseButtonDown(0))
                nav.Touched(Input.mousePosition);

            if (Input.touchCount > 0)
                nav.Touched(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y));

            //usado no fps
            //deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        }
	}
		
    void OnTriggerEnter(Collider other)
    {
		if (other.CompareTag("asteroid") || other.CompareTag("alienmissile"))
		{
			if(shielded)
			{
				shielded = false;
				shield.collisionEnterTurnOff();
			}
			else
			{
				nav.totalSecsInSameSpot = 0.0f;
                nav.distanceFlew = 0.0f;
				--_life;
				
                if(life <= 0)
                {    
                    hud.UpdateLife(0); 
                    Destroyed();    
                }
                else if(life == 1)
                {
                     hud.UpdateLife(1);
                     StartCoroutine(Damaged());
                }
                else
                {
                    float perclife = ((float)(life - 1) / (float)(maxLife - 1)) * 100;
                    hud.UpdateLife((int)perclife);
                    StartCoroutine(Damaged());
				}
			}
		}
    }

    // Chamado quando é game over
    void OnGameOver()
    {
        StartCoroutine(Die());
    }

    // Chamado quando é revivido ao assistir o video
    void OnReborn()
    {
        _material.color = Color.white;
        _life = 1;
        hud.UpdateLife(1);

        transform.FindChild("FireCentral").gameObject.SetActive(true);
        transform.FindChild("jetLeft").transform.FindChild("FireLeft").gameObject.SetActive(true);
        transform.FindChild("jetRight").transform.FindChild("FireRight").gameObject.SetActive(true);
        nav.enabled = true;

        GetComponent<Renderer>().enabled = true;
        transform.FindChild("jetLeft").GetComponent<Renderer>().enabled = true;
        transform.FindChild("jetRight").GetComponent<Renderer>().enabled = true;
        GetComponent<Collider>().enabled = true;

        gameOver = false;
        StartCoroutine(GenerateBullet());

    }

    // Chamado quando vida chega a 0. É o novo Die.
    void Destroyed()
    { 
        totalScore = (wallet.fragments * 10) + ((int)timePlaying * 10);

        if (totalScore > ShipStatusData.GetBestRecord())
            ShipStatusData.SetBestRecord(totalScore);

        GooglePlayManager gm = ServiceLocator.GetGooglePlayManager();
        gm.publishScore(totalScore);
        gm.SaveGame();

        if (onCheckAchievement != null)
            onCheckAchievement(AchievementType.GameOver);

        //GameManager.Instance.ShipDestroyed(totalScore, wallet.fragments);
        GameManager.Instance.GameOver(totalScore, wallet.fragments);
    }

#region Métodos de Atributos da Nave
    public void ActiveShield(string kind)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).tag == "shield" && 
                transform.GetChild(i).gameObject.activeSelf && 
                kind != transform.GetChild(i).name)
            {
                transform.GetChild(i).SendMessage("TurnOff");
            }
        }
        transform.FindChild(kind).gameObject.SetActive(true);
        transform.FindChild(kind).SendMessage("TurnOn");
		shielded = true;
    }

    public void AddLife()
    {
        if (life == maxLife)
			return;
		
		_life++;
        //atualizando interface
        float perclife = ((float)(life - 1) / (float)(maxLife - 1)) * 100;
        hud.UpdateLife((int)perclife);
    }
#endregion

#region Coroutines
    IEnumerator Die()
    {
        GameObject.FindGameObjectWithTag("shipexplosion").GetComponent<ParticleSystem>().Play();
        //Destroy(transform.FindChild("FireCentral").gameObject);
        transform.FindChild("FireCentral").gameObject.SetActive(false);
        //Destroy(transform.FindChild("jetLeft").transform.FindChild("FireLeft").gameObject);
        transform.FindChild("jetLeft").transform.FindChild("FireLeft").gameObject.SetActive(false);
        //Destroy(transform.FindChild("jetRight").transform.FindChild("FireRight").gameObject);
        transform.FindChild("jetRight").transform.FindChild("FireRight").gameObject.SetActive(false);
        //Destroy(nav);
        nav.enabled = false;
        //jets = new JetBehavior[0];

        // Código abaixo mudado para o método Destroyed
        //Calculando o score da fase
        totalScore = (wallet.fragments * 10) + ((int)timePlaying * 10);

        if (totalScore > ShipStatusData.GetBestRecord())
        {
            ShipStatusData.SetBestRecord(totalScore);
        }

        GooglePlayManager gm = ServiceLocator.GetGooglePlayManager();
        gm.publishScore(totalScore);
        gm.SaveGame();

        yield return new WaitForEndOfFrame();

        GetComponent<Renderer>().enabled = false;
        transform.FindChild("jetLeft").GetComponent<Renderer>().enabled = false;
        transform.FindChild("jetRight").GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        gameOver = true;

        //GameObject.FindGameObjectWithTag("gamemanager").SendMessage("GameOver");
        //GameManager.Instance.GameOver(totalScore, wallet.fragments);

        //if(onCheckAchievement != null)
        //    onCheckAchievement(AchievementType.GameOver);

        StopAllCoroutines();
    }

    IEnumerator GenerateBullet()
    {
        while (!gameOver)
        {
            if (!paused)
            {
                GameObject obj = myPoolerScript.GetPooledObject();
                if (obj != null)
                {
                    obj.transform.position = shotOrigin.position;
                    obj.transform.rotation = shotOrigin.rotation;
                    obj.SetActive(true);
                }
            }
            yield return new WaitForSeconds(rate);
        }
    }

    IEnumerator Damaged()
    {
        _material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _material.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        _material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _material.color = Color.white;
    }
#endregion

	//DEBUG
    /*void OnGUI()
    {
        if (!gameOver)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 25;
            style.normal.textColor = Color.white;

            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.} fps", fps);

            GUI.Label(new Rect(10, 10, 100, 100), text, style);
        }
    }*/
}
