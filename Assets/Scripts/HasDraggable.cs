using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class HasDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[SerializeField] private SlotInventory slotInventory;

	private SlotInventory slotAfterDrag_;
	public SlotInventory slotAfterDrag { set { slotAfterDrag_ = value; } }
	private AssetInventory data_Asset;
	private int amount_Asset;
	private bool canDrag = false;

	public void OnBeginDrag(PointerEventData eventData)
	{
		slotAfterDrag = slotInventory;
		data_Asset = slotInventory.AssetStored;
		amount_Asset = slotInventory.Amount;

		canDrag = slotInventory.HasAsset;

		if (slotInventory.HasAsset)
		{
			DraggableItemConnection.onStartDragging?.Invoke(slotInventory.AssetStored, slotInventory.Amount);
			slotInventory.AssetStored = null;
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (!canDrag) return;
		
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			if (Input.touchCount > 0)
			{
				DraggableItemConnection.onDragging?.Invoke(Input.GetTouch(0).position);
			}
		}
		else
		{
			DraggableItemConnection.onDragging?.Invoke(Input.mousePosition);

		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		slotAfterDrag_.AssetStored = data_Asset;
		slotAfterDrag_.Amount = amount_Asset;
		DraggableItemConnection.onDraggingEnds?.Invoke();
	}
}



