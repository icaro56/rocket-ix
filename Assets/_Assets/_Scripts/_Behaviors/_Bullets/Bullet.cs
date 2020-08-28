using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
    private bool alive = false;
    private Rigidbody myRigidbody;
	private AudioSource clip;
    public float speed = 50;

	bool paused = false;

    static AudioManager audioManager = null;

	void Awake()
	{
        if (audioManager == null)
            audioManager = ServiceLocator.GetAudioManager();

		clip = GetComponent<AudioSource>();
	}

	void Start () 
	{
        myRigidbody = GetComponent<Rigidbody>();

        // TODO change when AudioManager is implemented.
        //GameData gd = ServiceLocator.GetGameData();
        //clip.clip = gd.clipShootLaser;
        //clip.volume = 0.05f * gd.GetOverallVolume();
        if (audioManager != null)
            audioManager.Register(AudioManager.Clips.LASERSHOOT);

		GameManager.Instance.onPause += TooglePause;
	}

	void TooglePause(bool isPaused)
	{
		paused = isPaused;
	}

    void OnEnable()
    {
        Setup();
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    void Destroy()
    {
        alive = false;
        myRigidbody.Sleep();
        gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if(paused) return;

	    if (alive)
        {
            myRigidbody.AddRelativeForce(Vector3.forward * speed * Time.deltaTime, ForceMode.VelocityChange);
        }
	}

    void Setup()
    {
        alive = true;
		clip.Play();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "asteroid")
        {
            Destroy();
        }
        else if (collision.gameObject.tag == "boundary")
        {
            Destroy();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "boundary")
        {
            Destroy();
        }
    }



}
