using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RedSmile_Controller : MonoBehaviour
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

    public bool isDead;
    private bool isAttacking;

    private bool StartIdleTimer;
    private bool StartATKTimer;
    private bool StartDeathTimer;

    public float ChasePlayerTimer;
    public LayerMask Player;

    public float FOVradius = 5f;
    public float FOVangle;
    public float ATKradius = 2f;
    public float DOTradius = 5f;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    private AIDestinationSetter SproutMinionAItarget;

    [SerializeField] 
    protected float WaitOutOfFOVtime = 2f; //default time till "give up"
    float OutOfFOVtimer; //the timer that counts down
    bool OutOfFOVtimerCountingDown = false; //to actually countdwon timer once per frame/deltatime
    bool canSeeUnit = false;
    Vector2 facing;

    private AIDestinationSetter DestSetter;

    private HealthManager healthManager;

    private BoxCollider2D boxCollider;

    [HideInInspector] public UnityEvent<GameObject> OnHitEvent;

    // Start is called before the first frame update
    void Start()
    {
        if (OnHitEvent == null)
            OnHitEvent = new UnityEvent<GameObject>();
        OnHitEvent.AddListener(FindAttacker);

        isDead = false;

        StartIdleTimer = false;
        StartATKTimer = false;
        StartDeathTimer = false;
        ChasePlayerTimer = 3f;

        ChangeState(currentState);
        //StartCoroutine(Die());

        SproutMinionAItarget = GetComponent<AIDestinationSetter>();

        if(SproutMinionAItarget.target == null)
        {
            SproutMinionAItarget.target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        DestSetter = GetComponent<AIDestinationSetter>();
        healthManager = GetComponent<HealthManager>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //healthManager.TakeDamage(10f, this.gameObject);
        if(healthManager.CurrentHealth <= 0)
        {
            ChangeState(State.DEATH);
        }

        if (isDead)
        {
            boxCollider.isTrigger = true;
            aiPath.canMove = false;
            ChangeState(State.DEATH);
        }

        if (!isDead)
        {
            FOVCheck();
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

    }
    private void LateUpdate()
    {
        //while moving, set Facing to be the dir it is moving to
        if (aiPath.desiredVelocity != Vector3.zero)
            facing = aiPath.desiredVelocity.normalized;
        else
        {
            //not moving, prevent facing.x from being 0
            facing.x = -transform.localScale.x;
        }
        Debug.DrawRay(transform.position, facing * FOVradius, Color.green);
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
                isDead = true;
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
                Player_HealthManager.Instance.TakeDamage(5f, gameObject);
                StartATKTimer = true;
                StartCoroutine(AttackCooldown());
            }
        }
    }

    //When hit by something, target the attacker if in LOS
    private void FindAttacker(GameObject attacker) {
        Vector2 dir = ((Vector2)attacker.transform.position - (Vector2)transform.position).normalized;
        float distanceToTarget = Vector2.Distance(transform.position, attacker.transform.position);
        //check for obstructions
        if (!Physics2D.Raycast(transform.position, dir, distanceToTarget, obstacleMask)) {
            canSeeUnit = true;
            OutOfFOVtimer = WaitOutOfFOVtime; //reset timer
            DestSetter.target = attacker.transform;
            ChangeState(State.CHARGE);
            //Debug.Log(unit.gameObject.name + " seen");
        }
        else {
            ChangeState(State.IDLE);
        }
    }

    private void Death()
    {
        boxCollider.enabled = false;
        isDead = true;
        //aiPath.canMove = false;
        Destroy(gameObject, 5f);
    }

    private void FOVCheck()
    {
        //Collider2D[] detectedUnits = Physics2D.OverlapCircleAll(transform.position, FOVradius, targetMask);
        //Collider2D[] ATKRange = Physics2D.OverlapCircleAll(transform.position, ATKradius, targetMask);

        //if (ATKRange.Length != 0)
        //{
        //    ChangeState(State.ATTACK);
        //    isAttacking = true;
        //}

        //else if (detectedUnits.Length != 0)
        //{
        //    if (!isAttacking)
        //    {
        //        //Debug.Log("MOVE");
        //        ChangeState(State.CHARGE);
        //    }
        //    //inRange = true;
        //    //ChasePlayerTimer = 3f;
        //}

        //else
        //{
        //    //inRange = false;
        //    if (!StartIdleTimer)
        //    {
        //        StartIdleTimer = true;
        //        StartCoroutine(ReturnToIdle());
        //    }
        //}

        //look for units (player layer)
        Collider2D[] detectedUnits = Physics2D.OverlapCircleAll(transform.position, FOVradius, targetMask);
        Collider2D[] ATKRange = Physics2D.OverlapCircleAll(transform.position, ATKradius, targetMask);
        Collider2D[] DOTRange = Physics2D.OverlapCircleAll(transform.position, DOTradius, targetMask);

        Transform unit;
        //check for the player, or targetable units, within range
        if(DOTRange.Length != 0)
        {
            //Drain Sanity
            //Debug.Log("Draining Sanity");
        }

        if(ATKRange.Length != 0)
        {
            foreach (Collider2D col in ATKRange)
            {
                if(col.gameObject.tag == "Player")
                {
                    ChangeState(State.ATTACK);
                    isAttacking = true;
                    break;
                }
            }
        }

        else if (detectedUnits.Length != 0)
        {
            //find nearest GO for target
            float smallestDist = Mathf.Infinity;
            Transform nearestGO = null;
            foreach (Collider2D col in detectedUnits)
            {
                if (col.gameObject.tag != "Player")
                {
                    continue;
                }
                //get dist between this enemy and the potential target (sqrMagnitude is distance, but maybe faster?)
                float dist = ((Vector2)transform.position - (Vector2)col.transform.position).sqrMagnitude;
                if (dist < smallestDist)
                {
                    //if the curr target is nearer than the prev target, this target is the new closest one
                    smallestDist = dist;
                    nearestGO = col.gameObject.transform;
                }
            }
            //Debug.Log("nearest Go: " + nearestGO?.name);
            unit = nearestGO; //make nearest potential target the target
            if (unit == null)
                return;
            Vector2 dir = ((Vector2)unit.position - (Vector2)transform.position).normalized;
            //check if unit is within FOV angle
            if (Vector2.Angle(facing, dir) <= FOVangle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, unit.position);
                //check for obstructions
                if (!Physics2D.Raycast(transform.position, dir, distanceToTarget, obstacleMask))
                {
                    canSeeUnit = true;
                    OutOfFOVtimer = WaitOutOfFOVtime; //reset timer
                    DestSetter.target = unit;
                    ChangeState(State.CHARGE);
                    //Debug.Log(unit.gameObject.name + " seen");
                }
                else if (canSeeUnit)
                    OutOfFOVtimerCountingDown = true; //saw player move behind obstacle, keep following
            }
            else if (canSeeUnit)
            {
                //saw player, then lost LOS, keep following.
                OutOfFOVtimerCountingDown = true;
            }
        }
        else if (canSeeUnit)
        { //saw player move out of range, give up
            LostUnit();
        }
        //using this bool since there are multiple places that need to count down, 
        //but only want to -time once per frame
        if (OutOfFOVtimerCountingDown)
        {
            OutOfFOVtimer -= Time.fixedDeltaTime;
            OutOfFOVtimerCountingDown = false;
        }
        if (OutOfFOVtimer <= 0f)
        {
            LostUnit();
        }
    }
    protected virtual void LostUnit()
    {
        //OutOfFOVtimer ran out, enemy "give up" pursuing player
        //Debug.Log(gameObject.name + " Lost " + target.gameObject.name);
        canSeeUnit = false;
        DestSetter.target = null;
        ChangeState(State.IDLE);
        OutOfFOVtimer = WaitOutOfFOVtime;
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

    IEnumerator Die()
    {
        yield return new WaitForSeconds(15f);
        ChangeState(State.DEATH);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, FOVradius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, ATKradius);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, DOTradius);
    }
}
