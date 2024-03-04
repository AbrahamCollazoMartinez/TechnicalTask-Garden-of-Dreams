using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Net.WebSockets;
using System.Threading.Tasks;

public class Manager_PopUpWindow : MonoBehaviour
{
	[SerializeField] private StatsDisplay[] statsDisplay;
	[SerializeField] private Button[] buttonsLeft;
	[SerializeField] private Image imageIcon;
	[SerializeField] private TMP_Text name_Icon;
	[SerializeField] private CanvasGroup canvasGroup;
	[SerializeField] private GameObject raycasterBlocker, panelWindow;


	private bool isWindowOpen = false;
	private SlotInventory currentSlotOpen_;
	public SlotInventory currentSlotOpen { get { return currentSlotOpen_; } }

	public static Action<SlotInventory> openWindow = delegate { };



	private Tween anim_Panel, anim_Canvas;

	public async void OpenPopUpWindow(SlotInventory assetData)
	{
		if (isWindowOpen) return;

		isWindowOpen = true;
		currentSlotOpen_ = assetData;

		raycasterBlocker.gameObject.SetActive(true);
		if (assetData != null)
		{
			imageIcon.sprite = await assetData.AssetStored.GetIcon();
			name_Icon.text = assetData.AssetStored.GetNameAsset();
			ReadData(assetData.AssetStored);

		}


		panelWindow.transform.localScale = Vector3.one * 1.5f;

		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;


		if (anim_Panel != null)
			anim_Panel.Kill();

		if (anim_Canvas != null)
			anim_Canvas.Kill();

		anim_Canvas = canvasGroup.DOFade(1, 1f);
		anim_Panel = panelWindow.transform.DOScale(1, 1f).SetEase(Ease.InOutElastic);

	}
	public void ClosePopUpWindow()
	{
		if (!isWindowOpen) return;

		isWindowOpen = false;


		raycasterBlocker.gameObject.SetActive(false);

		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;

		if (anim_Panel != null)
			anim_Panel.Kill();

		if (anim_Canvas != null)
			anim_Canvas.Kill();

		anim_Canvas = canvasGroup.DOFade(0, 0.5f);
		anim_Panel = panelWindow.transform.DOScale(Vector3.one * 1.5f, 1f).SetEase(Ease.InOutElastic);
		
		TrySaveCurrentStateInventory();
	}

	private void ReadData(AssetInventory dataAsset)
	{
		if (dataAsset is Consumable)
		{
			var castObject = dataAsset as Consumable;

			foreach (var button in buttonsLeft)
			{
				button.gameObject.SetActive(false);
			}



			switch (castObject.consumableType_)
			{
				case ConsumableType.Damage:

					foreach (var item in statsDisplay)
					{
						if (item.typeStat == TypeStat.Weigh)
						{
							item.statAccess.gameObject.SetActive(true);
							item.statAccess.SetText($"+ {castObject.GetWeigh()}");

						}
						else if (item.typeStat == TypeStat.Damage)
						{
							item.statAccess.gameObject.SetActive(true);
							item.statAccess.SetText($"- {castObject.damageAmount_}");
						}
						else
						{
							item.statAccess.gameObject.SetActive(false);
						}
					}

					buttonsLeft[1].gameObject.SetActive(true);//button to damage asset
					break;
				case ConsumableType.Heal:

					foreach (var item in statsDisplay)
					{
						if (item.typeStat == TypeStat.Weigh)
						{
							item.statAccess.gameObject.SetActive(true);
							item.statAccess.SetText($"+ {castObject.GetWeigh()}");

						}
						else if (item.typeStat == TypeStat.Heal)
						{
							item.statAccess.gameObject.SetActive(true);
							item.statAccess.SetText($"+ {castObject.restoreHealAmount_}");
						}
						else
						{
							item.statAccess.gameObject.SetActive(false);
						}
					}

					buttonsLeft[2].gameObject.SetActive(true);//button to heal asset
					break;
			}

		}
		else if (dataAsset is Clothing)
		{
			var castObject = dataAsset as Clothing;
			foreach (var item in statsDisplay)
			{
				if (item.typeStat == TypeStat.Weigh)
				{
					item.statAccess.gameObject.SetActive(true);
					item.statAccess.SetText($"+ {castObject.GetWeigh()}");

				}
				else if (item.typeStat == TypeStat.Protection)
				{
					item.statAccess.gameObject.SetActive(true);
					item.statAccess.SetText($"+ {castObject.protectionAmount_}");
				}
				else
				{
					item.statAccess.gameObject.SetActive(false);
				}
			}

			foreach (var button in buttonsLeft)
			{
				button.gameObject.SetActive(false);
			}
			buttonsLeft[0].gameObject.SetActive(true);//button to equip
		}
	}

	private async void TrySaveCurrentStateInventory()
	{
		await Task.Yield();
		await Task.Yield();
		ManagerInventory.saveData?.Invoke();
	}


	private void OnEnable()
	{
		openWindow += OpenPopUpWindow;
	}

	private void OnDisable()
	{
		openWindow -= OpenPopUpWindow;
	}

}

[Serializable]
class StatsDisplay
{
	public TypeStat typeStat;
	public StatItem statAccess;
}

enum TypeStat
{
	Heal,
	Damage,
	Protection,
	Weigh
}
