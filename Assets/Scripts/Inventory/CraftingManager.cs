using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance;

    [SerializeField] craftingSlot firstSlot;
    [SerializeField] craftingSlot secondSlot;
    [SerializeField] craftingSlot finalSlot;
    [SerializeField] GameObject craftingMenu;

    public CraftingRecipe[] recipes;
    InventoryItem firstItemInSlot;
    InventoryItem secondItemInSlot;
    private Item finalProduct;

    public GameObject inventoryItemPrefab;

    private int finalAmt;
    private int firstAmt;
    private int secondAmt;
    private bool lastRecipeisOne = false;
    private bool crafted;
    private bool itemBackToInventory;
    InventoryManager inventoryManager;


    private void Start()
    {
        craftingMenu.SetActive(false);
        finalAmt = 0;
        firstAmt = 0;
        secondAmt = 0;
        crafted = false;
        itemBackToInventory = false;
        inventoryManager = InventoryManager.Instance;
        finalProduct = null;
        Instance = this;
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.C) )
        //{
        //    if (craftingMenu.activeSelf == true)
        //    {
        //        craftingMenu.SetActive(false);
        //    }
        //    else
        //    {
        //        craftingMenu.SetActive(true);
        //    }
            
        //}
        firstItemInSlot = firstSlot.GetComponentInChildren<InventoryItem>();
        secondItemInSlot = secondSlot.GetComponentInChildren<InventoryItem>();
        if (craftingMenu.activeSelf == true)
        {
            itemBackToInventory = false;
            if (finalSlot.GetComponentInChildren<InventoryItem>() == null && crafted == false)
            {
                if (firstItemInSlot != null && secondItemInSlot != null)
                {
                    Debug.Log(firstItemInSlot.item.itemName);
                    Debug.Log(secondItemInSlot.item.itemName);
                    if (getProduct(firstItemInSlot.item, secondItemInSlot.item) != null)
                    {
                        lastRecipeisOne = false;
                        finalProduct = getProduct(firstItemInSlot.item, secondItemInSlot.item);
                        spawnProduct(finalProduct);

                    }
                }
                else if (firstItemInSlot != null)
                {
                    if (getProduct(firstItemInSlot.item) != null)
                    {
                        lastRecipeisOne = true;
                        finalProduct = getProduct(firstItemInSlot.item);
                        Debug.Log(finalProduct);
                        spawnProduct(finalProduct);

                    }
                }
                else if (secondItemInSlot != null)
                {
                    if (getProduct(secondItemInSlot.item) != null)
                    {
                        lastRecipeisOne = true;
                        finalProduct = getProduct(secondItemInSlot.item);
                        spawnProduct(finalProduct);

                    }
                }
            }
            if (finalSlot.GetComponentInChildren<InventoryItem>() != null && crafted == true)
            {

                if (firstItemInSlot == null && !lastRecipeisOne)
                {
                    Destroy(finalSlot.transform.GetChild(0).gameObject);
                    crafted = false;
                }
                else if (secondItemInSlot == null && !lastRecipeisOne)
                {
                    Destroy(finalSlot.transform.GetChild(0).gameObject);
                    crafted = false;
                }
                else if (lastRecipeisOne && firstItemInSlot == null && secondItemInSlot == null)
                {
                    Destroy(finalSlot.transform.GetChild(0).gameObject);
                    crafted = false;
                }
            }
        }
        else
        {
            if (itemBackToInventory == false)
            {
                if (firstItemInSlot != null)
                {
                    firstItemInSlot.transform.SetParent(inventoryManager.getEmptySlot().transform);
                }
                if (secondItemInSlot != null)
                {
                    secondItemInSlot.transform.SetParent(inventoryManager.getEmptySlot().transform);
                }
                if (finalSlot.GetComponentInChildren<InventoryItem>() != null && crafted == false)
                {
                    finalSlot.GetComponentInChildren<InventoryItem>().transform.SetParent(inventoryManager.getEmptySlot().transform);
                }
            }
           
        }
    }

    
    private Item getProduct(Item item1)
    {
        for (int i = 0; i < recipes.Length; i++)
        {
            CraftingRecipe recipe = recipes[i];
            if (recipe.firstMaterial == item1 
                && recipe.secondMaterial == null
                && item1.amount >= recipe.amtOfFirst)
            {
                firstAmt = recipe.amtOfFirst;
                finalAmt = recipe.amtOfFinal;//finalAmt of items in inv
                return recipe.finalMaterial;
            }
        }
        return null;
    }

    private Item getProduct(Item item1, Item item2)
    {
        for (int i = 0; i < recipes.Length; i++)
        {
            CraftingRecipe recipe = recipes[i];
            
            if (recipe.firstMaterial == item1 && recipe.secondMaterial == item2
                && item1.amount >= recipe.amtOfFirst && item2.amount >= recipe.amtOfSecond)
            {
               
                firstAmt = recipe.amtOfFirst;
                secondAmt = recipe.amtOfSecond;
                finalAmt = recipe.amtOfFinal;//finalAmt of items in inv
                return recipe.finalMaterial;
            }
        }

        return null;
    }


    private void spawnProduct(Item item)
    {
        GameObject newItemGO = Instantiate(inventoryItemPrefab);
        InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();
        inventoryItem.count = finalAmt;
        inventoryItem.InitializeItem(item);
        newItemGO.transform.SetParent(finalSlot.transform);
        crafted = true;
    }

    public void destroyMaterials()
    {
        if (firstItemInSlot != null && secondItemInSlot != null)
        {
            firstItemInSlot.count -= firstAmt;
            firstItemInSlot.updateCount();
           
            if (firstItemInSlot.count == 0)
            {
                Destroy(firstSlot.transform.GetChild(0).gameObject);
            }

            secondItemInSlot.count -= secondAmt;
            secondItemInSlot.updateCount();

            if (secondItemInSlot.count == 0)
            {
                Destroy(secondSlot.transform.GetChild(0).gameObject);
            }
        }
        else if (firstItemInSlot != null)
        {
            firstItemInSlot.count -= firstAmt;
            firstItemInSlot.updateCount();

            if (firstItemInSlot.count == 0)
            {
                Destroy(firstSlot.transform.GetChild(0).gameObject);
            }
        }
        else if (secondItemInSlot != null)
        {
            secondItemInSlot.count -= firstAmt;
            secondItemInSlot.updateCount();

            if (secondItemInSlot.count == 0)
            {
                Destroy(secondSlot.transform.GetChild(0).gameObject);
            }
        }
        finalAmt = 0;
        secondAmt = 0;
        firstAmt = 0;
        crafted = false;
    }
}
