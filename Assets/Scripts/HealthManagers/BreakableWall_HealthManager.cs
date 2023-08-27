using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall_HealthManager : HealthManager
{
    protected Vector2 startingPos;
    protected bool shaking = false;
    protected float ShakeTimer = 0f;
    [Header("Shaking on-hit")]
    [SerializeField] protected float ShakeSpeed = 1f;
    [Tooltip("Amplitude")]
    [SerializeField] protected float ShakeAmount = 1f;
    [Tooltip("Shake for how long")]
    [SerializeField] protected float TimeToShake = 0.5f;

    // Start is called before the first frame update
    protected override void Start()
    {
        startingPos = transform.position;
        MaxHealth = Base_MaxHealth;
        CurrentHealth = MaxHealth;
    }

    // Update is called once per frame
    protected override void Update()
    {
        //count down shake timer
        if (ShakeTimer > 0f) {
            ShakeTimer -= Time.deltaTime;
        }
        else if (shaking) {
            shaking = false;
            transform.position = startingPos;
        }

        //shake the wall a bit
        if (shaking) {
            transform.position = new Vector3(startingPos.x + Mathf.Sin(Time.time * ShakeSpeed) * ShakeAmount, 
                startingPos.y + Mathf.Sin(Time.time * ShakeSpeed) * ShakeAmount, 
                transform.position.z);
        }
    }

    public override void TakeDamage(float damage, GameObject attacker) {
        if (Death) return;

        CurrentHealth -= (damage * DamageMultiplier);
        if (CurrentHealth <= 0) {
            killer = attacker;
            OnDeath();
        }
        if (!shaking) {
            shaking = true;
            ShakeTimer = TimeToShake;
        }
    }

    public override void Heal(float amt) {
        //left empty to override
    }
}
