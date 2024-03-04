using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;


public abstract class AssetInventory : ScriptableObject, IAssetInventory
{
	[SerializeField] private int maxStack;
	[SerializeField] private float weigh;
	[SerializeField] private string iconID;
	[SerializeField] private string nameAsset;


	public virtual int GetMaxStack()
	{
		return maxStack;
	}

	public virtual float GetWeigh()
	{
		return weigh;
	}

	public async Task<Sprite> GetIcon()
	{

		(bool, Sprite) dataSprite = await Manager_GlobalVariables.Instance.TryGetIcon(iconID);

		if (!dataSprite.Item1)
		{
			await Manager_GlobalVariables.Instance.FetchImages();
			//Give a try to download if the sprite returns null
			dataSprite = await Manager_GlobalVariables.Instance.TryGetIcon(iconID);
		}


		return dataSprite.Item2;
	}

	public string GetNameAsset()
	{
		return nameAsset;
	}
}


#region ClassesForSerialization

[Serializable]
public class DataAssets
{
	public string nameAsset;
	public int amount;
	public int slotSaved;
	public bool isAtEquipmentSaved;
	public ClothingType equipmentSlot;
}

#endregion
