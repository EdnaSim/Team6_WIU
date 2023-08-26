using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class shopItem : MonoBehaviour
{
    public ItemInShop itemInShop;
    public TMP_Text itemName;
    public TMP_Text itemCost;
    public Image itemImage;

   
    private void Start()
    {
        itemName.text = itemInShop.item.itemName.ToString() 
            + " x" + itemInShop.amountOfItem.ToString();
        itemImage.sprite = itemInShop.item.image;
        itemCost.text = itemInShop.costOfItem.ToString();
    }

    public void buyItem()
    {
       
    }
}
