using UnityEngine;
using System.Collections;

public class AchivCollector1 : BaseAchievement 
{
	void Start () 
	{
        base.Setup();

        myGooglePlayId = Rocket_IX.GPGSIds.achievement_collector_1;
    }

    public override bool Check()
    {
        //Se achievement ainda nao foi feito.
        if(ShipStatusData.load("achievement_" + title, 0) == 0) 
        {
            //Se pegou mais de 100 amarelos numa jogada
            if (rocket.wallet.totalYellowFrag >= 100)
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
