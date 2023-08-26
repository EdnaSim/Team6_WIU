using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWorm_Controller : MonoBehaviour
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

    public GameObject FirePoint;
    public GameObject FireBall;
    public float FireballSpeed = 10f;

    private bool isDead;
    private bool isAttacking;

    private bool StartIdleTimer;
    private bool StartATKTimer;
    private bool StartDeathTimer;

    public float ChasePlayerTimer;

    public float FOVradius = 5f;
    public float ATKradius = 2f;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    private AIDestinationSetter FireWormtarget;

    private HealthManager healthManager;

    // Start is called before the first frame update
    void Start()
    {
        StartIdleTimer = false;
        StartATKTimer = false;
        StartDeathTimer = false;
        ChasePlayerTimer = 3f;

        ChangeState(currentState);

        healthManager = GetComponent<HealthManager>();

        FireWormtarget = GetComponent<AIDestinationSetter>();

        if (FireWormtarget.target == null)
        {
            FireWormtarget.target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (healthManager.CurrentHealth <= 0)
        {
            ChangeState(State.DEATH);
        }

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

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    isDead = true;
        //}

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
        else if (next == State.DEATH)
        {
            if (!StartDeathTimer)
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

            //Switch to Cooldown
            if (!StartATKTimer)
            {
                StartATKTimer = true;
                StartCoroutine(Cooldown());
            }
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

        if(ATKRange.Length != 0)
        {
            ChangeState(State.ATTACK);
            isAttacking = true;
        }

        else if (detectedUnits.Length != 0)
        {
            foreach (Collider2D col in detectedUnits)
            {
                if (col.gameObject.tag == "Player")
                {
                    if (!isAttacking)
                    {
                        //Debug.Log("MOVE");
                        ChangeState(State.CHARGE);
                    }
                    //inRange = true;
                    //ChasePlayerTimer = 3f;
                }
            }
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

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(0.6f);
        var bullet = Instantiate(FireBall, FirePoint.transform.position, FirePoint.transform.rotation);
        //bullet.GetComponent<Rigidbody2D>().rotation = 180;
        bullet.GetComponent<Rigidbody2D>().velocity = FirePoint.transform.right * FireballSpeed;
        ChangeState(State.IDLE);

        yield return new WaitForSeconds(0.7f);
        isAttacking = false;
        StartATKTimer = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, FOVradius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, ATKradius);
    }
}
