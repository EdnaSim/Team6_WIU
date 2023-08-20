using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour
{
    protected Animator ar;

    [Header("Health")]
    [SerializeField] protected float Base_MaxHealth;
    [HideInInspector] public float MaxHealth;
    [HideInInspector] public float CurrentHealth;

    [Header("UI")]
    [SerializeField] protected UI_Healthbar UI_Hpbar;

    [Header("Modifiers")]
    public float DamageMultiplier = 1;

    [HideInInspector] public bool Death = false;
    protected GameObject killer;

    



    // Start is called before the first frame update
    void Start()
    {
        if (MaxHealth <= 0)
            MaxHealth = Base_MaxHealth;
        if (CurrentHealth <= 0)
            CurrentHealth = MaxHealth;

        UI_Hpbar?.UpdateSlider();

        ar = transform.GetComponentInChildren<Animator>();

        killer = null;
    }

    public virtual void TakeDamage(float damage, GameObject attacker) {
        if (Death || attacker.layer == gameObject.layer)
            return;

        CurrentHealth -= (damage * DamageMultiplier);
        if (CurrentHealth <= 0) {
            killer = attacker;
            OnDeath();
        }
        //update after taking damage
        if (UI_Hpbar != null)
            UI_Hpbar.UpdateSlider();
    }

    public virtual void Heal(float amt) {
        CurrentHealth += amt;
        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;

        UI_Hpbar.UpdateSlider();
    }

    public virtual void OnDeath() {
        CurrentHealth = 0;
        Death = true;
        //play death anim
        if (ar != null)
            ar.SetTrigger("Die");
        else
            OnDeathAnimEnd();
    }

    //animation event triggers this
    public virtual void OnDeathAnimEnd() {
        gameObject.SetActive(false);
    }
}
