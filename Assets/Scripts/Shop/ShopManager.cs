using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{

    public Item meteorite;
    public TMP_Text NPCText;

    private void Start()
    {
        NPCText.text = "Hello! Would you like to buy anything?";
    }
    public void buyItem(shopItem _shopItem)
    {

        Debug.Log(_shopItem.itemInShop.item);
        if (meteorite.amount >= _shopItem.itemInShop.costOfItem)
        {
            for (int i = 0; i < _shopItem.itemInShop.amountOfItem; i++)
            {
                InventoryManager.Instance.addItem(_shopItem.itemInShop.item);
            }

            InventoryManager.Instance.removeItem(meteorite, _shopItem.itemInShop.costOfItem);

            NPCText.text = "Thank you for your purchase!";
        }
        else
        {
            NPCText.text = "Aw, you have not enough for this item.";
        }
    }
}