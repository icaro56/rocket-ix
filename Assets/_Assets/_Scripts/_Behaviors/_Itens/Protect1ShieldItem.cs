using UnityEngine;
using System.Collections;
using System;

public class Protect1ShieldItem : ItemBehavior
{
    public event Action<string> onActiveShield;

	void FixedUpdate () 
	{
        OwnUpdate();

        if (spaceship != null)
            onActiveShield += spaceship.ActiveShield;
	}

	protected override void OwnSetup()
	{
		speed = UnityEngine.Random.Range(1.0f, 3.0f);
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "rocket")
        {
            if (onActiveShield != null)
                onActiveShield("protection1");
            TurnOff();
        }
        else if (other.tag == "boundary")
        {
            //Destroy(gameObject);
			transform.parent = null;
			gameObject.SetActive(false);
		}
    }
}
