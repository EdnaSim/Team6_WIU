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
        //GetComponent<CanvasGroup>().blocksRaycasts = false;
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        for (int i = 0; i < inventorySlots.Length; i++) {
            InventoryItem invi = inventorySlots[i].GetComponentInChildren<InventoryItem>();
            if (invi == null) continue;
            invi.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true; //this one is not set in the for loop
        image.raycastTarget = true;
        if (CheckIfStack) {
            for (int i = 0; i < inventorySlots.Length; i++) {
                //dropped on a slot that already has an item
                if (inventorySlots[i].transform == PrevParentAfterDrag) {
                    //check if same as the existing item
                    InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();
                    if (itemInSlot != null && item.itemName == itemInSlot.item.itemName) {
                        for (int ii = 0; ii < count; ii++) {
                            if (itemInSlot.count < InventoryManager.Instance.MaxStackedItems
                                && itemInSlot.item.stackable) {
                                itemInSlot.count++;
                                itemInSlot.updateCount();
                                Destroy(gameObject);
                            }
                        }
                        break;
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
        indicator.SetActive(true);
        selected = true;
        if(item.type == Item.itemType.food)
        {
            consumeButton.SetActive(true);
        }
        else if (item.type == Item.itemType.melee) {
            Player_Controller.Instance.wc.ChangeMeleeWeapon(WeaponController.OwnedMeleeList.Find((MeleeWeaponStats w) => w.WeaponName == item.itemName));
        }
        else if (item.type == Item.itemType.ranged) {
            Player_Controller.Instance.wc.ChangeRangedWeapon(WeaponController.OwnedRangedList.Find((RangedWeaponStats r) => r.WeaponName == item.itemName));
        }
    }
    public void deselectItem()
    {
        indicator.SetActive(false);
        selected = false;
        if (consumeButton != null)
            consumeButton.SetActive(false);
    }
}
