using UnityEngine;
using System.Collections;

public class ButtonPauseResume : MonoBehaviour 
{
	public void Clicked()
	{
		GameManager.Instance.TooglePause();
	}
}
