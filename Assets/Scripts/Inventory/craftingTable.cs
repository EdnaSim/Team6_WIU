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
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && Input.GetKey(KeyCode.C))
        {
            if (Input.GetKeyDown(KeyCode.C))
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
