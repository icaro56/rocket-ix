using UnityEngine;
using System.Collections;

public class FoeBehavior : MonoBehaviour 
{
	public bool exploded = false;
	protected bool paused = false;

	protected Vector2 finalPos;
	protected Vector2 shipPos;
	protected Vector2 astPos;

	public Vector2 dir;
	protected Vector2 rot;
	public float speed;

	static protected GameObject rocket = null;
    static protected RocketBehavior rocketBehavior = null;
    static protected float explosionClipLength;
    static protected AudioManager audioManager = null;

	#region CONSTANTES DO MUNDO
	protected float yMin = -22;
	protected float yMax = 22;
	protected float xMin = -28.5f;
	protected float xMax = 28.5f;
	#endregion

	// Use this for initialization
	void Awake()
	{
        if(rocket == null)
		    rocket = GameObject.FindGameObjectWithTag("rocket") as GameObject;

        if (rocketBehavior == null)
            rocketBehavior = rocket.GetComponent<RocketBehavior>();
        
        if (audioManager == null)
            audioManager = ServiceLocator.GetAudioManager();

        OwnAwake();
        explosionClipLength = audioManager.GetClipLength(AudioManager.Clips.COLLISION);
    }

	protected void Start () 
	{
        // TODO change when AudioManager is implemented.
        //GameData gd = ServiceLocator.GetGameData();
        //clip = GetComponent<AudioSource>();
        //clip.clip = gd.clipColision;
        //clip.volume = 0.5f * gd.GetOverallVolume();

        if (GameObject.FindGameObjectWithTag("gamemanager") != null)
		{
			GameManager.Instance.onPause += TooglePause;
			GameManager.Instance.onGameOver += GameOver;
		}
	}

	protected void FixedUpdate()
	{
		if(paused) return;

		if(!exploded)
			OwnUpdate();
	}

	protected void TooglePause(bool isPaused)
	{
		paused = isPaused;
	}

    protected virtual void OwnAwake()
    { }

	protected virtual void OwnUpdate()
	{}

	protected virtual void OwnSetup(int side, Vector3 p)
	{}
		
	public void Setup(int side, Vector3 p)
	{
		transform.position = p;
		transform.localRotation = Quaternion.identity;
		exploded = false;

		GetComponent<Renderer>().enabled = true;
		GetComponent<Collider>().enabled = true;

		shipPos = rocketBehavior.nav.ToVec2();
		finalPos = shipPos;
		astPos = ToVec2();

		float targetOffset = 0;
		//Up Or Down
		if (side == 0 || side == 2)
		{
			if (astPos.x > shipPos.x)
				targetOffset = Random.Range(0, xMin - -astPos.x);
			else
				targetOffset = Random.Range(0, xMax + -astPos.x);
			finalPos.x += targetOffset;
		}
		else 
		{
			if (astPos.y > shipPos.y)
				targetOffset = Random.Range(0, yMin - -astPos.y);
			else
				targetOffset = Random.Range(0, yMax + -astPos.y);
			finalPos.y += targetOffset;
		}
		dir = finalPos - astPos;
		dir.Normalize();

		OwnSetup(side, p);
	}

	protected Vector2 ToVec2()
	{
		return new Vector2(transform.position.x, transform.position.z);
	}

	void OnTriggerEnter(Collider other)
	{ 
		if (other.tag == "rocket" || other.tag == "shield" || other.tag == "missile")
		{
			StartCoroutine(Die());
		}
		else if (other.tag == "boundary")
		{
			transform.parent = null;
			gameObject.SetActive(false);
		}
	}

    protected virtual void OnDie()
    { }

	protected virtual IEnumerator Die()
	{
        OnDie();

        audioManager.Register(AudioManager.Clips.COLLISION);
        yield return new WaitForSeconds(explosionClipLength);
        //clip.Play();
        //while(clip.isPlaying)
        //	yield return new WaitForEndOfFrame();


        gameObject.SetActive(false);
        //Destroy(gameObject);
	}

	public void GameOver()
	{
		gameObject.SetActive(false);
		//StartCoroutine(Die());
	}
}
