using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class ManagerInventory : MonoBehaviour
{
	[SerializeField] private SlotInventory[] slotsInventory;
	[SerializeField] private EquipmentStat[] slotsEquipment;
	[SerializeField] private LifeElement lifePlayer;
	[SerializeField] private LifeElement lifeEnemy;
	[SerializeField] private UnityEvent OnInventoryDataNull;

	private List<DataAssets> dataInventory = new List<DataAssets>();
	private const string DATA_GAME_NAME = "DataGame";
	private JSONObject dataForSaving;
	private ReaderData readerData;
	public static Action saveData = delegate { };


	private void Start()
	{
		FetchLocalData();
	}

	private void FetchLocalData()
	{
		(bool, string) dataFetched = Manager_SavingManagment.Instance.LoadData(DATA_GAME_NAME);

		if (dataFetched.Item1)
		{
			dataForSaving = new JSONObject(dataFetched.Item2);

			if (dataForSaving.HasField(DATA_GAME_NAME))
			{
				readerData = JsonUtility.FromJson<ReaderData>(dataForSaving[DATA_GAME_NAME].ToString());
				dataInventory = readerData.dataInventory;
			}

			ReadData();
		}
		else
		{
			//this means the data saved is null so we set up the start template
			OnInventoryDataNull?.Invoke();
		}
	}

	public void LoadSimulatedTemplate(List<DataAssets> data)
	{
		readerData = new ReaderData();
		readerData.dataInventory = data;
		readerData.lifeEnemy = 100;
		readerData.lifePlayer = 100;
		dataInventory.Clear();
		dataInventory = data;
		ReadData();
	}

	private void ReadData()
	{
		CleanSlots();

		foreach (var asset in dataInventory)
		{
			if (!asset.isAtEquipmentSaved)
			{

				slotsInventory[asset.slotSaved].AssetStored = Manager_GlobalVariables.Instance.GetAsset(asset.nameAsset);
				slotsInventory[asset.slotSaved].Amount = asset.amount;
			}
			else
			{

				foreach (var item in slotsEquipment)
				{
					Clothing castObject = Manager_GlobalVariables.Instance.GetAsset(asset.nameAsset) as Clothing;
					if (item.getClothType == castObject.clothingType_)
					{
						item.assetInventory = castObject;
					}
				}
			}
		}

		lifePlayer.SetValueLife(readerData.lifePlayer);
		lifeEnemy.SetValueLife(readerData.lifeEnemy);
		
	}


	private void CleanSlots()
	{
		foreach (var slot in slotsInventory)
		{
			slot.AssetStored = null;
		}

		foreach (var slotEquipment in slotsEquipment)
		{
			slotEquipment.assetInventory = null;
		}
	}


	private void PrepareDataForSaving()
	{
		dataForSaving = new JSONObject();
		dataInventory = new List<DataAssets>();

		foreach (var item in slotsInventory)
		{
			if (item.HasAsset)
			{
				DataAssets dataForSaving = new DataAssets();
				dataForSaving.nameAsset = item.AssetStored.name;
				dataForSaving.slotSaved = item.slotPosition;
				dataForSaving.amount = item.Amount;
				dataForSaving.isAtEquipmentSaved = false;

				dataInventory.Add(dataForSaving);
			}
		}

		foreach (var item in slotsEquipment)
		{
			if (item.hasEquipment)
			{

				DataAssets dataForSaving = new DataAssets();
				dataForSaving.nameAsset = item.assetInventory.name;
				dataForSaving.slotSaved = 0;
				dataForSaving.amount = 1;
				dataForSaving.isAtEquipmentSaved = true;

				dataInventory.Add(dataForSaving);
			}
		}


		readerData = new ReaderData();
		readerData.dataInventory = dataInventory;

		readerData.lifePlayer = lifePlayer.amountLife;
		readerData.lifeEnemy = lifeEnemy.amountLife;

		JSONObject JsonObj = new JSONObject(JsonUtility.ToJson(readerData));
		dataForSaving.AddField(DATA_GAME_NAME, JsonObj);
	}

	private void SaveData()
	{
		PrepareDataForSaving();
		Manager_SavingManagment.Instance.SaveData(dataForSaving, DATA_GAME_NAME);
	}


	public (bool, SlotInventory) InventoryHasAsset(AssetInventory assetToSearch)
	{
		foreach (var slot in slotsInventory)
		{
			if (slot.HasAsset)
			{
				if (slot.AssetStored == assetToSearch)
				{
					return (true, slot);
				}
			}
		}

		return (false, null);
	}
	public void AddRandomItem()
	{
		var assetSpawn = Manager_GlobalVariables.Instance.GetRandomItem();
		bool isMedicKit = assetSpawn is Consumable;

		foreach (var slot in slotsInventory)
		{
			if (isMedicKit)
			{
				if (slot.AssetStored is Consumable)
				{
					var castObject = slot.AssetStored as Consumable;
					if (castObject.consumableType_ == ConsumableType.Heal)
					{
						slot.Amount++;
						break;
					}
				}
			}
			else
			{
				if (!slot.HasAsset)
				{
					slot.AssetStored = assetSpawn;
					slot.Amount = 1;
					break;
				}
			}
		}


		SaveData();
	}


	private void OnEnable()
	{
		saveData += SaveData;
	}

	private void OnDisable()
	{
		saveData -= SaveData;
	}
}

[Serializable]
class ReaderData
{
	public List<DataAssets> dataInventory = new List<DataAssets>();
	public int lifePlayer;
	public int lifeEnemy;
}
