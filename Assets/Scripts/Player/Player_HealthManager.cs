using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_HealthManager : HealthManager
{
    public static Player_HealthManager Instance;

    private void Awake() {
        Instance = this;
        AccumulatedDM = DamageMultiplier;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerData.MaxHp <= 0)
            PlayerData.MaxHp = Base_MaxHealth;
        if (PlayerData.CurrHP <= 0)
            PlayerData.CurrHP = PlayerData.MaxHp;

        UI_Hpbar.UpdateSlider();
    }

    protected override void Update() {
        if (OnlyShowBarWhenDamaged) {
            if (HPbarShowTimer > 0f) {
                UI_Hpbar.SetBarDisplay(true);
                HPbarShowTimer -= Time.deltaTime;
            }
            else {
                UI_Hpbar.SetBarDisplay(false);
            }
        }

        //make the bar go down smoothly
        float smoothValue = Mathf.SmoothDamp(UI_Hpbar.slider.value, PlayerData.CurrHP, ref currVel, 0.4f);
        UI_Hpbar.slider.value = smoothValue;
    }

    public override void TakeDamage(float damage, GameObject attacker) {
        if (Death || attacker.layer == gameObject.layer)
            return;

        PlayerData.CurrHP -= (damage * DamageMultiplier);
        if (PlayerData.CurrHP <= 0) {
            killer = attacker;
            OnDeath();
            return;
        }

        //damage effects (sanity drain, pet attack etc)
        if (attacker.GetComponent<EnergyManager>() == null)
            Player_Controller.Instance.OnHitByEnemy(attacker);
    }

    public override void Heal(float amt) {
        PlayerData.CurrHP += amt;
        if (PlayerData.CurrHP > PlayerData.MaxHp)
            PlayerData.CurrHP = PlayerData.MaxHp;
    }

    public override void OnDeath() {
        PlayerData.CurrHP = 0;
        Death = true;
        //play death anim
        if (ar != null)
            ar.SetTrigger("Death");
        else
            OnDeathAnimEnd();
    }

    //called by animation event trigger (but not invoker, directly called by animation)
    public override void OnDeathAnimEnd() {
        //reset player data
        PlayerData.MaxHp = 0;
        PlayerData.CurrHP = 0;
        PlayerData.MaxSanity = 0;
        PlayerData.CurrSanity = 0;
        PlayerData.MaxStamina = 0;
        PlayerData.CurrStamina = 0;

        //reset pet
        PetManager.Instance.ResetAndRemovePet();
        //TODO: display game over screen

    }
}
