using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class craftingTable : MonoBehaviour
{
    [SerializeField] GameObject craftingMenu;
    private void Start()
    {
        craftingMenu.SetActive(false);
    }
    private void OnTriggerStay2D(Collider2D collision) {// Use KeybindManager to get the interaction key
        KeyCode interactionKey = KeyCode.F; // Default interaction key is F

        if (KeybindManager.Instance != null) {
            interactionKey = KeybindManager.Instance.GetKeyForAction("Interact");
        }
        if (collision.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(interactionKey))
            {
                if (craftingMenu.activeSelf == true)
                {
                    craftingMenu.SetActive(false);
                }
                else
                {
                    craftingMenu.SetActive(true);
                }
            }
        }
    }
}
