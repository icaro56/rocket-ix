using UnityEngine;
using System.Collections;

public class StartsGenerator : MonoBehaviour 
{
	public GameObject star;

    public Vector2 boundX = new Vector2(-28.0f, 28.0f);
    public Vector2 boundY = new Vector2(-22.0f, 22.0f);
    public float deepness = -500;

	// Use this for initialization
	void Start () 
	{
		/*for(int i = 0; i < 200; i++)
		{
			GameObject go = GameObject.Instantiate(star) as GameObject;
            go.transform.position = new Vector3(Random.Range(boundX.x, boundX.y), deepness, Random.Range(boundY.x, boundY.y));
            go.GetComponent<StarBehavior>().Randomize(1);
            go.transform.parent = this.transform;
		}*/

        for (int i = 0; i < 50; i++)
        {
            GameObject go = GameObject.Instantiate(star) as GameObject;
            go.transform.position = new Vector3(Random.Range(boundX.x, boundX.y), deepness, Random.Range(boundY.x, boundY.y));
            go.GetComponent<StarBehavior>().Randomize(2);
            go.transform.parent = this.transform;
        }

        for (int i = 0; i < 10; i++)
        {
            GameObject go = GameObject.Instantiate(star) as GameObject;
            go.transform.position = new Vector3(Random.Range(boundX.x, boundX.y), deepness, Random.Range(boundY.x, boundY.y));
            go.GetComponent<StarBehavior>().Randomize(3);
            go.transform.parent = this.transform;
        }
	}
}
