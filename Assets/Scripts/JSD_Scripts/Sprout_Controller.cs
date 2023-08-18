using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprout_Controller : MonoBehaviour
{
    public enum State
    {
        IDLE,
        CHARGE,
        ATTACK,
        SUMMON,
        DEATH
    }
    [SerializeField] private State currentState;

    public Animator anim;

    public AIPath aiPath;

    private bool isDead;
    private bool isAttacking;

    private bool StartIdleTimer;
    private bool StartATKTimer;
    private bool StartDeathTimer;
    private bool StartSummonTimer;

    public float ChasePlayerTimer;
    public LayerMask Player;

    public float FOVradius = 5f;
    public float ATKradius = 2f;
    public LayerMask targetMask;

    public float MinSummonTime = 3f;
    public float MaxSummonTime = 5f;
    private float SummonTimer;

    public GameObject MinionSummon;

    // Start is called before the first frame update
    void Start()
    {
        StartIdleTimer = false;
        StartATKTimer = false;
        StartDeathTimer = false;
        ChasePlayerTimer = 3f;

        ChangeState(currentState);

        SummonTimer = Random.Range(MinSummonTime, MaxSummonTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == State.IDLE)
        {
            Idle();
        }
        else if (currentState == State.CHARGE)
        {
            Charge();
        }
        else if (currentState == State.ATTACK)
        {
            Attack();
        }
        else if (currentState == State.SUMMON)
        {
            Summon();
        }
        else if (currentState == State.DEATH)
        {
            Death();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            isDead = true;
        }

        if (isDead)
        {
            aiPath.canMove = false;
            ChangeState(State.DEATH);
        }
        FOVCheck();
    }

    private void ChangeState(State next)
    {
        if (next == State.IDLE)
        {
            anim.SetBool("IsMoving", false);
            anim.SetBool("IsSummon", false);
            isAttacking = false;
        }
        else if (next == State.CHARGE)
        {
            anim.SetBool("IsMoving", true);
        }
        else if (next == State.ATTACK)
        {
            anim.SetTrigger("Attack");
            isAttacking = true;
        }
        else if (next == State.SUMMON)
        {
            anim.SetTrigger("Attack");
            anim.SetBool("IsSummon", true);
        }
        else if (next == State.DEATH)
        {
            if (!StartDeathTimer)
            {
                anim.SetBool("IsDead", true);
                anim.SetTrigger("Die");
                StartDeathTimer = true;
            }
        }

        currentState = next;
    }

    private void Idle()
    {
        if (!isDead)
        {
            aiPath.canMove = false;
            anim.SetBool("IsMoving", false);
        }
    }

    private void Charge()
    {
        if (!isDead)
        {
            if (!StartATKTimer)
            {
                aiPath.canMove = true;
                anim.SetBool("IsMoving", true);
            }
        }
    }

    private void Attack()
    {
        if (!isDead)
        {
            aiPath.canMove = false;

            //Switch to Cooldown
            if (!StartATKTimer)
            {
                StartATKTimer = true;
                StartCoroutine(AttackCooldown());
            }
        }
    }

    private void Summon()
    {
        if (!isDead)
        {
            //aiPath.canMove = false;
            Instantiate(MinionSummon, transform.position, transform.rotation);
            Debug.Log("dibsibesfeio");
            StartCoroutine(SummonCooldown());
        }
    }

    private void Death()
    {
        isDead = true;
        Destroy(gameObject, 5f);
    }

    private void FOVCheck()
    {
        Collider2D[] detectedUnits = Physics2D.OverlapCircleAll(transform.position, FOVradius, targetMask);
        Collider2D[] ATKRange = Physics2D.OverlapCircleAll(transform.position, ATKradius, targetMask);

        if (ATKRange.Length != 0)
        {
            ChangeState(State.ATTACK);
            isAttacking = true;
        }

        else if (detectedUnits.Length != 0)
        {
            SummonTimer -= Time.deltaTime;
            if (!isAttacking && !StartSummonTimer)
            {
                //Debug.Log("MOVE");
                ChangeState(State.CHARGE);
            }

            if (SummonTimer <= 0)
            {
                StartSummonTimer = true;
                aiPath.canMove = false;
                ChangeState(State.SUMMON);
                SummonTimer = Random.Range(MinSummonTime, MaxSummonTime);
            }
            //inRange = true;
            //ChasePlayerTimer = 3f;
        }

        else
        {
            //inRange = false;
            if (!StartIdleTimer)
            {
                StartIdleTimer = true;
                StartCoroutine(ReturnToIdle());
            }
        }
    }

    IEnumerator ReturnToIdle()
    {
        yield return new WaitForSeconds(3f);
        ChangeState(State.IDLE);
        StartIdleTimer = false;
    }

    IEnumerator AttackCooldown()
    {
        ChangeState(State.IDLE);
        yield return new WaitForSeconds(0.7f);
        isAttacking = false;
        StartATKTimer = false;
    }

    IEnumerator SummonCooldown()
    {
        ChangeState(State.IDLE);
        yield return new WaitForSeconds(0.4f);
        StartSummonTimer = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, FOVradius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, ATKradius);
    }
}
