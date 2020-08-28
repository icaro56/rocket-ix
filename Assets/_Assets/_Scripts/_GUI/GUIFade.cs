using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIFade : MonoBehaviour {

	public float waitToStart;
	public float fadeLength;

	Text lbl;
	Image sprite;

	// Use this for initialization
	void Start () {
		lbl = GetComponentInChildren<Text>();
		sprite = GetComponentInChildren<Image>();
		if(lbl != null)		lbl.color = new Color(1, 1, 1, 0);
		if(sprite != null)	sprite.color = new Color(1, 1, 1, 0);

		StartCoroutine(Fade());
	}

	IEnumerator Fade()
	{
		yield return new WaitForSeconds(waitToStart);

		float fadeSprite = -0.2f;
		float fadeLabel = 0;

		fadeLength = 1.0f/fadeLength;

		while(fadeSprite < 1.0f)
		{
			fadeSprite += Time.deltaTime * fadeLength;
			fadeLabel += Time.deltaTime * fadeLength;

			if(sprite != null)	sprite.color = new Color(1, 1, 1, fadeSprite);
			if(lbl != null)		lbl.color = new Color(1, 1, 1, fadeLabel);

			yield return null;
		}
	}
}
