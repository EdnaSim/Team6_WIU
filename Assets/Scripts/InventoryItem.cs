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
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        image.raycastTarget = false;
        if (transform.parent.gameObject.tag == "finalProduct")
        {
            Debug.Log("hei");
            CraftingManager.Instance.destroyMaterials();
        }
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

        GetComponent<CanvasGroup>().blocksRaycasts = true;
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
        Debug.Log(parentAfterDrag);
        
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
        if(item.type == Item.itemType.food || item.type ==Item.itemType.consumables)
        {

            consumeButton.SetActive(true);
        }
        
    }
    public void deselectItem()
    {
        indicator.SetActive(false);
        selected = false;
        if(consumeButton != null)
        {
            consumeButton.SetActive(false);
        }
       

    }

}
