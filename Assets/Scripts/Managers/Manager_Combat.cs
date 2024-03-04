using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;

public class Manager_Combat : MonoBehaviour
{
	[SerializeField] private LifeElement life_Player;
	[SerializeField] private LifeElement life_Enemy;
	[SerializeField] private GameObject button_Weapon, button_Pistol;
	[SerializeField] private CanvasGroup canvasGroup_Weapon, canvasGroup_Pistol;
	[SerializeField] private ManagerInventory inventoryManager;
	[SerializeField] private Consumable dataBullets_Weapon, dataBullets_Pistol;
	[SerializeField] private DamageAnim anim_Damage_Player, anim_Damage_Enemy;
	[SerializeField] private EquipmentStat stat_Player_Head, stat_Player_Body;
	[SerializeField] private CanvasGroup GameOverScreen;
	[SerializeField] private GameObject elementGameObject;
	[SerializeField] private UnityEvent onGameRestart;

	bool rightWeaponSelected = true;


	Tween animButton_weapon, animButton_pistol;
	Tween animCanvas_weapon, animCanvas_pistol;
	Tween anim_GameOver_Canvas, anim_Size;
	Consumable dataConsumableCast;
	int damageToPlayer = 15;
	bool lookFire = false;


	private void Start()
	{
		UpdateStateSelection();
	}


	public void OnClickWeapon()
	{

		rightWeaponSelected = false;

		UpdateStateSelection();
	}

	public void OnClickPistol()
	{
		rightWeaponSelected = true;

		UpdateStateSelection();
	}

	public async void OnclickFire()
	{
		if (lookFire) return;

		lookFire = true;

		if (rightWeaponSelected)
		{
			(bool, SlotInventory) dataAsset = inventoryManager.InventoryHasAsset(dataBullets_Pistol);
			if (dataAsset.Item1)
			{
				if (dataAsset.Item2.Amount > 0)
				{
					dataAsset.Item2.Amount--;
					dataConsumableCast = dataAsset.Item2.AssetStored as Consumable;
					life_Enemy.Damage(dataConsumableCast.damageAmount_);
					anim_Damage_Enemy.TriggerAnim();
					CheckEnemyLife();

					DamageToPlayer();
				}

			}
		}
		else
		{
			(bool, SlotInventory) dataAsset = inventoryManager.InventoryHasAsset(dataBullets_Weapon);
			if (dataAsset.Item1)
			{
				if (dataAsset.Item2.Amount >= 3)
				{
					dataAsset.Item2.Amount = dataAsset.Item2.Amount - 3;
					dataConsumableCast = dataAsset.Item2.AssetStored as Consumable;
					life_Enemy.Damage(dataConsumableCast.damageAmount_ * 3);
					anim_Damage_Enemy.TriggerAnim();
					CheckEnemyLife();

					DamageToPlayer();
				}

			}
		}

		await Task.Delay(1000);
		lookFire = false;
		await Task.Delay(1000);
		
		ManagerInventory.saveData?.Invoke();

	}

	private void CheckEnemyLife()
	{

		if (life_Enemy.amountLife <= 0)
		{
			inventoryManager.AddRandomItem();
			Restartenemy();
		}
	}

	private void CheckPlayerLife()
	{

		if (life_Player.amountLife <= 0)
		{
			if (anim_GameOver_Canvas != null)
				anim_GameOver_Canvas.Kill();

			if (anim_Size != null)
				anim_Size.Kill();

			anim_GameOver_Canvas = GameOverScreen.DOFade(1f, 0.5f);
			GameOverScreen.interactable = true;
			GameOverScreen.blocksRaycasts = true;

			elementGameObject.transform.localScale = Vector3.one * 1.3f;
			anim_Size = elementGameObject.transform.DOScale(Vector3.one, 0.5f);
		}

	}

	public void RestartGame()
	{
		if (anim_GameOver_Canvas != null)
			anim_GameOver_Canvas.Kill();

		if (anim_Size != null)
			anim_Size.Kill();

		anim_GameOver_Canvas = GameOverScreen.DOFade(0f, 0.5f);
		GameOverScreen.interactable = false;
		GameOverScreen.blocksRaycasts = false;

		elementGameObject.transform.localScale = Vector3.one * 1.5f;
		anim_Size = elementGameObject.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutElastic);

		life_Player.Heal(100);
		life_Enemy.Heal(100);

		onGameRestart?.Invoke();

	}

	private async void Restartenemy()
	{
		await Task.Delay(1000);
		life_Enemy.Heal(100);
	}

	private async void DamageToPlayer()
	{
		await Task.Delay(1000);

		System.Random random = new System.Random();
		bool isHead = random.Next(2) == 0;

		if (isHead)
		{
			if (stat_Player_Head.hasEquipment)
			{
				life_Player.Damage(damageToPlayer - stat_Player_Head.assetInventory.protectionAmount_);
			}
			else
			{
				life_Player.Damage(damageToPlayer);
			}
		}
		else
		{
			if (stat_Player_Body.hasEquipment)
			{
				life_Player.Damage(damageToPlayer - stat_Player_Body.assetInventory.protectionAmount_);
			}
			else
			{
				life_Player.Damage(damageToPlayer);
			}
		}
		anim_Damage_Player.TriggerAnim();

		CheckPlayerLife();
	}

	private void UpdateStateSelection()
	{
		if (animButton_weapon != null)
			animButton_weapon.Kill();

		if (animButton_pistol != null)
			animButton_pistol.Kill();

		if (animCanvas_pistol != null)
			animCanvas_pistol.Kill();

		if (animCanvas_weapon != null)
			animCanvas_weapon.Kill();


		if (rightWeaponSelected)
		{
			animButton_pistol = button_Pistol.transform.DOScale(Vector3.one, 0.5f);
			animButton_weapon = button_Weapon.transform.DOScale(Vector3.one * 0.8f, 0.5f);

			animCanvas_pistol = canvasGroup_Pistol.DOFade(1f, 0.5f);
			animCanvas_weapon = canvasGroup_Weapon.DOFade(0.6f, 0.5f);
		}
		else
		{
			animButton_pistol = button_Pistol.transform.DOScale(Vector3.one * 0.8f, 0.5f);
			animButton_weapon = button_Weapon.transform.DOScale(Vector3.one, 0.5f);

			animCanvas_pistol = canvasGroup_Pistol.DOFade(0.6f, 0.5f);
			animCanvas_weapon = canvasGroup_Weapon.DOFade(1f, 0.5f);
		}
	}

}
