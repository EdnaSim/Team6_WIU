using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
    public Image StaminaBar;

    public float staminaAmt = 100f;

    public float MaxStaminaAmt = 100f;

    // Start is called before the first frame update
    void Start()
    {
        staminaAmt = MaxStaminaAmt;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoseEnergy(float energyloss)
    {
        staminaAmt -= energyloss;
        StaminaBar.fillAmount = staminaAmt / MaxStaminaAmt;
    }

    public void EnergyRecover(float energygain)
    {
        staminaAmt += energygain;
        StaminaBar.fillAmount = staminaAmt / MaxStaminaAmt;
    }
}
