using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaceForDraggable : MonoBehaviour, IDropHandler
{
	[SerializeField] private SlotInventory slotInventory;
	public void OnDrop(PointerEventData eventData)
	{
		GameObject dropped = eventData.pointerDrag;

		HasDraggable draggableItem = dropped.GetComponent<HasDraggable>();

		if (draggableItem != null)
		{
			draggableItem.slotAfterDrag = slotInventory;
		}
		TrySaveCurrentStateInventory();
	}

	private async void TrySaveCurrentStateInventory()
	{
		await Task.Yield();
		await Task.Yield();
		ManagerInventory.saveData?.Invoke();
	}
}
