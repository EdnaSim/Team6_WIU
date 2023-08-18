using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_PlayerHealthbar : UI_Healthbar
{
    // Start is called before the first frame update
    void Start() {
        if (transform.childCount >= 4)
            numDisplay = transform.GetChild(3).gameObject.GetComponent<TMP_Text>();
        slider = GetComponent<Slider>();
        owner = transform.parent.transform.parent.gameObject;
        hm = owner.GetComponent<HealthManager>();
        slider.maxValue = PlayerData.MaxHp;
        if (PlayerData.CurrHP <= 0)
            PlayerData.CurrHP = PlayerData.MaxHp;
        UpdateSlider();
    }

    public override void UpdateSlider() {
        if (slider != null) {
            slider.maxValue = PlayerData.MaxHp;
            slider.value = PlayerData.CurrHP;
        }

        if (numDisplay != null) {
            numDisplay.text = PlayerData.CurrHP + " / " + PlayerData.MaxHp;
        }
    }
}
