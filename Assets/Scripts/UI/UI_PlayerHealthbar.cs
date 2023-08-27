using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_PlayerHealthbar : UI_Healthbar
{
    [Header("UI")]
    [SerializeField] Slider HealthBar;
    Image BarImage;
    Color originalCol;
    float LowHPFlashTimer;

    // Start is called before the first frame update
    void Start() {
        if (transform.childCount >= 4)
            numDisplay = transform.GetChild(3).gameObject.GetComponent<TMP_Text>();
        slider = GetComponent<Slider>();
        //owner = transform.parent.transform.parent.gameObject;
        //hm = owner.GetComponent<HealthManager>();
        slider.maxValue = PlayerData.MaxHp;
        if (PlayerData.CurrHP <= 0)
            PlayerData.CurrHP = PlayerData.MaxHp;
        UpdateSlider();
        BarImage = HealthBar.fillRect.GetComponent<Image>();
        originalCol = BarImage.color;
    }

    private void Update()
    {
        if ((PlayerData.CurrHP / PlayerData.MaxHp) * 100 <= 25)
        {
            LowHPFlashTimer += Time.deltaTime;
            if (LowHPFlashTimer >= (PlayerData.CurrHP / PlayerData.MaxHp))
            {
                BarImage.color = new Color(0.45f, 0.55f, 0.568f, 1);
            }
            if (LowHPFlashTimer > (PlayerData.CurrHP / PlayerData.MaxHp) * 2)
            {
                BarImage.color = originalCol;
                LowHPFlashTimer = 0f;
            }
        }
        else
        {
            BarImage.color = originalCol;
        }
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
