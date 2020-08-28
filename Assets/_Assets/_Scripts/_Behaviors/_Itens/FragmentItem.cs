using UnityEngine;
using System.Collections;
using System;

public class FragmentItem : ItemBehavior 
{
	public event Action<int> onFragmentAdition;

	int amount = 1;
    public Transform itemShape;

    void Start()
    {
		// Pega o rocket para cadastra-lo no evento do fragmento ser pego
		if(spaceship != null)
		{
            onFragmentAdition += spaceship.wallet.AddFragment;
		}
    }
		
	void FixedUpdate () 
	{
        OwnUpdate();

		if(!paused)
		{
        	itemShape.Rotate(Vector3.up, 45 * Time.fixedDeltaTime);
        	itemShape.Rotate(Vector3.right, 45 * Time.fixedDeltaTime);
		}
	}

	protected override void OwnSetup()
	{
		itemShape = transform.FindChild("item");
	
		switch (amount)
		{
		case 1:
			itemShape.GetComponent<Renderer>().materials[0].SetColor("_Color", Color.yellow);
			itemShape.GetComponent<Renderer>().materials[0].SetColor("_Emission", Color.yellow);
			speed = 1.0f;
			break;
		case 3:
			itemShape.GetComponent<Renderer>().materials[0].SetColor("_Color", new Color(0.5f, 0.0f, 0.5f));
			itemShape.GetComponent<Renderer>().materials[0].SetColor("_Emission", new Color(0.5f, 0.0f, 0.5f));
			speed = 2.0f;
			break;
		case 5:
			itemShape.GetComponent<Renderer>().materials[0].SetColor("_Color", new Color(1.0f, 0.0f, 0.0f));
			itemShape.GetComponent<Renderer>().materials[0].SetColor("_Emission", new Color(1.0f, 0.0f, 0.0f));
			speed = 3.0f;
			break;
		case 10:
			itemShape.GetComponent<Renderer>().materials[0].SetColor("_Color", new Color(1.0f, 1.0f, 1.0f));
			itemShape.GetComponent<Renderer>().materials[0].SetColor("_Emission", new Color(1.0f, 1.0f, 1.0f));
			speed = 4.0f;
			break;
		} 
	}

    public void SetEssence(int size)
    {
        amount = size;   
		OwnSetup();
    }

    void OnTriggerEnter(Collider other)
    {
		// melhor passar essa colisão para o rocket behavior!
        if (other.CompareTag("rocket"))
        {
            //GameObject.FindGameObjectWithTag("rocket").SendMessage("AddFragment", amount);
			if(onFragmentAdition != null)
				onFragmentAdition(amount);

			TurnOff();
        }
        else if (other.CompareTag("boundary"))
        {
            //Destroy(gameObject);
			transform.parent = null;
			gameObject.SetActive(false);
		}
    }
}
