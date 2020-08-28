using UnityEngine;
using System.Collections;

public class AsteroidGenerator : MonoBehaviour {
    //public GameObject[] asteroids;
    //public GameObject[] meteors;

    public NewObjectPoolerScript asteroids;
    public NewObjectPoolerScript meteor;
    public NewObjectPoolerScript alienSpaceCraft;
    public NewObjectPoolerScript alienFollower;
    public NewObjectPoolerScript alienShooter;

	int quota = 25;
    int asteroidGenerated = 0;
    float timer = 0.0f;
	bool paused = false;

    int nextShooterSpawn = 1000;

    #region CONSTANTES DO MUNDO
    float yMin = -25;
    float yMax = 25;
    float xMin = -39.5f;
    float xMax = 39.5f;
    #endregion

	// Use this for initialization
	void Start () 
    {
		// Se cadastrando no evento.
		GameManager.Instance.onGameOver += GameOver;
		GameManager.Instance.onPause += TooglePause;
        GameManager.Instance.onReborn += Reborn;

        RocketBehavior r = GameObject.FindGameObjectWithTag("rocket").GetComponent<RocketBehavior>();
        r.onScoreUpdate += SpawShooter;
        Reborn();
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (!paused)
            timer += Time.deltaTime;
	}

	void TooglePause(bool isPaused)
	{
		paused = isPaused;
	}

    void Reborn()
    {
        StartCoroutine(GeneratePooledAsteroid());
        StartCoroutine(GenerateFollowerHorde());
    }

	IEnumerator GeneratePooledAsteroid()
	{
		while(true)
		{
			if(!paused)
			{
				//só geramos asteroid se existe menos da quota em jogo.
				if(transform.childCount < quota)
				{
					int side = Random.Range(0, 4);
					Vector3 p = new Vector3(xMin,0,yMin);
					switch (side)
					{
						//Up Side
					case 0: p = new Vector3(Random.Range(xMin, xMax), 0, yMax); break;
						//Right Side
					case 1: p = new Vector3(xMax, 0, Random.Range(yMin, yMax)); break;
						//Down Side
					case 2: p = new Vector3(Random.Range(xMin, xMax), 0, yMin); break;
						//Left Side
					case 3: p = new Vector3(xMin, 0, Random.Range(yMin, yMax)); break;
					}

					asteroidGenerated++;
					GameObject asteroid;
					// POOLING
                    if (asteroidGenerated % 10.0f == 0 || Random.Range(0, 100) < 25)
                    {
                        switch (Random.Range(0, 3))
                        {
                            case 0: asteroid = meteor.GetPooledObject(); break;
                            case 1: asteroid = alienSpaceCraft.GetPooledObject(); break;
                            case 2: asteroid = alienFollower.GetPooledObject(); break;
                            default: asteroid = meteor.GetPooledObject(); break;
                        }
                    }
                    else
                    {
                        asteroid = asteroids.GetPooledObject();
                    }
                   
					asteroid.GetComponent<FoeBehavior>().Setup(side, p);
                    asteroid.SetActive(true);
					asteroid.transform.parent = transform;

					yield return new WaitForSeconds(Mathf.Max(0.5f, 2.0f - ((Time.timeSinceLevelLoad/30)*2.0f)/100.0f));
				}
				else
				{
					yield return new WaitForSeconds(1.0f);
				}
			}
			else
			{
				yield return new WaitForEndOfFrame();
			}
		}
	}

    IEnumerator GenerateFollowerHorde()
    {
        int counter = 0;
        while (true)
        {
            while (!paused)
            {
                yield return new WaitForSeconds(30.0f);

                counter = Mathf.Min(10, counter+1);
                for (int i = 0; i < counter; i++)
                {
                    int side = Random.Range(0, 4);
                    Vector3 p = new Vector3(xMin, 0, yMin);
                    switch (side)
                    {
                        //Up Side
                        case 0: p = new Vector3(Random.Range(xMin, xMax), 0, yMax); break;
                        //Right Side
                        case 1: p = new Vector3(xMax, 0, Random.Range(yMin, yMax)); break;
                        //Down Side
                        case 2: p = new Vector3(Random.Range(xMin, xMax), 0, yMin); break;
                        //Left Side
                        case 3: p = new Vector3(xMin, 0, Random.Range(yMin, yMax)); break;
                    }
                    GameObject alien = alienFollower.GetPooledObject();
                    alien.GetComponent<FoeBehavior>().Setup(side, p);
                    alien.SetActive(true);
                    alien.transform.parent = transform;
                }
            }
        }
    }

    public void SpawShooter(int score)
    {
        if (score > nextShooterSpawn)
        {
            nextShooterSpawn += 1000;

            int side = Random.Range(0, 4);
            Vector3 p = new Vector3(xMin, 0, yMin);
            switch (side)
            {
                //Up Side
                case 0: p = new Vector3(Random.Range(xMin, xMax), 0, yMax); break;
                //Right Side
                case 1: p = new Vector3(xMax, 0, Random.Range(yMin, yMax)); break;
                //Down Side
                case 2: p = new Vector3(Random.Range(xMin, xMax), 0, yMin); break;
                //Left Side
                case 3: p = new Vector3(xMin, 0, Random.Range(yMin, yMax)); break;
            }
            GameObject alien = alienShooter.GetPooledObject();
            alien.GetComponent<FoeBehavior>().Setup(side, p);
            alien.SetActive(true);
            alien.transform.parent = transform;
        }
    }

    public void GameOver()
    {
        StopAllCoroutines();
    }
}
