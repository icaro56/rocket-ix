using UnityEngine;
using System.Collections;

public class AccelerationCamera : MonoBehaviour {
	//public Transform menu;
    public float turnWeight = 1.0f;

	// Use this for initialization
	void Start () {
		Input.gyro.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		
        transform.Rotate(Vector3.forward, -Input.gyro.rotationRateUnbiased.y * turnWeight);
        transform.Rotate(Vector3.right, -Input.gyro.rotationRateUnbiased.x * turnWeight);
		
		//menu.Rotate(Vector3.up, Input.gyro.rotationRateUnbiased.y * 0.2f);
		//menu.Rotate(Vector3.right, Input.gyro.rotationRateUnbiased.x * 0.2f);
	}

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 25;
        style.normal.textColor = Color.white;
		//GUI.Label(new Rect(10, 10, 100, 100), Input.gyro.enabled + ", " + Input.gyro.rotationRateUnbiased.ToString(), style);
    }


}
