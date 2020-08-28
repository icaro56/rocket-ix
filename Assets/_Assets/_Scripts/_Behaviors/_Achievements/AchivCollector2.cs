using UnityEngine;
using System.Collections;

public class AchivCollector2 : BaseAchievement 
{
	void Start () 
	{
        base.Setup();

        myGooglePlayId = Rocket_IX.GPGSIds.achievement_collector_2;
    }

    public override bool Check()
    {
        //Se achievement ainda nao foi feito.
        if(ShipStatusData.load("achievement_" + title, 0) == 0) 
        {
            //Se pegou mais de 20 magentas em uma jogada
            if (rocket.wallet.totalMagentaFrag >= 20)
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
