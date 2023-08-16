using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item item;

    void pickedUp()
    {
        InventoryManager.Instance.addItem(item);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Player" /*&& Input.GetKeyDown(KeyCode.F)*/)
        {
            Debug.Log("hello");
            pickedUp();
        }
    }
}
