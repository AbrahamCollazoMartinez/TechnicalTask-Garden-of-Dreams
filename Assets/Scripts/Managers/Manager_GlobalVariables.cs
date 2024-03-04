using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Manager_GlobalVariables : Singleton<Manager_GlobalVariables>
{
	//this can be changed to fetch the images from a remote storage
	[SerializeField] private ImageToDownloadData[] iconsData_urls;
	[SerializeField] private AssetInventory[] assets_Inventory;
	[SerializeField] private AssetInventory[] assets_InventoryToSpawnRandomly;


	private (string, Sprite)[] imagesAccess;

	public async Task FetchImages()
	{
		imagesAccess = await ImageDownloader.RequestImages(iconsData_urls);
	}

	public async Task<(bool, Sprite)> TryGetIcon(string nameID)
	{
		if (imagesAccess == null)
		{
			await FetchImages();
		}
		else
		{
			foreach (var data in imagesAccess)
			{
				if (data.Item1 == nameID)
					return (true, data.Item2);
			}

		}

		return (false, null);
	}

	public AssetInventory GetAsset(string nameAsset)
	{
		foreach (var asset in assets_Inventory)
		{
			if (asset.GetNameAsset() == nameAsset)
				return asset;
		}

		return null;
	}

	public AssetInventory GetRandomItem()
	{
		int randomIndex = Random.Range(0, assets_InventoryToSpawnRandomly.Length);
		return assets_InventoryToSpawnRandomly[randomIndex];
	}
}

