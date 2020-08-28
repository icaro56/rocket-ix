using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverHUD : MonoBehaviour 
{

	#region Members
	public Text ScoreHolder;
	public Text FragmentsHolder;
	#endregion

	void Awake () 
	{
		ScoreHolder = transform.FindChild("ScoreHolder").GetComponent<Text>();
		FragmentsHolder = transform.FindChild("FragmentsHolder").GetComponent<Text>();
	}

	public void SetValues(int Score, int Fragments)
	{
		ScoreHolder.text = Score.ToString();
		FragmentsHolder.text = Fragments.ToString();
	}
}
