using UnityEngine;
using System.Collections;

public class AchivOver9000 : BaseAchievement 
{
	void Start () 
	{
        base.Setup();

        myGooglePlayId = Rocket_IX.GPGSIds.achievement_its_over_9000;
    }

    public override bool Check()
    {
        //Se achievement ainda nao foi feito.
        if(ShipStatusData.load("achievement_" + title, 0) == 0) 
        {
            //Se total de fragments é maior que 9k
            if (rocket.totalScore >= 9000)
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
