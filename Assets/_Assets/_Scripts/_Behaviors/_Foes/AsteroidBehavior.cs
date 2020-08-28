using UnityEngine;
using System.Collections;

public class AsteroidBehavior : FoeBehavior 
{
	protected override void OwnUpdate ()
    {
        transform.Rotate(Vector3.up, rot.x * Time.fixedDeltaTime);
        transform.Rotate(Vector3.right, rot.y * Time.fixedDeltaTime);

        transform.Translate(dir.x * speed * Time.fixedDeltaTime, 0, dir.y * speed * Time.fixedDeltaTime, Space.World);
	}

	protected override void OwnSetup(int side, Vector3 p)
    {
		rot.x = Random.Range(-15, 15);
		rot.y = rot.x - Random.Range(0, 30);
		speed = Random.Range(1.0f, 3.0f);
    }

	protected override void OnDie()
    {
		transform.parent = null;
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        exploded = true;
        GetComponentInChildren<ParticleSystem>().Emit(50);
    }
}
