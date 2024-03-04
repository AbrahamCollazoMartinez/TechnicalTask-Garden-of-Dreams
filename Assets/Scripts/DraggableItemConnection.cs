using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.EventSystems;

public class DraggableItemConnection : MonoBehaviour
{
	[SerializeField] private SlotInventory slotInventory;


	public static Action<AssetInventory, int> onStartDragging = delegate { };
	public static Action<Vector3> onDragging = delegate { };
	public static Action onDraggingEnds = delegate { };


	public void OnStartDragging(AssetInventory dataItem, int amount)
	{
		slotInventory.AssetStored = dataItem;
		slotInventory.AssetStored = dataItem;
		slotInventory.Amount = amount;
		this.transform.localScale = Vector3.zero;
		this.transform.DOScale(1, 0.3f).SetEase(Ease.InOutElastic);
	}

	private void OnDragging(Vector3 position)
	{
		this.transform.position = position;
	}

	private void OnDraggingEnd()
	{
		this.transform.DOScale(0, 0.5f).SetEase(Ease.InOutElastic);
	}


	private void OnEnable()
	{
		onStartDragging += OnStartDragging;
		onDragging += OnDragging;
		onDraggingEnds += OnDraggingEnd;
	}

	private void OnDisable()
	{
		onStartDragging -= OnStartDragging;
		onDragging -= OnDragging;
		onDraggingEnds -= OnDraggingEnd;
	}


}
