using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[CreateAssetMenu(fileName = "AssetInventory", menuName = "ScriptableObjects/Clothing", order = 2)]
public class Clothing : AssetInventory
{
	[SerializeField] private ClothingType clothingType;
	[SerializeField] private float protectionAmount;



	public ClothingType clothingType_ { get { return clothingType; } }
	public float protectionAmount_ { get { return protectionAmount; } }

}

public enum ClothingType
{
	Head,
	Body,
	Legs,
	Foot
}
