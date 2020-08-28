using UnityEngine;
using System.Collections;

public class ShieldBehavior : MonoBehaviour 
{
    protected float scaleSize = 1.0f;
	protected int resistance;
    protected Material material;

    void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

	public void collisionEnterTurnOff()
	{
		resistance--;
        if (resistance == 0)
			TurnOff();
	}

    public virtual void TurnOn()
    { 
        //transform.localScale = new Vector3(0, 0, 0);
        //Não vai colidir até ter aparecido por completo
        GetComponent<Collider>().enabled = false;
        StartCoroutine(Show());
    }

    public virtual void TurnOff()
    {
        StartCoroutine(Hide());
    }

    protected IEnumerator Show()
    {
        float grow = GetComponent<Renderer>().material.GetFloat("_Cutoff"); //(Time.deltaTime * 2) * scaleSize / 1;
        GetComponent<Renderer>().material.SetFloat("_Cutoff", grow - (Time.deltaTime * 2)) ;
        //transform.localScale = transform.localScale +  new Vector3(grow, grow, grow);

        yield return new WaitForEndOfFrame();
        if (grow > 0)
        {
            StartCoroutine(Show());
        }
        else
        {
            //Pode colidir assim que apareceu por completo
            GetComponent<Collider>().enabled = true;
        }
    }

    protected IEnumerator Hide()
    {
        float shrink = 0;   
        
        while (shrink < 1)
        {
           material.SetFloat("_Cutoff", shrink + (Time.deltaTime * 2));
           yield return new WaitForEndOfFrame();
           shrink = material.GetFloat("_Cutoff");
        }
        gameObject.SetActive(false);        
    }
}
