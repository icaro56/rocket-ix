using UnityEngine;
using System.Collections;

public class FollowerBehavior : FoeBehavior
{
    static private int countCreated = 0;
    static private float speedInc = 0.0f;

    protected override void OwnUpdate()
    {
        Vector2 dir = rocketBehavior.nav.ToVec2() - this.ToVec2();
        dir.Normalize();
        transform.forward = new Vector3(dir.x, 0, dir.y);
        transform.Translate(0, 0, Time.fixedDeltaTime * speed, Space.Self);
    }

    protected override void OwnSetup(int side, Vector3 p)
    {
        countCreated++;
        speedInc = Mathf.Min(countCreated * 0.02f, 3.5f);

        speed = Random.Range(0.5f + speedInc, 1.0f + speedInc);
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
