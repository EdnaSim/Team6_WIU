using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [Header("Loot sets")]
    public GameObject Set1Container;
    public GameObject Set2Container;
    // Start is called before the first frame update
    void Start()
    {
        SetItemsActive(Set1Container, false);
        SetItemsActive(Set2Container, false);
        
        GenerateRandomSet();
    }
    public GameObject PickSet() {
        Random.InitState(System.DateTime.Now.Millisecond);
        if (Random.Range(0,2) == 0) {
            return Set1Container;
        }
        else {
            return Set2Container;
        }
    }

    public void GenerateRandomSet() {
        GameObject ChosenSet = PickSet();

        bool isDup;
        for (int i=0; i < ChosenSet.transform.childCount; i++) {
            isDup = false;
            Item item = ChosenSet.transform.GetChild(i).GetComponent<ItemPickUp>().item;
            if (item.type == Item.itemType.melee) {
                for (int ii=0; ii < WeaponController.OwnedMeleeList.Count; ii++) {
                    if (item.itemName == WeaponController.OwnedMeleeList[ii].WeaponName) { 
                        //found in owned weapons list
                        isDup = true;
                        break; 
                    }
                }
            }
            else if (item.type == Item.itemType.ranged) {
                for (int ii = 0; ii < WeaponController.OwnedRangedList.Count; ii++) {
                    if (item.itemName == WeaponController.OwnedRangedList[ii].WeaponName) {
                        //found in owned weapons list
                        isDup = true;
                        break;
                    }
                }
            }

            if (!isDup) {
                //instantiate the object
                Instantiate(ChosenSet.transform.GetChild(i), ChosenSet.transform.GetChild(i).position, Quaternion.identity).gameObject.SetActive(true);
            }
        }
    }

    public void SetItemsActive(GameObject set, bool active) {
        for (int i=0; i < set.transform.childCount; i++) {
            set.transform.GetChild(i).gameObject.SetActive(active);
        }
    }
}
