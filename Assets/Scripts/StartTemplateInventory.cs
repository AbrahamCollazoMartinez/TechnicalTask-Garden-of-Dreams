using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;


//This script is with the only purpose of this technical task, in a real project this doesn't exist, this managment MUST be handle in another way. because this is hard coding


//the purpose of the script is to set up the positions and amount of items at the inventory at a start template


public class StartTemplateInventory : MonoBehaviour
{
	[SerializeField] private inventoryAssetTemplateData[] assetsData;
	[SerializeField] private UnityEvent<List<DataAssets>> onInventoryStart;

	public void SimulateData()
	{
		List<DataAssets> dataCollection = new List<DataAssets>();

		foreach (var asset in assetsData)
		{
			DataAssets assetData = new DataAssets();
			assetData.nameAsset = asset.asset.GetNameAsset();
			assetData.amount = asset.amountItem;
			assetData.slotSaved = asset.posSlot - 1;

			dataCollection.Add(assetData);
		}

		onInventoryStart?.Invoke(dataCollection);
	}

}

[Serializable]
class inventoryAssetTemplateData
{
	public AssetInventory asset;
	public int amountItem;
	public int posSlot;
}
