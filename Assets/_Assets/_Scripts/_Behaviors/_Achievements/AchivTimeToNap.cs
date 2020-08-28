using UnityEngine;
using System.Collections;

public class AchivTimeToNap : BaseAchievement 
{
	void Start ()
	{
        base.Setup();

        myGooglePlayId = Rocket_IX.GPGSIds.achievement_nap_time;
    }

    public override bool Check()
    {
        //Se achievement ainda nao foi feito.
        if(ShipStatusData.load("achievement_" + title, 0) == 0) 
        {
            //Se ficou no mesmo lugar por 60 segundos
            if (rocket.nav.totalSecsInSameSpot >= 60)
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
