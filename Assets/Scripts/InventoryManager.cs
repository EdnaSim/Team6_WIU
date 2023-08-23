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
    public GameObject consumeButton;

    private void Awake() 
    {
        Instance = this;
        Desc_UI_Container.SetActive(false);
        consumeButton.SetActive(false);
    }
    public bool addItem(Item _item)
    {
        List<MeleeWeaponData> mwl = Player_Controller.Instance.wc.WeaponList.MeleeWeaponlist;
        List<RangedWeaponData> rwl = Player_Controller.Instance.wc.WeaponList.RangedWeaponList;
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
        //if nothing returns true in the above loop
        for (int i = 0; i< inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(_item, slot);
                
                //add to weapon controller's list, for easier searching later
                for (int m=0; m < mwl.Count; m++) {
                    if (_item.itemName == mwl[m].Stats.WeaponName) {
                        Player_Controller.Instance.wc.AddMeleeWeapon(mwl[m].Stats);
                    }
                }
                //add to range weapon list, if possible
                for (int r = 0; r < rwl.Count; r++){
                    if (_item.itemName == rwl[r].Stats.WeaponName) {
                        Player_Controller.Instance.wc.AddRangedWeapon(rwl[r].Stats);
                    }
                }
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
                //if a weapon, remove from weapon controller lists
                if (_item.type == Item.itemType.melee) {
                    for (int m =0; m < WeaponController.OwnedMeleeList.Count; m++) {
                        if (_item.itemName == WeaponController.OwnedMeleeList[m].WeaponName) {
                            WeaponController.OwnedMeleeList.RemoveAt(m);
                        }
                    }
                }
                else if (_item.type == Item.itemType.ranged) {
                    for (int r =0; r < WeaponController.OwnedRangedList.Count; r++) {
                        if (_item.itemName == WeaponController.OwnedRangedList[r].WeaponName) {
                            WeaponController.OwnedRangedList.RemoveAt(r);
                        }
                    }
                }

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
    public Item getSelected()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.selected == true)
            {
                return itemInSlot.item;
            }
            
        }

        return null;
            
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
        if (ammo != null && ammo.amount > 0)
        {
            return ammo.amount;
        }
        else
        {
            return 0;
        }
        
    }
    public void RemoveAmmo(int amt)
    {
        Item ammo = getItem("Ammo");
        removeItem(ammo, amt);
       
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
