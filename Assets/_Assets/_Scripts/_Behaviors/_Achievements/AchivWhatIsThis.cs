using UnityEngine;
using System.Collections;

public class AchivWhatIsThis : BaseAchievement 
{
	void Start () 
	{
        base.Setup();

        myGooglePlayId = Rocket_IX.GPGSIds.achievement_whats_this;
    }

    public override bool Check()
    {
        //Se achievement ainda nao foi feito.
        if(ShipStatusData.load("achievement_" + title, 0) == 0) 
        {
            //Se pegou foi o amarelo
            if (rocket.wallet.lastFragmentTaken == 1)
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
