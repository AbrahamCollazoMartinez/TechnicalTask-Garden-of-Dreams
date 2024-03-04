using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Threading.Tasks;

public class SlotInventory : MonoBehaviour
{
	[SerializeField] private Image imageIcon;
	[SerializeField] private CanvasGroup canvasGroup_icon, canvasGroup_count;
	[SerializeField] private TMP_Text text_count;
	[SerializeField] private GameObject loadingIcon;


	private AssetInventory assetStored;
	public AssetInventory AssetStored
	{
		get => assetStored; set
		{
			assetStored = value;

			ReadData();
		}
	}

	private bool hasAsset;
	public bool HasAsset { get { return (assetStored != null) ? true : false; } }

	private int amount;
	public int Amount
	{
		get { return amount; }
		set
		{
			amount = value;
			text_count.text = value.ToString();
		}
	}

	public int slotPosition { get { return transform.GetSiblingIndex(); } }




	//Cache data
	private Tween cache_anim_FadeIcon;
	private Tween cache_anim_FadeCount;






	private async void ReadData()
	{
		await Task.Yield();

		if (assetStored == null)
		{
			ShowIcon(false);
			ShowAmount(false);
			return;
		}


		loadingIcon.SetActive(true);

		imageIcon.sprite = await assetStored.GetIcon();

		loadingIcon.SetActive(false);

		if (amount <= 1)
		{
			ShowAmount(false);
		}
		else
		{
			ShowAmount(true);
		}

		ShowIcon(true);

	}


	private void ShowIcon(bool Show)
	{

		if (cache_anim_FadeIcon != null)
			cache_anim_FadeIcon.Kill();


		cache_anim_FadeIcon = canvasGroup_icon.DOFade(AlphaAmount(Show), 0.5f).SetEase(Ease.InOutQuint);
	}

	private void ShowAmount(bool Show)
	{
		if (cache_anim_FadeCount != null)
			cache_anim_FadeCount.Kill();


		cache_anim_FadeCount = canvasGroup_count.DOFade(AlphaAmount(Show), 0.5f).SetEase(Ease.InOutQuint);
	}

	private int AlphaAmount(bool state) => state ? 1 : 0;





	//-------public methods
	public void TryOpenItem()
	{
		if (HasAsset)
		{
			Manager_PopUpWindow.openWindow?.Invoke(this);
		}
	}

}
