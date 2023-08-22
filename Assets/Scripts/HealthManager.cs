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
    [Tooltip("If true, the bar will disappear, and reappear when this unit is damaged, for the duration specified in TimeToShowHPbar. " +
        "If false, the healthbar will be permanently visible, even in the dark")]
    [SerializeField] protected bool OnlyShowBarWhenDamaged = true;
    [Tooltip("In Seconds. When damaged, the hp bar flashes for a moment, this value is how long to flash for")]
    [Min(0)][SerializeField] protected float TimeToShowHPbar = 2;
    protected float HPbarShowTimer = 0f;

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
    protected virtual void Update() {
        if (OnlyShowBarWhenDamaged) {
            if (HPbarShowTimer > 0f) {
                UI_Hpbar.SetBarDisplay(true);
                HPbarShowTimer -= Time.deltaTime;
            }
            else {
                UI_Hpbar.SetBarDisplay(false);
            }
        }
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
        if (UI_Hpbar != null) {
            UI_Hpbar.SetBarDisplay(true);
            UI_Hpbar.UpdateSlider();
            HPbarShowTimer = TimeToShowHPbar;
        }
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

    public void ChangeDamageModifier(float amt)
    {
        DamageMultiplier += amt;
        if (DamageMultiplier < 0)
            DamageMultiplier = 0;
    }
}
