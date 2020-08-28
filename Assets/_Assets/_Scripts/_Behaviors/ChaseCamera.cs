using UnityEngine;
using System.Collections;

public class ChaseCamera : MonoBehaviour {
	public RocketBehavior spaceship;
	Transform chase;
	
	bool chasing = false;
	// Use this for initialization
	void Start () {
		chase = spaceship.transform;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(!chasing)
		{
			if(/*!spaceship.isMoving &&*/ (chase.position.x != transform.position.x || chase.position.z != transform.position.z))
			{
				//startChase = true;
				StartCoroutine(startChase());
			}
		}
		else
		{
			Vector2 dir = chaseV2() - thisV2();
			if(dir.magnitude < 0.1)
			{
				chasing = false;
				Vector3 p = transform.position;
				p.x = chase.position.x;
				p.z = chase.position.z;
				transform.position = p;
			}
			else
			{
				Vector3 v = transform.position + new Vector3( dir.x / 10.0f, 0, dir.y / 10.0f);
				transform.position = v;
				
			}
		}

        if (Input.GetAxis("Vertical") > 0)
        {
            transform.Translate(0, 0, 5);
        }
	}
	
	Vector2 chaseV2()
	{
		return new Vector2(chase.position.x, chase.position.z);		
	}
	
	Vector2 thisV2()
	{
		return new Vector2(transform.position.x, transform.position.z);	
	}
	
	IEnumerator startChase()
	{
		yield return new WaitForSeconds(0.5f);
		
		chasing = true;
	}
}
