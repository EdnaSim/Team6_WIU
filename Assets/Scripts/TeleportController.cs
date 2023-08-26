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
            if(Input.GetKeyDown(KeyCode.F))
            {
                collision.gameObject.transform.position = TeleportDestination.transform.position;
            }
        }
    }
}
