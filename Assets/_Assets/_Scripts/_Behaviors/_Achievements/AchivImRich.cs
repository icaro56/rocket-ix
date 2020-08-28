using UnityEngine;
using System.Collections;

public class AchivImRich : BaseAchievement 
{
	void Start () 
	{
        base.Setup();

        myGooglePlayId = Rocket_IX.GPGSIds.achievement_i_am_rich;
    }

    public override bool Check()
    {
        //Se achievement ainda nao foi feito.
        if(ShipStatusData.load("achievement_" + title, 0) == 0) 
        {
            //Se total de fragments é maior que 10k
            if (ShipStatusData.GetTotalFragments() >= 10000)
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
