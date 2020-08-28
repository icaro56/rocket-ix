using UnityEngine;
using System.Collections;

public class RocketMainScreen : MonoBehaviour
{
    public JetBehavior[] jets;

    void Start()
    {
        for (int i = 0; i < jets.Length; i++)
        {
            StartCoroutine(jets[i].GetOn());
        }
    }

}
