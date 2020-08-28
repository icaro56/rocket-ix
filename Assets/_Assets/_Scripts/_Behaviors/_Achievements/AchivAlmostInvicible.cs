using UnityEngine;
using System.Collections;

public class AchivAlmostInvicible : BaseAchievement 
{
	void Start () 
	{
        base.Setup();

        myGooglePlayId = Rocket_IX.GPGSIds.achievement_almost_invincible;
    }

    public override bool Check()
    {
        //Se achievement ainda nao foi feito.
        if(ShipStatusData.load("achievement_" + title, 0) == 0) 
        {
            //Se fez upgrade em todas as partes da nave
			if (ShipStatusData.GetSpeedLevel() == 3 &&
                ShipStatusData.GetHandlingLevel() == 3 &&
                ShipStatusData.GetStructLevel() == 3 &&
                ShipStatusData.GetRateShotLevel() == 3 &&
                ShipStatusData.GetMagneticLevel() == 3)
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
