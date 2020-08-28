using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonUpgrade : MonoBehaviour 
{
	#region attributes

	public string AttributeName;
	public int level;

	#endregion

	int upgradeLevel;
	int upgradePrice;
	Button button;
	Text textPrice;

    GameData gameData;

	void Start () 
	{
        //ShipStatusData.save(AttributeName, 0);
        gameData = ServiceLocator.GetGameData();

        button = GetComponent<Button>() as Button;
		textPrice = transform.FindChild("Price").GetComponent<Text>() as Text;

		UpdateStatus();

	}

	public void UpdateStatus()
	{
		upgradeLevel = ShipStatusData.load(AttributeName, 0);
        upgradePrice = gameData.GetPriceByName(AttributeName, level);
		textPrice.text = upgradePrice.ToString();

		ColorBlock block = button.colors;
		if(upgradeLevel >= level)
		{
			button.interactable = false;
			transform.FindChild("PurchaseIcon").gameObject.SetActive(true);
			textPrice.text = " \t Ok!";
			//orange color
			block.disabledColor = new Color(0.74f, 0.415f, 0f);
			button.colors = block;
		}
		else if(level > upgradeLevel+1)
		{
			button.interactable = false;
			block.disabledColor = Color.gray;
			button.colors = block;
		}
		else
		{
			button.interactable = true;
		}
	}

	public void OnClick()
	{
		if(ShopManager.Instance.IsPurchasable(upgradePrice))
		{
            ShipStatusData.save(AttributeName, upgradeLevel + 1);
			ShopManager.Instance.Purcharsed(upgradePrice);
		}
		else
		{
			//TO DO Audio of wrong action
			ShopManager.Instance.NoFunds();
		}
	}
}
