using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet_Bird : Pet
{
    [Header("Ability - HP regen")]
    public float RegenInterval;
    public float RegenAmount;

    protected float regenTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (regenTimer < RegenInterval) {
            regenTimer += Time.deltaTime;
        }
        else {
            regenTimer = 0f;
            if (Player_HealthManager.Player_hm != null)
                Player_HealthManager.Player_hm.Heal(RegenAmount);
        }
    }
}
