using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IAssetInventory
{
	float GetWeigh();
	int GetMaxStack(); 
	Task<Sprite> GetIcon();
	string GetNameAsset();
}
