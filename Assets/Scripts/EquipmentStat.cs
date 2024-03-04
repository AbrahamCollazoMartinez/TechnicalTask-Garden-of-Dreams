using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentStat : MonoBehaviour
{
	[SerializeField] private GameObject icon;
	[SerializeField] private Image iconEquipment;
	[SerializeField] private ClothingType equiptmentType;
	[SerializeField] private TMP_Text protectionAmountText;

	private Clothing assetData;
	public Clothing assetInventory
	{
		get { return assetData; }
		set
		{
			assetData = value;
			UpdateUI();
		}
	}

	public bool hasEquipment { get { return assetData != null; } }
	public ClothingType getClothType { get { return equiptmentType; } }



	//Cache---------

	private async void UpdateUI()
	{
		if (hasEquipment)
		{
			iconEquipment.sprite = await assetData.GetIcon();

			icon.SetActive(false);
			iconEquipment.gameObject.SetActive(true);

			protectionAmountText.text = assetData.protectionAmount_.ToString();

		}
		else
		{
			icon.SetActive(true);
			iconEquipment.gameObject.SetActive(false);
			protectionAmountText.text = 0.ToString();
		}
	}
}
