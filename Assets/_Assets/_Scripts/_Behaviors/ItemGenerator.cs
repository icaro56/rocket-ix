using UnityEngine;
using System.Collections;

public class ItemGenerator : MonoBehaviour 
{
    //public GameObject fragment;
    //public GameObject[] itens;

	public NewObjectPoolerScript fragmentPooler;
	public NewObjectPoolerScript[] itemPooler;

	bool paused = false;
    float timer = 0.0f;

    #region CONSTANTES DO MUNDO
	float yMin = -25;
	float yMax = 25;
	float xMin = -39.5f;
	float xMax = 39.5f;
    #endregion

	// Use this for initialization
	void Start () 
    {
		GameManager.Instance.onGameOver += GameOver;
		GameManager.Instance.onPause += TooglePause;
        GameManager.Instance.onReborn += Reborn;
        Reborn();
    }
	
	// Update is called once per frame
	void Update () 
    {
        timer += Time.deltaTime;
	}

	void TooglePause(bool isPaused)
	{
		paused = isPaused;
	}

    void Reborn()
    {
        StartCoroutine(GenerateFragment(1));
        StartCoroutine(GenerateItem());
    }

    IEnumerator GenerateFragment(int fragmentsGenerated)
    { 
		while(true)
		{
			if(!paused)
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

		        //Gera um asteroid aleatórios da lista
				//GameObject item = GameObject.Instantiate(fragment, p, Quaternion.identity) as GameObject;
				GameObject item = fragmentPooler.GetPooledObject();
				item.transform.position = p;
		        item.transform.parent = transform;
				item.SetActive(true);

				FragmentItem fragItem = item.GetComponent<FragmentItem>() as FragmentItem;
		        int[] values = new int[]{ side };
		        //item.SendMessage("Setup", values);
				fragItem.Setup(values);

		        //50, 100, 150, 200
		        if (fragmentsGenerated % 50.0f == 0)
		        {
		           //item.SendMessage("SetEssence", 10);
					fragItem.SetEssence(10);
				}
		        //22, 44, 66, 88, 110, 132, 154, 176, 198
		        else if (fragmentsGenerated % 22.0f == 0)
		        {
		            //item.SendMessage("SetEssence", 5);
					fragItem.SetEssence(5);
				}
		        //aleatorio aos 12, 24, 36, 48
		        else if (fragmentsGenerated % 12.0f == 0)
		        {
		            int[] sort = { 1, 3, 5, 10 };
		            //item.SendMessage("SetEssence", sort[Random.Range(0,4)]);
					fragItem.SetEssence(sort[Random.Range(0,4)]);
				}
		        //10, 20, 30, 40, 60, 70, 80, 90
		        else if (fragmentsGenerated % 10.0f == 0)
		        {
		            //item.SendMessage("SetEssence", 3);
					fragItem.SetEssence(3);
		        }
		        else
		        {
		            //item.SendMessage("SetEssence", 1);
					fragItem.SetEssence(1);
		        }

				++fragmentsGenerated;
		        yield return new WaitForSeconds(Random.Range(1.0f, 10.0f));
			}
			else
			{
                yield return new WaitForSeconds(Random.Range(1.0f, 10.0f));
			}
		}
    }

    IEnumerator GenerateItem()
    {
		while(true)
		{
			if(!paused)
			{
		        yield return new WaitForSeconds(Random.Range(15.0f, 40.0f));

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

				//GameObject item = GameObject.Instantiate(itens[Random.Range(0, itens.Length)], p, Quaternion.identity) as GameObject;
				GameObject item = itemPooler[Random.Range(0, itemPooler.Length)].GetPooledObject();
				item.transform.position = p;
		        item.transform.parent = transform;
				item.SetActive(true);

				ItemBehavior itemBh = item.GetComponent<ItemBehavior>() as ItemBehavior;
		        int[] values = new int[] { side };
		        //item.SendMessage("Setup", values);
				itemBh.Setup(values);
			}
			else
			{
                yield return new WaitForSeconds(Random.Range(15.0f, 40.0f));
			}
		}
    }

    public void GameOver()
    {
        StopAllCoroutines();
    }
}
