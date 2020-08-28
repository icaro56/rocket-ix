using UnityEngine;
using System.Collections;

public class AlienBullet : MonoBehaviour 
{
    public float speed = 50;

    Rigidbody _rigidbody;
    Animator _anim;

    bool alive = false;
	bool paused = false;

    static AudioManager audioManager = null;

	void Awake()
	{
        _anim = GetComponentInChildren<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        
        if (audioManager == null)
            audioManager = ServiceLocator.GetAudioManager();
	}

	void OnEnable () 
	{
		GameManager.Instance.onPause += TooglePause;
        Setup();
	}

    void OnDisable()
    {
        GameManager.Instance.onPause -= TooglePause;
        CancelInvoke();
    }

	void TooglePause(bool isPaused)
	{
		paused = isPaused;
	}

    void Destroy()
    {
        alive = false;
        _rigidbody.Sleep();
        gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if(paused) return;

	    if (alive)
        {
            _rigidbody.AddRelativeForce(Vector3.forward * speed * Time.deltaTime, ForceMode.VelocityChange);
        }
	}

    void Setup()
    {
        _anim.SetTrigger("Alive");
        alive = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("asteroid"))
        {
            _anim.SetTrigger("Destroy");
            Invoke("Destroy", 0.25f); 
        }
    }



}
