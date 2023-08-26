using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkZone : MonoBehaviour
{
    [Tooltip("If true, the trigger area will set inDark to false. Set this to true if the trigger area is a bright area")]
    [SerializeField] bool Reverse = false;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            SanityManager.Instance.inDark = Reverse ? false : true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            SanityManager.Instance.inDark = Reverse? true : false;
        }
    }
}
