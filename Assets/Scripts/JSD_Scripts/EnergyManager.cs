using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager Instance;
    public Slider StaminaBar;

    public float staminaAmt = 100f;

    public float MaxStaminaAmt = 100f;

    public bool CanDrain;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        staminaAmt = MaxStaminaAmt;
        StaminaBar.value = staminaAmt / MaxStaminaAmt;
        CanDrain = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(CanDrain)
        {
            LoseEnergy(1f * Time.deltaTime);
        }

        if(staminaAmt <= 0)
        {
            Player_HealthManager.Player_hm.TakeDamage(1f, gameObject);
        }
    }

    public void LoseEnergy(float energyloss)
    {
        staminaAmt -= energyloss;
        StaminaBar.value = staminaAmt / MaxStaminaAmt;
    }

    public void EnergyRecover(float energygain)
    {
        staminaAmt += energygain;
        StaminaBar.value = staminaAmt / MaxStaminaAmt;
    }
}
