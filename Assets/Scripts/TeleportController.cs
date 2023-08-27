using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    public GameObject TeleportDestination;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            // Use KeybindManager to get the interaction key
            KeyCode interactionKey = KeyCode.F; // Default interaction key is F

            if (KeybindManager.Instance != null) {
                interactionKey = KeybindManager.Instance.GetKeyForAction("Interact");
            }
            if (Input.GetKeyDown(interactionKey))
            {
                collision.gameObject.transform.position = TeleportDestination.transform.position;
            }
        }
    }
}
