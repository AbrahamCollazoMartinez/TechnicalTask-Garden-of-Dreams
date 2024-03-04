using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AssetManagment : MonoBehaviour
{
	[SerializeField] private Manager_PopUpWindow manager_PopUpWindow;
	[SerializeField] private LifeElement lifeAccess;
	[SerializeField] private EquipmentStat[] equipmentsAccess;


	//cache-------------
	Consumable castObject_Consum;
	Clothing castObject_Cloth;


	public void onClick_Buy()
	{
		if (manager_PopUpWindow.currentSlotOpen.Amount < manager_PopUpWindow.currentSlotOpen.AssetStored.GetMaxStack())
		{
			manager_PopUpWindow.currentSlotOpen.Amount++;
		}
	}

	public void onClick_Heal()
	{
		if (lifeAccess.amountLife == 100) return;

		castObject_Consum = manager_PopUpWindow.currentSlotOpen.AssetStored as Consumable;

		lifeAccess.Heal(castObject_Consum.restoreHealAmount_);

		if (manager_PopUpWindow.currentSlotOpen.Amount == 1)
		{
			manager_PopUpWindow.currentSlotOpen.AssetStored = null;
		}
		else
		{
			manager_PopUpWindow.currentSlotOpen.Amount--;
		}

	}

	public void onClickEquip()
	{
		castObject_Cloth = manager_PopUpWindow.currentSlotOpen.AssetStored as Clothing;
		foreach (var stat in equipmentsAccess)
		{
			if (castObject_Cloth.clothingType_ == stat.getClothType)
			{
				if (stat.hasEquipment)
				{
					//change equipment
					Clothing dataFromStats = stat.assetInventory;
					manager_PopUpWindow.currentSlotOpen.AssetStored = dataFromStats;
					stat.assetInventory = castObject_Cloth;
				}
				else
				{
					stat.assetInventory = castObject_Cloth;
					manager_PopUpWindow.currentSlotOpen.AssetStored = null;
				}
			}
		}
	}

	public void onClickDelete()
	{
		if (manager_PopUpWindow.currentSlotOpen.Amount == 1)
		{

			manager_PopUpWindow.currentSlotOpen.AssetStored = null;
		}
		else
		{
			manager_PopUpWindow.currentSlotOpen.Amount--;

		}

	}
}
