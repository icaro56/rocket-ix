using UnityEngine;
using System.Collections;

public class AchivBreakToCoffe : BaseAchievement 
{
	void Start () 
	{
        base.Setup();

        myGooglePlayId = Rocket_IX.GPGSIds.achievement_break_to_coffee;
    }

    public override bool Check()
    {
        //Se achievement ainda nao foi feito.
        if(ShipStatusData.load("achievement_" + title, 0) == 0) 
        {
            //Se ficou mais de 30 segundos no mesmo lugar
            if (rocket.nav.totalSecsInSameSpot >= 30)
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
