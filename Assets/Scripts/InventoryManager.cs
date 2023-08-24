using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager Instance;

    [SerializeField] public GameObject Desc_UI_Container;


    public InventorySlot[] inventorySlots;
    public GameObject[] items;

    public GameObject inventoryItemPrefab;

    private int MaxStackedItems = 20;

    public ItemStats[] itemStats;

    public ArmorSlot armorSlot;
    [SerializeField] GameObject player;
    [SerializeField] public GameObject consumeButton;

    private void Awake() 
    {
        Instance = this;
        Desc_UI_Container.SetActive(false);
        consumeButton.SetActive(false);
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


    public bool removeItem(Item _item, int amount)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null
                && itemInSlot.item == _item)
            {

                itemInSlot.count -= amount;
                itemInSlot.updateCount();
                if (itemInSlot.count <= 0)
                {
                    itemInSlot.item.amount = 0;
                    Destroy(slot.transform.GetChild(0).gameObject);
                }
                return true;
            }
        }

        return false;
    }

    public InventorySlot getEmptySlot()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {

                return slot;
            }
        }
        return null;
    }
    public string getSelected()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.selected == true)
            {
                Debug.Log("select" + itemInSlot.item.itemName);
                return itemInSlot.item.itemName;

            }
            
        }

        return "";
            
    }
    void SpawnNewItem(Item _item , InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(_item);
        newItemGo.transform.SetParent(slot.transform);
    }

    public void resetSelected()
    {
        for(int i = 0; i < 5; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.selected == true)
            {
                return;
            }
        }
        if (inventorySlots[0].GetComponentInChildren<InventoryItem>() != null)
        {
            inventorySlots[0].GetComponentInChildren<InventoryItem>().selectItem();
           
        }
        return;
    }

    public Item getItem(string _itemName)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item.itemName == _itemName)
            {
                return itemInSlot.item;
            }

        }
        return null;
    }

    public int getAmmo()
    {
        Item ammo = getItem("Ammo");
        if (ammo.amount > 0)
        {
            return ammo.amount;
        }
        else
        {
            return 0;
        }
        
    }
    public void setAmmo(int addAmt)
    {
        Item ammo = getItem("Ammo");
        removeItem(ammo, addAmt);
       
    }

    public void dropItem()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.selected == true)
            {
                itemInSlot.count--;
                itemInSlot.updateCount();

                for (int j = 0; j < items.Length; j++)
                {
                    Item item = items[j].GetComponent<ItemPickUp>().item;
                    if (item == itemInSlot.item)
                    {
                        GameObject spawnItem = Instantiate(items[j]);
                        spawnItem.transform.position = player.transform.position;
                        
                    }
                }
                if (itemInSlot.count == 0)
                {
                    Debug.Log("delete");
                    Destroy(slot.transform.GetChild(0).gameObject);
                }

            }

        }

    }

    public void consumeItem()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.selected == true)
            {
                itemInSlot.count--;
                itemInSlot.updateCount();
                addStats(itemInSlot.item);
                if (itemInSlot.count == 0)
                {
                    Debug.Log("delete");
                    Destroy(slot.transform.GetChild(0).gameObject);
                }

            }
        }
    }

    void addStats(Item _item)
    {
        for (int i = 0; i < itemStats.Length; i++)
        {
            ItemStats _itemStats = itemStats[i];
            if (_itemStats != null && _itemStats.item == _item)
            {
                if (_itemStats.statstype == ItemStats.statsType.health)
                {
                    // add to player health
                }
                else if (_itemStats.statstype == ItemStats.statsType.stamina)
                {
                    // add to player stamina
                }
                else if (_itemStats.statstype == ItemStats.statsType.sanity)
                {
                    // add to player sanity
                }
            }
        }
    }

    public string getArmour()
    {
        InventoryItem InvtItem = armorSlot.GetComponentInChildren<InventoryItem>();
        
        if (InvtItem != null)
        {
            return InvtItem.item.itemName;
        }
        return "";
    }

    
 
}
