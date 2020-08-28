using UnityEngine;
using System.Collections;

public class ItemBehavior : MonoBehaviour 
{
    protected Vector2 dir;
    protected float speed;
    protected bool taken;
    protected bool follow;
    protected float timeFollowing;
	protected bool paused;
	//protected AudioSource clip;
    static protected RocketBehavior spaceship = null;
    static protected AudioManager audioManager = null;
    static protected float pickupClipLength;

    #region CONSTANTES DO MUNDO
    float yMin = -22;
    float yMax = 22;
    float xMin = -28.5f;
    float xMax = 28.5f;
    #endregion

	void Awake()
	{
        if (spaceship == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag("rocket");
            if (go)
                spaceship = go.GetComponent<RocketBehavior>();
        }
	
        if (audioManager == null)
            audioManager = ServiceLocator.GetAudioManager();

        pickupClipLength = audioManager.GetClipLength(AudioManager.Clips.PICKUP);

        if (GameObject.FindGameObjectWithTag("gamemanager") != null)
        {
            GameManager.Instance.onPause += TooglePause;
            GameManager.Instance.onGameOver += GameOver;
        }
	}

	protected void TooglePause(bool isPaused)
	{
		paused = isPaused;
	}

    protected void OwnUpdate()
    {
		if(paused) return;

        if (spaceship == null)  return;

        if (follow)
        {
            timeFollowing += Time.fixedDeltaTime;
            Vector2 pos = (1 - timeFollowing) * this.ToVec2() + timeFollowing * spaceship.nav.ToVec2();
            Vector3 v = new Vector3(pos.x, 0, pos.y);
            transform.position = v;  
        }
        else
        {
			if (Vector3.Distance(spaceship.transform.position, transform.position) <= spaceship.magneticSize)
				follow = true;

            if (!taken)
                transform.Translate(dir.x * speed * Time.fixedDeltaTime, 0, dir.y * speed * Time.fixedDeltaTime, Space.World);
        }   
    }

	protected virtual void OwnSetup()
	{}

    public void Setup(int[] values)
    {
		taken = false;
		follow = false;
		timeFollowing = 0.0f;
		transform.localScale = new Vector3(1.5f,1.5f,1.5f);

        Vector2 itemPos = ToVec2();
        Vector2 finalPos = itemPos;

        //Tilt faz com que o item siga numa direção reta, em vez de ir para cima da nave
        int tilt = Random.Range(0, 100);
        if (tilt < 50)
        {
            switch (values[0])
            { 
                //Up
                case 0: finalPos.y -= 10; break;
                //Right
                case 1: finalPos.x -= 10; break;
                //Down
                case 2: finalPos.y += 10; break;
                //Left
                case 3: finalPos.x += 10; break;
            }
        }
        else
        {
            Vector2 shipPos = spaceship.nav.ToVec2();
            finalPos = shipPos;
            float targetOffset = 0;
            //Up Or Down
            if (values[0] == 0 || values[0] == 2)
            {
                if (itemPos.x > shipPos.x)
                {
                    targetOffset = Random.Range(-(xMin - -itemPos.x), xMin - -itemPos.x);
                }
                else
                {
                    targetOffset = Random.Range(-(xMax + -itemPos.x), xMax + -itemPos.x);
                }
                finalPos.x += targetOffset;
            }
            else
            {
                if (itemPos.y > shipPos.y)
                {
                    targetOffset = Random.Range(-(yMin - -itemPos.y), yMin - -itemPos.y);
                }
                else
                {
                    targetOffset = Random.Range(-(yMax + -itemPos.y), yMax + -itemPos.y);
                }
                finalPos.y += targetOffset;
            }
        }
        dir = finalPos - itemPos;
        dir.Normalize();

		OwnSetup();
    }

    public Vector2 ToVec2()
    {
        return new Vector2(transform.position.x, transform.position.z);
    }

    public void TurnOn()
    {
        transform.localScale = new Vector3(0, 0, 0);
        gameObject.SetActive(true);
        StartCoroutine(Show());
    }

    public void TurnOff()
    {
        taken = true;
        StartCoroutine(Hide());
    }

    protected IEnumerator Show()
    {
		while(transform.localScale.x < 1.0f)
		{
	        float grow = Time.deltaTime * 2;
	        transform.localScale = transform.localScale + new Vector3(grow, grow, grow);

	        yield return new WaitForEndOfFrame();
		}
    }

    protected IEnumerator Hide()
    {
        audioManager.Register(AudioManager.Clips.PICKUP);
		//clip.Play();   
		//while(transform.localScale.x < 2.0f)
        float played = pickupClipLength;
		while(played > 0)
		{
			float shrink = Time.deltaTime * 10;
			transform.localScale = transform.localScale + new Vector3(shrink, shrink, shrink);
        	yield return new WaitForEndOfFrame();
            played -= Time.deltaTime;
        }
        //Destroy(gameObject);
		transform.parent = null;
		gameObject.SetActive(false);
	}

    public void GameOver()
    {
		gameObject.SetActive(false);
        //TurnOff();
		//Destroy(gameObject);
    }

}
