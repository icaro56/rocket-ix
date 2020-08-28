using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LabelPlayerPrefs : MonoBehaviour
{
	public string PlayerPrefName;
	Text label;

	void Start () 
	{
		label = transform.FindChild("Quantity").GetComponent<Text>() as Text;
		label.text = ShipStatusData.load(PlayerPrefName, 0).ToString();
	}
	
	public void UpdateValue () 
	{
		label.text = ShipStatusData.load(PlayerPrefName, 0).ToString();
	}

	public IEnumerator UpdateValueIteratively(int until)
	{
		int cur = int.Parse(label.text);
		while(cur != until)
		{
			if (until < cur)
				cur -= (int)Mathf.Ceil(Mathf.Abs(cur-until)*0.5f);
			else if (until > cur)
				cur += (int)Mathf.Ceil(Mathf.Abs(cur-until)*0.5f);

			label.text = cur.ToString();

			yield return new WaitForSeconds(0.01f);
		}
	}

	public IEnumerator Blink()
	{
		int times = 0;
		while(times < 2)
		{
			label.color = Color.red;
			yield return new WaitForSeconds(0.03f);
			label.color = Color.white;
			yield return new WaitForSeconds(0.03f);
			times++;
		}
	}
}
