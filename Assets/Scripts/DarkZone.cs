using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            SanityManager.Instance.inDark = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            SanityManager.Instance.inDark = false;
        }
    }
}
