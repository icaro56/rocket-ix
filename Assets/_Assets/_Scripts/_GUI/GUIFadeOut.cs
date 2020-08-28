using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class GUIFadeOut : MonoBehaviour
{
	public float waitToStart;
	public float fadeLength;

	public UnityEvent callback;

	Text lbl;
	Image sprite;

	// Use this for initialization
	void Start () {
		lbl = GetComponentInChildren<Text>();
		sprite = GetComponentInChildren<Image>();

		StartCoroutine(Fade());
	}

	IEnumerator Fade()
	{
		yield return new WaitForSeconds(waitToStart);

		float fadeLabel = 1.0f;

		fadeLength = 1.0f/fadeLength;

		while(fadeLabel >= 0)
		{
			fadeLabel -= Time.deltaTime * fadeLength;
			if(sprite != null) sprite.color = new Color(1, 1, 1, fadeLabel);
			if(lbl != null) lbl.color = new Color(1, 1, 1, fadeLabel);

			yield return null;
		}

		callback.Invoke();
	}
}
