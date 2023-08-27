using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepOnBed : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision) {
        // Use KeybindManager to get the interaction key
        KeyCode interactionKey = KeyCode.F; // Default interaction key is F

        if (KeybindManager.Instance != null) {
            interactionKey = KeybindManager.Instance.GetKeyForAction("Interact");
        }
        if (Input.GetKeyDown(interactionKey)) {
            Debug.Log("Sleeping");
            //TODO: skip to next day
        }
    }
}
