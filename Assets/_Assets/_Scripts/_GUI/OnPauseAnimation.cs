using UnityEngine;
using System.Collections;

public class OnPauseAnimation : MonoBehaviour 
{
	public void SetTimeScale(int scale)
	{
		Time.timeScale = scale;
	}
}
