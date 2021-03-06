﻿using UnityEngine;
using System.Collections;

public class AchivAnotherColor : BaseAchievement 
{
	void Start () 
	{
        base.Setup();
        myGooglePlayId = Rocket_IX.GPGSIds.achievement_another_color;
    }

    public override bool Check()
    {
        //Se achievement ainda nao foi feito.
        if(ShipStatusData.load("achievement_" + title, 0) == 0) 
        {
            //Se pegou foi o lilas
            if (rocket.wallet.lastFragmentTaken == 3)
            {
                //Feito
                ShipStatusData.save("achievement_" + title, 1);
                return true;
            }
            return false;
        }
        return false;
    }


}
