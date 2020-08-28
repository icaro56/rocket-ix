using UnityEngine;
using System.Collections;

public class ShooterBehavior : FoeBehavior
{
    NewObjectPoolerScript pool;
    float shootRate = 2;
    // distancia que vai viajar antes de ficar estacionado só atirando
    float dist;
    Transform shooterTransform;

    void OnEnable()
    {
        StartCoroutine(Shoot());
    }

    void OnDisable()
    {
        StopCoroutine(Shoot());
    }

    protected override void OwnAwake()
    {
        pool = gameObject.GetComponent<NewObjectPoolerScript>();
        shooterTransform = transform.FindChild("Shooter").transform;
    }

    protected override void OwnUpdate()
    {
        dist -= Time.fixedDeltaTime;
        if (dist > 0)
            transform.Translate(0, 0, Time.fixedDeltaTime * speed, Space.Self);
        else
        {
            Vector2 dir = rocketBehavior.nav.ToVec2() - this.ToVec2();
            dir = dir.normalized * Time.fixedDeltaTime;
            transform.forward = transform.forward + new Vector3(dir.x, 0, dir.y);
        }
    }

    protected override void OwnSetup(int side, Vector3 p)
    {
        speed = Random.Range(2.0f, 3.0f);
        shootRate = Random.Range(3.0f, 5.0f);

        dir = finalPos - astPos;
        dist = dir.magnitude / 5;
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
        StopCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            while (!paused)
            {
                GameObject obj = pool.GetPooledObject();
                if (obj != null)
                {
                    obj.transform.position = shooterTransform.position;
                    obj.transform.rotation = shooterTransform.rotation;
                    obj.SetActive(true);
                }
                yield return new WaitForSeconds(shootRate);
            }
        }
    }
}
