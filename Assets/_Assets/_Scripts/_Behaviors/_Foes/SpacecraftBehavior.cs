using UnityEngine;
using System.Collections;

public class SpacecraftBehavior : FoeBehavior 
{
    float turnSide;
    float dist;
    float angle;

	protected override void OwnUpdate()
    {
        dist -= Time.fixedDeltaTime;
        if (dist <= 0)
        	transform.Rotate(Vector3.up, angle * Time.fixedDeltaTime * turnSide);   
        transform.Translate(0, 0, Time.fixedDeltaTime * speed, Space.Self);
	}

	protected override void OwnSetup(int side, Vector3 p)
    {
		speed = Random.Range(2.0f, 5.0f);
		angle = Random.Range(45.0f, 90.0f);

        turnSide = (side == 0 || side == 2) ? -1 : 1;

		dir = finalPos - astPos;
        dist = dir.magnitude/10;
        dir.Normalize();
        transform.forward = new Vector3(dir.x, 0, dir.y);
    }

	protected override void OnDie()
    {
		transform.parent = null;
        GetComponent<Renderer>().enabled = false;
		GetComponent<Collider>().enabled = false;
        exploded = true;
        GetComponentInChildren<ParticleSystem>().Emit(20);
    }
}
