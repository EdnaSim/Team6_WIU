using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    public TMP_Text countText;
    [HideInInspector] public Item item;
    [HideInInspector] public int count = 1;
    GameObject description_UI;
    [SerializeField]GameObject indicator;
    public bool selected = false;
    private InventorySlot[] inventorySlots;
    private craftingSlot[] CraftingSlots;
    GameObject consumeButton;

    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public bool CheckIfStack = false;
    [HideInInspector] public Transform PrevParentAfterDrag;

    private void Start()
    {
        //description_UI = GameObject.Find("Description");
        description_UI = InventoryManager.Instance.Desc_UI_Container;
        indicator.SetActive(false);
        inventorySlots = InventoryManager.Instance.inventorySlots;
        consumeButton = InventoryManager.Instance.consumeButton;
        CraftingSlots = new craftingSlot[3];
        CraftingSlots[0] = CraftingManager.Instance.firstSlot;
        CraftingSlots[1] = CraftingManager.Instance.secondSlot;
        CraftingSlots[2] = CraftingManager.Instance.finalSlot;
    }
    public void InitializeItem(Item newItem)
    {
        item = newItem;     
        image.sprite = newItem.image;
        updateCount();
    }

    public void updateCount()
    {
        item.amount = count;
        countText.text = count.ToString();
        //bool textActive = count > 1;
        //countText.gameObject.SetActive(textActive);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        for (int i=0; i < inventorySlots.Length; i++) {
            InventoryItem invi = inventorySlots[i].GetComponentInChildren<InventoryItem>();
            if (invi == null) continue;
            invi.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        for (int i = 0; i < CraftingSlots.Length; i++) {
            InventoryItem invi = CraftingSlots[i].GetComponentInChildren<InventoryItem>();
            if (invi == null) continue;
            invi.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        //GetComponent<CanvasGroup>().blocksRaycasts = false;
        image.raycastTarget = false;
        if (transform.parent.gameObject.tag == "finalProduct") {
            CraftingManager.Instance.destroyMaterials();
        }
        GetComponent<Canvas>().sortingOrder = 12;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);

        selectItem();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<Canvas>().sortingOrder = 11;
        for (int i = 0; i < inventorySlots.Length; i++) {
            InventoryItem invi = inventorySlots[i].GetComponentInChildren<InventoryItem>();
            if (invi == null) continue;
            invi.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        for (int i=0; i < CraftingSlots.Length; i++) {
            InventoryItem invi = CraftingSlots[i].GetComponentInChildren<InventoryItem>();
            if (invi == null) continue;
            invi.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true; //this one is not set in the for loop
        image.raycastTarget = true;
        //dropped item on a slot with another item alr in it
        if (CheckIfStack) {
            for (int i = 0; i < inventorySlots.Length; i++) {
                //found the slot that player tried to drag into
                if (inventorySlots[i].transform == PrevParentAfterDrag) {
                    //check if same as the existing item
                    InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();
                    if (itemInSlot != null && item.itemName == itemInSlot.item.itemName && itemInSlot.item.stackable) {
                        //stackable, start stacking
                        for (int ii = 0; ii < count; ii++) {
                            if (itemInSlot.count < InventoryManager.Instance.MaxStackedItems) {
                                itemInSlot.count++;
                                itemInSlot.updateCount();
                                Destroy(gameObject);
                            }
                        }
                        break;
                    }
                    else if (itemInSlot != null) {
                        //not stackable, try swapping slots
                        if (parentAfterDrag != PrevParentAfterDrag && parentAfterDrag.childCount == 0) {
                            //swap parents
                            Transform newParent = itemInSlot.transform.parent; //need this cuz itemInSlot's parent gets changed
                            itemInSlot.transform.SetParent(parentAfterDrag);
                            parentAfterDrag = newParent;
                            break;
                        }
                    }
                }
            }
            for (int i=0; i < CraftingSlots.Length; i++) {
                //found the slot that player tried to drag into
                if (CraftingSlots[i].transform == PrevParentAfterDrag) {
                    //check if same as the existing item
                    InventoryItem itemInSlot = CraftingSlots[i].GetComponentInChildren<InventoryItem>();
                    if (itemInSlot != null && item.itemName == itemInSlot.item.itemName && itemInSlot.item.stackable) {
                        //stackable, start stacking
                        for (int ii = 0; ii < count; ii++) {
                            if (itemInSlot.count < InventoryManager.Instance.MaxStackedItems) {
                                itemInSlot.count++;
                                itemInSlot.updateCount();
                                Destroy(gameObject);
                            }
                        }
                        break;
                    }
                    else if (itemInSlot != null) {
                        //not stackable, try swapping slots
                        if (parentAfterDrag != PrevParentAfterDrag && parentAfterDrag.childCount == 0) {
                            //swap parents
                            Transform newParent = itemInSlot.transform.parent; //need this cuz itemInSlot's parent gets changed
                            itemInSlot.transform.SetParent(parentAfterDrag);
                            parentAfterDrag = newParent;
                            break;
                        }
                    }
                }
            }
            CheckIfStack = false;
        }

        transform.SetParent(parentAfterDrag);

        //if the armour is equipped, and now dragging off the armour slot
        if (item == ArmourDetails.Instance.EquippedArmour && parentAfterDrag != InventoryManager.Instance.armorSlot.transform) {
            ArmourDetails.Instance.UnequipArmour();
        }
    }

    public void showDescription()
    {
        description_UI.SetActive(true);
        TMP_Text descriptionName = description_UI.transform.Find("name").GetComponent<TMP_Text>();
        descriptionName.text = item.itemName.ToString();
        TMP_Text description = description_UI.transform.Find("description").GetComponent<TMP_Text>();
        description.text = item.description.ToString();
    }

    public void hideDescription()
    {
        description_UI.SetActive(false);
    }

    public void selectItem()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if(itemInSlot != null)
            {
                itemInSlot.deselectItem();
            }
            
        }
        for (int i= 0; i < CraftingSlots.Length; i++) {
            craftingSlot slot = CraftingSlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null) {
                itemInSlot.deselectItem();
            }
        }
        indicator.SetActive(true);
        selected = true;
        if(item.type == Item.itemType.food)
        {
            consumeButton.SetActive(true);
        }
        else if (item.type == Item.itemType.melee) {
            WeaponController.Instance.ChangeMeleeWeapon(WeaponController.OwnedMeleeList.Find((MeleeWeaponStats w) => w.WeaponName == item.itemName));
        }
        else if (item.type == Item.itemType.ranged) {
            WeaponController.Instance.ChangeRangedWeapon(WeaponController.OwnedRangedList.Find((RangedWeaponStats r) => r.WeaponName == item.itemName));
        }

        WeaponController.Instance.UpdateAmmoDisplay();
    }
    public void deselectItem()
    {
        indicator.SetActive(false);
        selected = false;
        if (consumeButton != null)
            consumeButton.SetActive(false);
    }
}
