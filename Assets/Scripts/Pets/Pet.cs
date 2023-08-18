using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using TMPro;

public abstract class Pet : MonoBehaviour
{
    protected GameObject player;
    [SerializeField] SO_PetDetails details;

    [Header("Base stats")]
    public float Base_MaxHunger;
    public float Base_HungerDrain;

    protected Rigidbody2D rb;
    protected Animator ar;
    protected SpriteRenderer sr;
    protected Vector2 facing;
    protected Vector2 force;

    [Header("Pathfinding")]
    public Transform target;
    [SerializeField] protected float moveSpeed;
    public float nextWaypointDist;
    protected Path path;
    [HideInInspector] public int currentWaypoint = 0;
    protected Seeker seeker;

    [Header("Sprite")]
    [SerializeField] protected bool OriginalSpriteFaceRight = false;

    [Header("UI")]
    [SerializeField] public TMP_Text Nametag;

    // Start is called before the first frame update
    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        ar = transform.GetChild(0).gameObject.GetComponent<Animator>();
        sr = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        seeker = GetComponent<Seeker>();

        target = player.transform;
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    protected virtual void FixedUpdate() {
        if (target == null)
            return;

        //calculate where to go and move towards it
        FollowPath();

        //"face" the target
        facing = (target.transform.position - gameObject.transform.position).normalized;

        //target dead, return to player
        if (!target.gameObject.activeSelf) {
            target = player.transform;
        }

        //cap velocity
        rb.velocity = new Vector2(Mathf.Min(rb.velocity.x, moveSpeed), Mathf.Min(rb.velocity.y, moveSpeed * 2));
    }
    private void LateUpdate() {
        UpdateAnimation();
        //teleport back to player if too far
        if (Vector2.Distance(player.transform.position, transform.position) > 10) {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        }
    }

    void UpdateAnimation() {
        if (ar == null)
            return;

        //update if current anim ends (normalizedTime>1 means 1 cycle completed)
        if (ar.GetCurrentAnimatorStateInfo(0).normalizedTime > 1) {
            if (Mathf.Abs(rb.velocity.x) <= 0.001f) ar.Play("Dog_Idle");
            if (Mathf.Abs(rb.velocity.x) > 0.001f) ar.Play("Dog_Run");
        }

        if (facing.x > 0) {
            if (OriginalSpriteFaceRight)
                sr.flipX = true;
            else
                sr.flipX = false; 
        }
        else if (facing.x < 0) {
            if (OriginalSpriteFaceRight)
                sr.flipX = false;
            else
                sr.flipX = true;
        }
    }

    void UpdatePath() {
        if (seeker.IsDone() && target != null) {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
            currentWaypoint = 0; //start at beginning of path
        }
    }

    private void FollowPath() {
        if (path == null)
            return;

        //check not at the end of waypoints
        if (currentWaypoint >= path.vectorPath.Count) {
            return;
        }

        Vector2 dir = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        force = dir * moveSpeed;

        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDist) {
            currentWaypoint++;
        }

        //to stop unit from moving when close to target
        if (Vector2.Distance((Vector2)target.position, (Vector2)transform.position) > nextWaypointDist)
            rb.AddForce(force);
    }

    public virtual void DrainHunger(float amt) {
        details.CurrentHunger -= amt;
        //starved to death
        if (details.CurrentHunger <= 0) {
            PetManager.PetDie?.Invoke();
        }
    }

    public virtual void OwnerAttacked(GameObject attacker) {
        //leave empty, set by the child classes
    }

    public virtual void TargetEnemyAttacked(GameObject enemy) {
        //leave empty, set by the child classes
    }
}
