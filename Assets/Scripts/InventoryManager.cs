using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager Instance;

    //public void removeItem(Item _item , int _amount)
    //{
    //    for (int i = 0; i < items.Count; i++)
    //    {
    //        if (items[i].itemName == _item.itemName)
    //        {
    //            items[i].amount -=  _amount;
    //            break;
    //        }
    //    }
    //}

    public InventorySlot[] inventorySlots;

    public GameObject inventoryItemPrefab;

    private int MaxStackedItems = 10;

    private void Awake()
    {
        Instance = this;
    }
    public bool addItem(Item _item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null
                && itemInSlot.item == _item
                && itemInSlot.count < MaxStackedItems
                && itemInSlot.item.stackable == true)
            {
                
                itemInSlot.count++;
                itemInSlot.updateCount();
                return true;
            }
        }

        for (int i = 0; i< inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(_item, slot);
                return true;
            }
        }
        return false;
    }

    void SpawnNewItem(Item _item , InventorySlot slot)
    {
        
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(_item);
        newItemGo.transform.SetParent(slot.transform);
    }

}
