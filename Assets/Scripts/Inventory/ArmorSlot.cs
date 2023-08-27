using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArmorSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0 )
        {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            if (inventoryItem.item.type == Item.itemType.equipment)
            {
                inventoryItem.parentAfterDrag = transform;
                ArmourDetails.Instance.EquipArmour(inventoryItem.item);
            }
        }
    }

    public string getArmorName()
    {
        InventoryItem inventoryItem = GetComponentInChildren<InventoryItem>();
        if(inventoryItem != null)
        {
            return inventoryItem.item.itemName;
        }
        return "";
    }
}
