using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitLevel : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject ExitPrompt;
    [SerializeField] Button ExitBtn;
    [SerializeField] Button CloseBtn;
    [SerializeField] string NextSceneName;

    // Start is called before the first frame update
    void Start()
    {
        ExitPrompt.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            MenuManager.Instance.ShowMenu(ExitPrompt);
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            MenuManager.Instance.HideMenu();
        }
    }

    public void OnExitBtnPress() {
        InventoryManager.Instance.SaveInvSlotData();
        WeaponController.Instance.SaveEquipment();
        //load to named scene

    }

    public void OnCloseBtnPress() {
        MenuManager.Instance.HideMenu();
    }
}
