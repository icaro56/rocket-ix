using UnityEngine;
using System.Collections;

public class EasterEgg : MonoBehaviour 
{
	FragmentItem frag;

	void Awake () 
	{
		frag = GetComponent<FragmentItem>() as FragmentItem;
		frag.SetEssence(1);
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
				if (hit.transform.tag == "item" && hit.transform.name == name)
                {
                    int frags = ShipStatusData.GetTotalFragments();
                    ShipStatusData.SetTotalFragments(frags + 1);

                    //hit.transform.SendMessage("TurnOff");
					frag.TurnOff();
					//Debug.Log(PlayerPrefs.GetInt("TotalFragments", 0));
                }
            }
        }

        if (Input.touchCount > 0)
        {
            RaycastHit hit;
            Vector3 v = new Vector3(Input.touches[0].position.x, Input.touches[0].position.y, 0);
            Ray ray = Camera.main.ScreenPointToRay(v);
            if (Physics.Raycast(ray, out hit))
            {
				if (hit.transform.tag == "item" && hit.transform.name == name)
                {
                    int frags = ShipStatusData.GetTotalFragments();
                    ShipStatusData.SetTotalFragments(frags + 1);
                    //hit.transform.SendMessage("TurnOff");
					frag.TurnOff();
					//Debug.Log(PlayerPrefs.GetInt("TotalFragments", 0));
                }
            }
        }
	}
}
