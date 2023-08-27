using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class craftingSlot : MonoBehaviour, IDropHandler
{ 
    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (transform.childCount == 0) {
            inventoryItem.CheckIfStack = false;
            inventoryItem.parentAfterDrag = transform;
        }
        else {
            inventoryItem.CheckIfStack = true;
            inventoryItem.PrevParentAfterDrag = transform;
        }
    }
}
