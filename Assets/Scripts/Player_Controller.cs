using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player_Controller : MonoBehaviour
{
    public static Player_Controller PC;

    Rigidbody2D rb;
    Animator ar;
    SpriteRenderer sr;
    [HideInInspector] public WeaponController wc;
    [SerializeField] SO_WeaponList wl;

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
    [SerializeField] Transform MeleeOrigin;
    public static List<MeleeWeaponStats> TempMeleeInv; // TEMP
    bool CanMelee = true;

    [Header("Ranged attack")]
    public static List<RangedWeaponStats> TempInventory; //TEMP
    int weaponListIndex = 0; //TEMP
    bool CanRange = true;

    [Header("Pet")]
    [SerializeField] SO_PetDetails PetDetails;

    // Start is called before the first frame update
    void Start()
    {
        PC = this;

        rb = GetComponent<Rigidbody2D>();
        ar = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        MovementSpeed = Base_MovementSpeed;

        wc = GetComponent<WeaponController>();

        //TEMP - add weapon instance for each type
        for (int i = 0; i < wl.RangedWeaponList.Count; i++) {
            TempInventory.Add(new RangedWeaponStats(wl.RangedWeaponList[i].Stats));
        }
        for (int i=0; i < wl.MeleeWeaponlist.Count; i++) {
            TempMeleeInv.Add(new MeleeWeaponStats(wl.MeleeWeaponlist[i].Stats));
        }

        Physics2D.IgnoreLayerCollision(gameObject.layer, gameObject.layer);
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
                
                //TEMP - Find weapon name to switch to it
                //wc.ChangeRangedWeapon(TempInventory.Find((RangedWeaponStats w) => w.WeaponName == "Triangle"));
                wc.ChangeRangedWeapon(TempInventory[weaponListIndex]);
                Debug.Log("Current Weapon: " + TempInventory[weaponListIndex].WeaponName);
                weaponListIndex++;
                if (weaponListIndex >= TempInventory.Count) weaponListIndex = 0;

                if (wc.MeleeStats.WeaponName == "Fists")
                    wc.ChangeMeleeWeapon(TempMeleeInv.Find((MeleeWeaponStats w) => w.WeaponName == "Sword"));
                else
                    wc.ChangeMeleeWeapon(TempMeleeInv.Find((MeleeWeaponStats w) => w.WeaponName == "Fists"));
            }
            if (Input.GetMouseButtonDown(0)) {
                //Attack (left click)
                if (EventSystem.current.IsPointerOverGameObject())
                    return; //if clicked UI

                if (CanMelee && !isAttacking) {
                    CanMelee = false;
                    //isAttacking = true;
                    //ar.SetTrigger("Melee");
                    StartMelee();
                    StartCoroutine(StartMeleeCooldown());

                    //make pet target enemy "attacked"
                    //using radiusX since its the height from the player to the tip of the box
                    MakePetTargetAttackedEnemy(wc.MeleeStats.RadiusX + 1);
                }
            }
            if (Input.GetMouseButtonDown(1)) {
                //Ranged attack (right click)
                if (CanRange && !isAttacking) {
                    CanRange = false;
                    //isAttacking = true;
                    StartRanged();
                    StartCoroutine(StartRangedCooldown());

                    //make pet target enemy "attacked"
                    MakePetTargetAttackedEnemy(wc.RangedStats.MaxRange);
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
    //TEMP - draw melee range (also rotate it)
    //private void OnDrawGizmos() {
    //    //Gizmos.DrawWireCube(MeleeOrigin.transform.position, MeleeRange);
    //    var oldMatrix = Gizmos.matrix;

    //    // create a matrix which translates an object by "position", rotates it by "rotation" and scales it by "halfExtends * 2"
    //    Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(facing.y, facing.x) * Mathf.Rad2Deg);
    //    Gizmos.matrix = Matrix4x4.TRS(MeleeOrigin.position, rotation, new Vector2(wc.MeleeStats.RadiusX, wc.MeleeStats.RadiusY));
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
    private void StartMelee() {
        //AOE
        wc.Melee(MeleeOrigin.position, facing);
    }
    IEnumerator StartMeleeCooldown() {
        yield return new WaitForSeconds(wc.MeleeStats.Cooldown);
        CanMelee = true;
    }

    private void StartRanged() {
        wc.Fire(facing);
    }
    IEnumerator StartRangedCooldown() {
        yield return new WaitForSeconds(wc.RangedStats.FireRate);
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

    public void OnHitByEnemy(GameObject attacker) {
        //drain sanity

        //pet action
        if (PetManager.Pet != null) {
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
