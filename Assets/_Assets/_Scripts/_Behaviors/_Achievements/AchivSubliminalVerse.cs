using UnityEngine;
using System.Collections;

public class AchivSubliminalVerse : BaseAchievement
{
	void Start () 
	{
        base.Setup();

        myGooglePlayId = Rocket_IX.GPGSIds.achievement_subliminal_score;
    }

    public override bool Check()
    {
        //Se achievement ainda nao foi feito.
        if(ShipStatusData.load("achievement_" + title, 0) == 0) 
        {
            //Se total de fragments pego na fase é 666
            if (rocket.wallet.fragments == 666)
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
