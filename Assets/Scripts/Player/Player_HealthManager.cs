using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_HealthManager : HealthManager
{
    public static Player_HealthManager Player_hm;

    // Start is called before the first frame update
    void Start()
    {
        Player_hm = this;
       
        if (PlayerData.MaxHp <= 0)
            PlayerData.MaxHp = Base_MaxHealth;
        if (PlayerData.CurrHP <= 0)
            PlayerData.CurrHP = PlayerData.MaxHp;

        UI_Hpbar?.UpdateSlider();
    }

    public override void TakeDamage(float damage, GameObject attacker) {
        if (isDead || attacker.layer == gameObject.layer)
            return;

        PlayerData.CurrHP -= (damage * DamageMultiplier);
        if (PlayerData.CurrHP <= 0) {
            killer = attacker;
            OnDeath();
            return;
        }
        //update after taking damage
        if (UI_Hpbar != null)
            UI_Hpbar.UpdateSlider();

        //damage effects (sanity drain, pet attack etc)
        Player_Controller.Instance.OnHitByEnemy(attacker);
    }

    public override void Heal(float amt) {
        PlayerData.CurrHP += amt;
        if (PlayerData.CurrHP > PlayerData.MaxHp)
            PlayerData.CurrHP = PlayerData.MaxHp;

        UI_Hpbar.UpdateSlider();
    }

    public override void OnDeath() {
        PlayerData.CurrHP = 0;
        if (UI_Hpbar != null)
            UI_Hpbar.UpdateSlider();
        isDead = true;
        //play death anim
        if (ar != null)
            ar.SetTrigger("Death");
        else
            OnDeathAnimEnd();
    }

    public override void OnDeathAnimEnd() {
        //reset player data
        PlayerData.MaxHp = 0;
        PlayerData.CurrHP = 0;
        PlayerData.MaxSanity = 0;
        PlayerData.CurrSanity = 0;

        //TODO: display gameover screen

    }
}
