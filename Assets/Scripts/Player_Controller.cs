using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    Rigidbody2D rb;
    Animator ar;
    SpriteRenderer sr;

    public bool isPaused = false;

    [Header("Movement")]
    [SerializeField] float Base_MovementSpeed;
    float MovementSpeed;
    [SerializeField] float SprintIncrease;
    float dirH;
    float dirV;
    Vector2 facing;

    bool isAttacking = false; //check if bound to an attack anim (set by anim trigger)
    [Header("Attack")]
    [SerializeField] float MeleeDamage;
    [SerializeField] float MeleeCooldown;
    [SerializeField] Vector2 MeleeRange;
    [SerializeField] Transform MeleeOrigin;
    bool CanMelee = true;

    [Header("Ranged attack")]
    [SerializeField] float RangeDamage;
    [SerializeField] float RangedCooldown;
    [SerializeField] float RangedAttackRange;
    [SerializeField] float RangedProjSpeed;
    [SerializeField] GameObject projPrefab;
    [SerializeField] int PooledProjTotal;
    bool CanRange = true;
    List<Projectile> PooledProjectiles;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ar = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        MovementSpeed = Base_MovementSpeed;

        PooledProjectiles = new List<Projectile>();
        for (int i = 0; i < PooledProjTotal; i++) {
            Projectile temp = Instantiate(projPrefab).GetComponent<Projectile>();
            temp.damage = RangeDamage;
            temp.caster = gameObject;
            temp.speed = RangedProjSpeed;
            temp.MaxRange = RangedAttackRange;
            temp.FalloffDist = RangedAttackRange / 2;
            temp.targetLayer = LayerMask.GetMask("Enemy");
            PooledProjectiles.Add(temp);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //stuff that cant be done when paused
        if (!isPaused) {
            //get input for moving
            dirH = Input.GetAxis("Horizontal");
            dirV = Input.GetAxis("Vertical");
            //holding down sprint key
            if (Input.GetKey(KeyCode.LeftShift)) {
                //Sprint
                MovementSpeed = Base_MovementSpeed + SprintIncrease;
            }
            else {
                MovementSpeed = Base_MovementSpeed;
            }

            //input
            if (Input.GetKeyDown(KeyCode.F)) {
                //Interact
            }
            if (Input.GetKeyDown(KeyCode.M)) {
                //Open map
            }
            if (Input.GetKeyDown(KeyCode.B)) {
                //Open Inventory
            }
            if (Input.GetMouseButtonDown(0)) {
                //Attack (left click)
                if (CanMelee && !isAttacking) {
                    CanMelee = false;
                    //isAttacking = true;
                    //ar.SetTrigger("Melee");
                    StartMelee();
                    StartCoroutine(StartMeleeCooldown());
                }
            }
            if (Input.GetMouseButtonDown(1)) {
                //Ranged attack (right click)
                if (CanRange && !isAttacking) {
                    CanRange = false;
                    //isAttacking = true;
                    StartRanged();
                    StartCoroutine(StartRangedCooldown());
                }
            }

            //Play audio
        }
        //can be done when paused
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) {
            //pause / unpause
            SetPause();
        }

    }
    private void FixedUpdate() {//move player
        rb.AddForce(new Vector2(dirH * MovementSpeed, dirV * MovementSpeed));
        //cap velocity to MovementSpeed
        rb.velocity = new Vector2(Mathf.Min(MovementSpeed, rb.velocity.x), Mathf.Min(MovementSpeed, rb.velocity.y));
    }

    private void LateUpdate() {
        UpdateMouseDirection();
        UpdateAnimation();
        UpdateSpriteHorizontalDirection();

        //move the origin to face the mouse
        MeleeOrigin.transform.position = new Vector2(gameObject.transform.position.x + facing.x, gameObject.transform.position.y + facing.y);
    }
    //TEMP
    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(MeleeOrigin.transform.position, MeleeRange);
    }

    public void SetPause(int DesiredPause = 2) {
        //0: false
        //1: true
        //2 or more: flip
        if (DesiredPause == 0)
            isPaused = false;
        else if (DesiredPause == 1)
            isPaused = true;
        else
            isPaused = !isPaused;

        //change time scale
        if (isPaused) {
            Time.timeScale = 0;
        }
        else {
            Time.timeScale = 1;
        }
    }

    //called by animation event trigger ("Melee")
    private void StartMelee() {
        //AOE
        foreach (Collider2D col in Physics2D.OverlapBoxAll(MeleeOrigin.position, MeleeRange, 0, LayerMask.GetMask("Enemy"))){
            Debug.Log(col.gameObject.name);
            HealthManager hm = col.gameObject.GetComponent<HealthManager>();
            if (hm != null) {
                hm.TakeDamage(MeleeDamage, gameObject);
            }
        }
    }
    IEnumerator StartMeleeCooldown() {
        yield return new WaitForSeconds(MeleeCooldown);
        CanMelee = true;
    }

    private void StartRanged() {
        foreach (Projectile p in PooledProjectiles) {
            if (p.gameObject.activeSelf) {
                continue;
            }
            p.dir = facing;
            p.ReSummon();
            break;
        }
    }
    IEnumerator StartRangedCooldown() {
        yield return new WaitForSeconds(RangedCooldown);
        CanRange = true;
    }

    private void UpdateAnimation() {
        if (ar == null)
            return;

        //update if current anim ends (normalizedTime>1 means 1 cycle completed)
        if (ar.GetCurrentAnimatorStateInfo(0).normalizedTime > 1) {
            //if (Mathf.Abs(rb.velocity.x) <= 0.001f) ar.Play("Player_Idle");
            //if (Mathf.Abs(rb.velocity.x) > 0.001f) ar.Play("Player_Run");

            //if (dirV > 0f) ar.Play("Player_Right");
            //else if (dirV < 0f) ar.Play("Player_Left");
        }
    }

    private void UpdateSpriteHorizontalDirection() {
        if (dirH > 0f) {
            sr.flipX = true;
        }
        else if (dirH < 0f) {
            sr.flipX = false;
        }

        //switches the whole gameobject, since the weapon hitboxes will be attached to the GO
        //if (dirH > 0f) transform.rotation = Quaternion.Euler(0, 0, 0);
        //else if (dirH < 0f) transform.rotation = Quaternion.Euler(0, 180, 0);
        //NOTE: this causes objects attached to the player to have their z-axis value flipped
    }

    private void UpdateMouseDirection() {
        Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //get direction 
        facing = new Vector2(mousepos.x - gameObject.transform.position.x, mousepos.y - gameObject.transform.position.y).normalized;
    }

    //called by anim event triggers when the anim is "over"
    public void ResetIsAttacking() {
        isAttacking = false;
    }
}
