using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet_Dog : Pet
{
    [Header("Attack")]
    [Min(0)] [SerializeField] float AttackRange;
    [SerializeField] float damage;
    [Min(0)] [SerializeField] float AttCd;
    bool CanAttack = true;

    protected override void FixedUpdate() {
        base.FixedUpdate();
        
        //near enemy, attack
        if (target != player.transform && Vector2.Distance((Vector2)target.position, (Vector2)transform.position) < nextWaypointDist) {
            if (CanAttack) {
                ar.SetTrigger("Attack");
                Attack();
            }
        }
    }

    void Attack() {
        if (target == null)
            return;

        CanAttack = false;
        HealthManager TargetHm = target.gameObject.GetComponent<HealthManager>();
        if (TargetHm != null && Vector2.Distance(transform.position, target.position) <= AttackRange) {
            TargetHm.TakeDamage(damage, gameObject);
        }
        StartCoroutine(WaitForCooldown());
    }

    protected IEnumerator WaitForCooldown() {
        yield return new WaitForSeconds(AttCd);
        CanAttack = true;
    }

    public override void OwnerAttacked(GameObject attacker) {
        base.OwnerAttacked(attacker);

        if (attacker != null) {
            target = attacker.transform;
        }
    }

    public override void TargetEnemyAttacked(GameObject enemy) {
        target = enemy.transform;
    }
}
