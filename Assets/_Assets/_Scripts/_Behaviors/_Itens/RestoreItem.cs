using UnityEngine;
using System.Collections;

public class RestoreItem : ItemBehavior
{
    Transform itemShape;

	// Update is called once per frame
	void FixedUpdate ()
	{
        OwnUpdate();

		if(!paused)
        	itemShape.Rotate(Vector3.up, 45 * Time.fixedDeltaTime, Space.Self);
	}

	protected override void OwnSetup()
	{
		itemShape = transform.FindChild("item");
		speed = Random.Range(1.0f, 3.0f);
	}

    void OnTriggerEnter(Collider other)
    {
		// talvez seja melhor colocar no RocketBehavior
        if (other.tag == "rocket")
        {
            GameObject.FindGameObjectWithTag("rocket").SendMessage("AddLife");
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
