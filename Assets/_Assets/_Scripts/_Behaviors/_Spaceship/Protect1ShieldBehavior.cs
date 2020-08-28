using UnityEngine;
using System.Collections;

public class Protect1ShieldBehavior : ShieldBehavior 
{
	void Start () 
	{
        scaleSize = 5.5f;
	}

    public override void TurnOn()
    {
        base.TurnOn();

        resistance = 1;
    }
}
