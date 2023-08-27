using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item item;

    void pickedUp()
    {
        if (InventoryManager.Instance.addItem(item))
            Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Use KeybindManager to get the interaction key
            KeyCode interactionKey = KeyCode.F; // Default interaction key is F

            if (KeybindManager.Instance != null)
            {
                interactionKey = KeybindManager.Instance.GetKeyForAction("Interact");
            }

            // Check if the interaction key is pressed
            if (Input.GetKeyDown(interactionKey))
            {
                pickedUp();
            }
        }
    }
}
