using UnityEngine;
using System.Collections;
using System;

public class RocketNavigation : MonoBehaviour
{
    public struct Coord
    {
        public float x;
        public float y;
        public Coord(int x, int y) { this.x = x; this.y = y; }
        public Coord(Vector2 p) { x = p.x; y = p.y; }
        public Coord(Vector3 p) { x = p.x; y = p.z; }
        public Vector2 ToVec2() { return new Vector2(x, y); }
        public Vector3 ToVec3() { return new Vector3(x, 0, y); }

    };

    #region Eventos
    public event Action<AchievementType> onCheckAchievement;

    #endregion

    // pointer animator, mostra onde foi o clique.
    Animator _pointerAnim;
    Transform _pointerTrans;

    public JetBehavior[] jets;

    public float totalSecsInSameSpot = 0;
    public float distanceFlew = 0;

    public float speed;
    public float handling;

    Coord coord;
    float time;
    //float distance;
    bool clockWise;
    bool isMoving = false;
    bool isHandling = false;
    float turnAngleLimit = 30.0f;
    float rangeAngleBoostSpeed = 15.0f;
    float maxVelocity = 20;
    float myVelocity;
    Rigidbody _rigidbody;
    Transform _transform;
    float angle;

    //bool floatLeft = true;
   // bool checkFloat = true;

    void Awake()
    {
        GameObject pointer = GameObject.Find("Pointer");
        if(pointer != null)
        {
            _pointerAnim = pointer.GetComponent<Animator>();
            _pointerTrans = pointer.GetComponent<Transform>();
        }
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
    }

    public void Touched(Vector3 v)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(v);
        if (Physics.Raycast(ray, out hit))
            SetCoord(hit.point);
    }

    void SetCoord(Vector3 p)
    {
        coord = new Coord(p);
        time = 0.0f;

        //Descobrindo quantos graus tem que girar para manobrar
        Vector2 dir = coord.ToVec2() - ToVec2();
        Vector2 forward = new Vector2(_transform.forward.x, _transform.forward.z);
        angle = Vector2.Angle(forward.normalized, dir.normalized);

        //Verificando para qual lado tem que girar
        Vector3 dummie1 = new Vector3(_transform.forward.x, _transform.forward.z, 0);
        Vector3 dummie2 = new Vector3(dir.x, dir.y, 0);
        if (Vector3.Cross(dummie1, dummie2).z < 0)
            angle = -angle;

        clockWise = !(angle > 0);

        isHandling = true;

        if ((angle < turnAngleLimit) && (angle > -turnAngleLimit))
            isMoving = true;
        else
            isMoving = false;

        //Limpando controles de achivements
        totalSecsInSameSpot = 0.0f;
        distanceFlew = 0.0f;

        for (int i = 0; i < jets.Length; i++)
        {
            StartCoroutine(jets[i].GetOn());
        }

        // Mostrando o pointer
        _pointerTrans.position = p;

        if (!_pointerAnim.GetCurrentAnimatorStateInfo(0).IsName("PointerShow"))
            _pointerAnim.SetTrigger("Show");
    }

    public void Move()
    {
        //Girando a nave
        if (isHandling || isMoving)
        {
            if (isHandling)
            {
                //Descobrindo quantos graus tem que girar para manobrar
                Vector2 dir = coord.ToVec2() - ToVec2();
                Vector2 forward = new Vector2(_transform.forward.x, _transform.forward.z);
                angle = Vector2.Angle(forward.normalized, dir.normalized);

                //Verificando para qual lado tem que girar
                Vector3 dummie1 = new Vector3(_transform.forward.x, _transform.forward.z, 0);
                Vector3 dummie2 = new Vector3(dir.x, dir.y, 0);
                if (Vector3.Cross(dummie1, dummie2).z < 0)
                    angle = -angle;

                int side = (angle > 0) ? -1 : 1;

                //transform.Rotate(Vector3.up, handling * Time.deltaTime * side);
                Quaternion deltaRotation = Quaternion.Euler(Vector3.up * handling * side * Time.deltaTime);
                _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);

                /* if (distance > 4000)
                     transform.Translate(0, 0, Time.deltaTime * 2, Space.Self);*/

                if ((side == 1 && angle > -1) || (side == -1 && angle < 1) || 
                    (clockWise && side == -1) || (!clockWise && side == 1))
                {
                    isHandling = false;
                    //isMoving = true;
                }

                if ((angle < turnAngleLimit) && (angle > -turnAngleLimit))
                    isMoving = true;
                else
                    isMoving = false;
            }
            //Movendo
            if (isMoving)
            {
                float currDis = Vector2.Distance(this.ToVec2(), coord.ToVec2());
                //float currDis = (this.ToVec2() - coord.ToVec2()).magnitude;

                if (currDis > 4)
                {
                    time += speed /*/ distance*/ * Time.deltaTime;
                    Vector2 pos = (1 - time) * this.ToVec2() + time * coord.ToVec2();
                    Vector3 v = new Vector3(pos.x, 0, pos.y);

                    distanceFlew += Vector3.Distance(_transform.position, v);


                    //float closestFactor = currDis < 5 ? 0.25f : currDis * Time.deltaTime;
                    float frontFactor = angle < rangeAngleBoostSpeed || angle > -rangeAngleBoostSpeed ? 3.0f : 1.0f;

                    currDis = currDis > 15 ? 15 : currDis;
                    float closestFactor = Time.deltaTime * currDis * frontFactor;

                    myVelocity = _rigidbody.velocity.magnitude;
                    if (myVelocity < maxVelocity)
                        _rigidbody.AddRelativeForce(Vector3.forward * speed * closestFactor * Time.deltaTime);
                }
                else
                {
                    isMoving = false;
                    for (int i = 0; i < jets.Length; i++)
                    {
                        StartCoroutine(jets[i].GetDown());
                    }
                    _pointerAnim.SetTrigger("Hide");
                }

                // checa Achievement de DISTANCE
                if (onCheckAchievement != null)
                {
                    onCheckAchievement(AchievementType.Distance);
                }
                totalSecsInSameSpot = 0.0f;
            }
        }
        else
        {
            //Achievement Break to coffe/ Watching a sitcom
            totalSecsInSameSpot += Time.deltaTime;
            //  checa Achievement de TIME
            if (onCheckAchievement != null)
                onCheckAchievement(AchievementType.Time);

        }
    }

    public Vector2 ToVec2()
    {
        return new Vector2(_transform.position.x, _transform.position.z);
    }
}
