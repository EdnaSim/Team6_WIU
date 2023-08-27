using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player_Controller : MonoBehaviour
{
    public static Player_Controller Instance;

    Rigidbody2D rb;
    Animator ar;
    SpriteRenderer sr;
    public ParticleSystem ps;
    [HideInInspector] public WeaponController wc;
    [SerializeField] SO_WeaponList wl;
    Vector2 PrevMoveDir;

    public bool isPaused = false;

    [Header("Movement")]
    public float Base_MovementSpeed;
    [HideInInspector] public float MovementSpeed;
    [SerializeField] float SprintIncrease;
    [HideInInspector] public Vector2 MoveDir;
    Vector2 facing;
    float RestTimer = 0f;
    bool Tired = false;

    bool isAttacking = false; //check if bound to an attack anim (set by anim trigger)
    [Header("Attack")]
    [SerializeField] Transform MeleeOrigin;
    [HideInInspector] public bool CanMelee = true;

    [Header("Ranged attack")]
    [HideInInspector] public bool CanRange = true;

    [Header("Pet")]
    [SerializeField] SO_PetDetails PetDetails;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ar = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        ps = GetComponent<ParticleSystem>();

        MovementSpeed = Base_MovementSpeed;

        wc = GetComponent<WeaponController>();
    }

    // Update is called once per frame
    void Update()
    {
        //stuff that cant be done when paused
        if (!isPaused && !Player_HealthManager.Instance.Death) {
            //get input for moving
            //MoveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
            // Use KeybindManager to get movement keys
            KeyCode moveUpKey = KeyCode.W; // Default value if KeybindManager is not available
            KeyCode moveDownKey = KeyCode.S;
            KeyCode moveLeftKey = KeyCode.A;
            KeyCode moveRightKey = KeyCode.D;
            KeyCode sprintKey = KeyCode.LeftShift;
            KeyCode attackKey = KeyCode.Mouse0;

            if (KeybindManager.Instance != null) {
                moveUpKey = KeybindManager.Instance.GetKeyForAction("Up");
                moveDownKey = KeybindManager.Instance.GetKeyForAction("Down");
                moveLeftKey = KeybindManager.Instance.GetKeyForAction("Left");
                moveRightKey = KeybindManager.Instance.GetKeyForAction("Right");
                sprintKey = KeybindManager.Instance.GetKeyForAction("Sprinting");
                attackKey = KeybindManager.Instance.GetKeyForAction("Attacking");
            }

            // Check movement input
            Vector2 newMoveDir = Vector2.zero;
            if (Input.GetKey(moveUpKey))
                newMoveDir.y = 1;
            if (Input.GetKey(moveDownKey))
                newMoveDir.y = -1;
            if (Input.GetKey(moveLeftKey))
                newMoveDir.x = -1;
            if (Input.GetKey(moveRightKey))
                newMoveDir.x = 1;

            MoveDir = newMoveDir.normalized;
            if (MoveDir == Vector2.zero) { //not moving
                RestTimer += Time.deltaTime;
            }
            else {
                RestTimer = 0f;
                EnergyManager.Instance.Regen = false;
            }
            //rested long enough, regen stamina
            if (RestTimer >= EnergyManager.Instance.RestTimeTillRegenStamina && !EnergyManager.Instance.Regen) {
                EnergyManager.Instance.StartRegen();
            }
            //holding down sprint key
            if (Input.GetKey(sprintKey) && PlayerData.CurrStamina > 0f) {
                //Sprint
                MovementSpeed = Base_MovementSpeed + SprintIncrease;
                if (MoveDir != Vector2.zero)
                    EnergyManager.Instance.isRunning = true;
            }
            else if (Input.GetKeyUp(sprintKey)) {
                if (EnergyManager.Instance.isRunning)
                    MovementSpeed -= SprintIncrease;
                EnergyManager.Instance.isRunning = false;
            }
            else if (PlayerData.CurrStamina <= 0 && !Tired) {
                Tired = true;
                if (EnergyManager.Instance.isRunning)
                    MovementSpeed -= SprintIncrease;
                EnergyManager.Instance.isRunning = false;
                
            }
            if (Input.GetKey(attackKey)) {
                //if clicked on UI / no object selected
                if (!EventSystem.current.IsPointerOverGameObject() 
                    && InventoryManager.Instance != null && InventoryManager.Instance.getSelected() != null) {

                    if (InventoryManager.Instance.getSelected().type == Item.itemType.melee) {
                        //melee attack
                        if (!isAttacking && wc.CanMelee()) {
                            ChangeIsAttacking(true);
                            ar.SetFloat("MouseX", facing.x);
                            ar.SetFloat("MouseY", facing.y);
                            ar.SetTrigger("Melee");

                            //make pet target enemy "attacked"
                            //using radiusX since its the height from the player to the tip of the box
                            MakePetTargetAttackedEnemy(wc.MeleeStats.RadiusX + 1);
                        }
                    }
                    else if (InventoryManager.Instance.getSelected().type == Item.itemType.ranged) {
                        //ranged attack
                        if (!isAttacking && wc.CanRanged()) {
                            ChangeIsAttacking(true);
                            ar.SetFloat("MouseX", facing.x);
                            ar.SetFloat("MouseY", facing.y);
                            ar.SetTrigger("Ranged");

                            //make pet target enemy "attacked"
                            MakePetTargetAttackedEnemy(wc.RangedStats.MaxRange);
                        }
                    }
                }
            }
        }
        //can be done when paused
        //if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) {
        //    //pause / unpause
        //    SetPause();
        //}
    }
    private void FixedUpdate() {//move player
        rb.AddForce(MoveDir * MovementSpeed);
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
    //TEMP - draw melee range (also rotate it)
    //private void OnDrawGizmos() {
    //    var oldMatrix = Gizmos.matrix;

    //    // create a matrix which translates an object by "position", rotates it by "rotation" and scales it by "halfExtends * 2"
    //    Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(facing.y, facing.x) * Mathf.Rad2Deg);
    //    if (wc.MeleeStats != null)
    //        Gizmos.matrix = Matrix4x4.TRS(MeleeOrigin.position, rotation, new Vector2(wc.MeleeStats.RadiusX, wc.MeleeStats.RadiusY));
    //    // Then use it one a default cube which is not translated nor scaled
    //    Gizmos.DrawWireCube(Vector3.zero, Vector3.one);

    //    Gizmos.matrix = oldMatrix;
    //}

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
    public void StartMelee() {
        //AOE
        wc.Melee(MeleeOrigin.position, facing);
        //play audio
    }

    public void StartRanged() {
        if (wc.Fire(facing)) {
            //play audio
        }
    }

    private void UpdateAnimation() {
        if (ar == null)
            return;

        //ac.SetDirection(MoveDir);
        
        //update if current anim ends (normalizedTime>1 means 1 cycle completed)
        if (ar.GetCurrentAnimatorStateInfo(0).normalizedTime > 1) {
            if (Mathf.Abs(rb.velocity.x) <= 0.01f && Mathf.Abs(rb.velocity.y) <= 0.001f) ar.SetBool("Idle", true);
            if (Mathf.Abs(rb.velocity.x) > 0.01f || Mathf.Abs(rb.velocity.y) > 0.001f) ar.SetBool("Idle", false);


            if (MoveDir.magnitude != 0) {
                ar.SetFloat("MoveDirX", MoveDir.x);
                ar.SetFloat("MoveDirY", MoveDir.y);
                PrevMoveDir = MoveDir;
            }
            if (ar.GetBool("Idle")) {
                ar.SetFloat("PrevMoveDirX", PrevMoveDir.x);
                ar.SetFloat("PrevMoveDirY", PrevMoveDir.y);
            }
        }
    }

    private void UpdateSpriteHorizontalDirection() {
        //if (MoveDir.x > 0f) {
        //    sr.flipX = true;
        //}
        //else if (MoveDir.y < 0f) {
        //    sr.flipX = false;
        //}

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
    public void ChangeIsAttacking(bool b) {
        isAttacking = b;
        ar.SetBool("isAttacking", b);
    }

    public void OnHitByEnemy(GameObject attacker) {
        //drain sanity
        if (SanityManager.Instance != null)
            SanityManager.Instance.ChangeSanity(-SanityManager.Instance.DrainAmtOnHit);

        //pet action
        if (PetManager.Pet != null) {
            //Only Pet_Dog has this function filled
            PetManager.Pet.OwnerAttacked(attacker);
        }
    }

    private void MakePetTargetAttackedEnemy(float radius) {
        if (PetManager.Pet == null) return;

        //get where player clicked
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //returns ray from cam to point
        //check if within radius range (attack range, for melee and ranged)
        if (Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position) <= radius) {
            Collider2D col = Physics2D.Raycast(ray.origin, ray.direction, 100).collider;
            if (col != null && col.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
                //make the enemy the summons' target
                PetManager.Pet.TargetEnemyAttacked(col.gameObject);
            }
        }
    }
}
