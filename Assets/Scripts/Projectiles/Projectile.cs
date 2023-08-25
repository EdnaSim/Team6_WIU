using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public WeaponController LinkedWeapon = null;

    [Header("Set by Weapon")]
    public float speed;
    public float damage;
    public GameObject caster;
    public LayerMask targetLayer;
    public float MaxRange;
    public float FalloffDist;

    [Header("Dont touch")]
    public Vector2 CastPos;
    public Vector2 dir;
    private float currDistFromCaster;
    protected bool hashit = false;
    protected float FalloffMult;
    public float AOE;
    public float AOEDamage;
    public bool DoesBulletDoDamage;

    protected Rigidbody2D rb;
    protected Animator ar;
    protected TrailRenderer tr;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ar = GetComponentInChildren<Animator>();
        tr = GetComponent<TrailRenderer>();
        gameObject.layer = caster.layer;
        Physics2D.IgnoreLayerCollision(caster.layer, caster.layer);
        //gameObject.SetActive(false);
        PrepareToFire();
    }

    private void Update() {
        if (hashit)
            return;

        //Damage fall off when the proj is at a range from the cast pos
        currDistFromCaster = Vector2.Distance(CastPos, gameObject.transform.position);
        if (currDistFromCaster >= FalloffDist) {
            //Start fall off
            FalloffMult = (MaxRange - currDistFromCaster) / FalloffDist;
            if (FalloffMult < 0.1f)
                FalloffMult = 0.1f;
        }
        if (currDistFromCaster >= MaxRange) {
            //gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (hashit)
            return;

        //does targetLayer contain col.layer?
        if ((targetLayer & (1 << col.gameObject.layer)) == 0) {
            return; //no
        }
        if (ar != null)
            ar.SetTrigger("Hit");

        hashit = true;
        rb.velocity = Vector2.zero;
        if (AOE <= 0 || (AOE > 0 && DoesBulletDoDamage)) {
            HealthManager hm = col.gameObject.GetComponent<HealthManager>();
            if (hm != null)
                hm.TakeDamage(damage * FalloffMult, gameObject);
        }
        if(AOE > 0) {
            //damage all enemies in AOE
            foreach (Collider2D c in Physics2D.OverlapCircleAll(transform.position, AOE)) {
                HealthManager hm = c.gameObject.GetComponent<HealthManager>();
                if (hm != null) {
                    hm.TakeDamage(AOEDamage, caster);
                }
            }
        }
        //if no anim to play
        if (ar == null)
            Destroy(gameObject);
            //gameObject.SetActive(false);
    }

    public void OnHitAnimEnded() {
        Destroy(gameObject);
        //gameObject.SetActive(false);
    }

    public void PrepareToFire() {
        //set active has to be called first for the other variables to be set
        //gameObject.SetActive(true);
        hashit = false;
        transform.position = caster.transform.position;
        CastPos = caster.transform.position;
        rb.velocity = dir * speed;
        currDistFromCaster = 0;
        FalloffMult = 1f;
        if (tr != null)
            tr.Clear();
        //rotate sprite
        float angleToFace = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        GetComponentInChildren<SpriteRenderer>().transform.rotation = Quaternion.AngleAxis(angleToFace, Vector3.forward);
    }

    IEnumerator DisableAfterTime() {
        yield return new WaitForSeconds(10);
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
