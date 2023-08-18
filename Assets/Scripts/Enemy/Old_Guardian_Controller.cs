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
    private AIDestinationSetter DestSetter;

    private bool isDead;
    private bool isAttacking;
    
    private bool StartIdleTimer;
    private bool StartATKTimer;
    private bool StartDeathTimer;

    public float ChasePlayerTimer;
    public LayerMask Player;

    [Header("Sight")]
    public float FOVradius = 5f;
    [Min(0)][SerializeField] float FOVangle;
    public float ATKradius = 2f;
    public LayerMask targetMask;
    [SerializeField] LayerMask obstacleMask;

    //FOV timer stuff, so enemy doesnt lose the target just by moving slightly past them (due to FOV)
    [SerializeField] protected float WaitOutOfFOVtime = 2f; //default time till "give up"
    float OutOfFOVtimer; //the timer that counts down
    bool OutOfFOVtimerCountingDown = false; //to actually countdwon timer once per frame/deltatime
    bool canSeeUnit = false;
    Vector2 facing;

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;

        StartIdleTimer = false;
        StartATKTimer = false;
        StartDeathTimer = false;
        ChasePlayerTimer = 3f;

        DestSetter = GetComponent<AIDestinationSetter>();

        ChangeState(currentState);
    }

    // Update is called once per frame
    void Update()
    {
        FOVCheck();
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
    }
    private void LateUpdate() {
        //while moving, set Facing to be the dir it is moving to
        if (aiPath.desiredVelocity != Vector3.zero)
            facing = aiPath.desiredVelocity.normalized;
        else {
            //not moving, prevent facing.x from being 0
            facing.x = -transform.localScale.x;
        }
        Debug.DrawRay(transform.position, facing*FOVradius, Color.green);
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
        //look for units (player layer)
        Collider2D[] detectedUnits = Physics2D.OverlapCircleAll(transform.position, FOVradius, targetMask);

        Transform unit;
        //check for the player, or targetable units, within range
        if (detectedUnits.Length != 0) {
            //find nearest GO for target
            float smallestDist = Mathf.Infinity;
            Transform nearestGO = null;
            foreach (Collider2D col in detectedUnits) {
                if (col.gameObject.tag != "Player" && col.gameObject.tag != "Minion") {
                    continue;
                }
                //get dist between this enemy and the potential target (sqrMagnitude is distance, but maybe faster?)
                float dist = ((Vector2)transform.position - (Vector2)col.transform.position).sqrMagnitude;
                if (dist < smallestDist) {
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
            if (Vector2.Angle(facing, dir) <= FOVangle / 2) {
                float distanceToTarget = Vector2.Distance(transform.position, unit.position);
                //check for obstructions
                if (!Physics2D.Raycast(transform.position, dir, distanceToTarget, obstacleMask)) {
                    canSeeUnit = true;
                    OutOfFOVtimer = WaitOutOfFOVtime; //reset timer
                    DestSetter.target = unit;
                    ChangeState(State.CHARGE);
                    //Debug.Log(unit.gameObject.name + " seen");
                }
                else if (canSeeUnit)
                    OutOfFOVtimerCountingDown = true; //saw player move behind obstacle, keep following
            }
            else if (canSeeUnit) {
                //saw player, then lost LOS, keep following.
                OutOfFOVtimerCountingDown = true;
            }
        }
        else if (canSeeUnit) { //saw player move out of range, give up
            LostUnit();
        }
        //using this bool since there are multiple places that need to count down, 
        //but only want to -time once per frame
        if (OutOfFOVtimerCountingDown) {
            OutOfFOVtimer -= Time.fixedDeltaTime;
            OutOfFOVtimerCountingDown = false;
        }
        if (OutOfFOVtimer <= 0f) {
            LostUnit();
        }
    }
    protected virtual void LostUnit() {
        //OutOfFOVtimer ran out, enemy "give up" pursuing player
        //Debug.Log(gameObject.name + " Lost " + target.gameObject.name);
        canSeeUnit = false;
        DestSetter.target = null;
        ChangeState(State.IDLE);
        OutOfFOVtimer = WaitOutOfFOVtime;
    }

    IEnumerator AttackTimer()
    {
        anim.SetBool("Attack", false);

        yield return new WaitForSeconds(0.75f);
        //Debug.Log("PlayerDamaged");
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
