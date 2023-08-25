using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet_Bird : Pet
{
    [Header("Ability - HP regen")]
    public float RegenInterval;
    public float RegenAmount;
    [Tooltip("At what % of HP does the regeneration stop. ")]
    [Range(0, 1)]public float MaxPercentLimit;

    protected float regenTimer = 0f;

    // Update is called once per frame
    void Update()
    {
        //if above % health, dont heal
        if (PlayerData.CurrHP / PlayerData.MaxHp >= MaxPercentLimit) {
            regenTimer = 0f;
            return;
        }

        if (regenTimer < RegenInterval) {
            regenTimer += Time.deltaTime;
        }
        else {
            regenTimer = 0f;
            if (Player_HealthManager.Instance != null)
                Player_HealthManager.Instance.Heal(RegenAmount);
        }
    }
}
