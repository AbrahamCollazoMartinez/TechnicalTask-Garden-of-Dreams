using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "AssetInventory", menuName = "ScriptableObjects/Consumable", order = 1)]
public class Consumable : AssetInventory
{
	[SerializeField] private ConsumableType consumableType;
	[SerializeField] private float restoreHealAmount;
	[SerializeField] private float damageAmount;


	public ConsumableType consumableType_ { get { return consumableType; } }
	public float restoreHealAmount_ { get { return restoreHealAmount; } }
	public float damageAmount_ { get { return damageAmount; } }
	
	
}

public enum ConsumableType
{
	Damage,
	Heal
}
