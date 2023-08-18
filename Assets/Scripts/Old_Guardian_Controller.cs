using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Old_Guardian_Controller : MonoBehaviour
{
    public enum State
    {
        IDLE,
        CHARGE,
        ATTACK,
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

    public float ChasePlayerTimer;
    public LayerMask Player;

    public float FOVradius = 5f;
    public float ATKradius = 2f;
    public LayerMask targetMask;


    // Start is called before the first frame update
    void Start()
    {
        isDead = false;

        StartIdleTimer = false;
        StartATKTimer = false;
        StartDeathTimer = false;
        ChasePlayerTimer = 3f;

        ChangeState(currentState);
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
        else if (currentState == State.DEATH)
        {
            Death();
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            isDead = true;
        }

        if(isDead)
        {
            aiPath.canMove = false;
            ChangeState(State.DEATH);
        }

        if(!isAttacking)
        {
            StopCoroutine(AttackTimer());
        }

        FOVCheck();
        
    }

    private void ChangeState(State next)
    {
        if (next == State.IDLE)
        {
            anim.SetBool("IsMoving", false);
        }
        else if (next == State.CHARGE)
        {
            anim.SetBool("IsMoving", true);
        }
        else if (next == State.ATTACK)
        {
            anim.SetBool("Attack", true);
        }
        else if (next == State.DEATH)
        {
            if(!StartDeathTimer)
            {
                anim.SetBool("Death", true);
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
            if (!StartATKTimer)
            {
                StartATKTimer = true;
                
                //anim.SetTrigger("Attack");
                StartCoroutine(AttackTimer());
            }
        }
        //AttackTimer();
        //currentState = State.ATTACK;
    }

    private void Death()
    {
        aiPath.canMove = false;
        Destroy(gameObject, 5f);
    }

    private void FOVCheck()
    {
        Collider2D[] detectedUnits = Physics2D.OverlapCircleAll(transform.position, FOVradius, targetMask);
        Collider2D[] ATKRange = Physics2D.OverlapCircleAll(transform.position, ATKradius, targetMask);

        //Debug.Log(detectedUnits.Length);
        if (ATKRange.Length != 0)
        {
            ChangeState(State.ATTACK);
            isAttacking = true;
            //if(!StartATKTimer)
            //{
            //    StartATKTimer = true;
            //    //anim.SetTrigger("Attack");
            //    StartCoroutine(AttackTimer());
            //}
        }

        else if (detectedUnits.Length != 0)
        {
            if(!isAttacking)
            {
                ChangeState(State.CHARGE);
            }
            //inRange = true;
            //ChasePlayerTimer = 3f;
        }

        else
        {
            //inRange = false;
            if(!StartIdleTimer)
            {
                StartIdleTimer = true;
                StartCoroutine(ReturnToIdle());
            }
        }
    }

    IEnumerator AttackTimer()
    {
        anim.SetBool("Attack", false);

        yield return new WaitForSeconds(0.75f);
        Debug.Log("PlayerDamaged");
        StartATKTimer = false;
        isAttacking = false;
        

        yield return new WaitForSeconds(1f);
        ChangeState(State.IDLE);
        //StopCoroutine(AttackTimer());
    }

    IEnumerator ReturnToIdle()
    {
        yield return new WaitForSeconds(3f);
        ChangeState(State.IDLE);
        StartIdleTimer = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, FOVradius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, ATKradius);
    }
}