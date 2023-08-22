using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Healthbar : MonoBehaviour
{
    [HideInInspector] public Slider slider;
    protected GameObject owner;
    protected HealthManager hm;
    protected TMP_Text numDisplay;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.childCount >= 4)
            numDisplay = transform.GetChild(3).gameObject.GetComponent<TMP_Text>();
        slider = GetComponent<Slider>();
        owner = transform.parent.transform.parent.gameObject;
        hm = owner.GetComponent<HealthManager>();
        slider.maxValue = hm.MaxHealth;
        if (hm.CurrentHealth <= 0)
            hm.CurrentHealth = hm.MaxHealth;
        UpdateSlider();
    }

    public virtual void UpdateSlider(){
        if (slider != null) {
            slider.maxValue = hm.MaxHealth;
            slider.value = hm.CurrentHealth;
        }

        if (numDisplay != null) {
            numDisplay.text = hm.CurrentHealth + " / " + hm.MaxHealth;
        }
    }

    public virtual void SetBarDisplay(bool b) {
        gameObject.SetActive(b);
    }
}
