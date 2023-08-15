using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile details")]
    public float speed;
    public float damage;
    public GameObject caster;
    public Vector2 CastPos;
    public LayerMask targetLayer;
    public Vector2 dir;
    public float MaxRange;
    private float currDistFromCaster;
    bool hashit = false;
    Coroutine DestroySelfCoroutine;

    [Header("Damage fall off")]
    public float FalloffDist;
    float FalloffMult;

    private Rigidbody2D rb;
    private Animator ar;
    private TrailRenderer tr;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ar = GetComponentInChildren<Animator>();
        tr = GetComponent<TrailRenderer>();
        gameObject.layer = caster.layer;
        Physics2D.IgnoreLayerCollision(caster.layer, caster.layer);
        gameObject.SetActive(false);
    }

    private void Update() {
        if (!gameObject.activeSelf)
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
            if (DestroySelfCoroutine != null)
                StopCoroutine(DestroySelfCoroutine); //prevent the coroutine from going off while the object is active
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (hashit)
            return;

        //does targetLayer contain col.layer?
        if ((targetLayer & (1 << col.gameObject.layer)) == 0) {
            return;
        }
        if (ar != null)
            ar.SetTrigger("Hit");
        hashit = true;
        rb.velocity = Vector2.zero;
        HealthManager hm = col.gameObject.GetComponent<HealthManager>();
        if (hm != null)
            hm.TakeDamage(damage * FalloffMult, gameObject);
        //if no anim to play
        if (ar == null)
            gameObject.SetActive(false);
    }

    public void OnHitAnimEnded() {
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }

    public void ReSummon() {
        //set active has to be called first for the other variables to be set
        gameObject.SetActive(true);
        hashit = false;
        transform.position = caster.transform.position;
        CastPos = caster.transform.position;
        rb.velocity = dir * speed;
        currDistFromCaster = 0;
        FalloffMult = 1f;
        DestroySelfCoroutine = null;
        if (tr != null)
            tr.Clear();
        //rotate sprite
        float angleToFace = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        GetComponentInChildren<SpriteRenderer>().transform.rotation = Quaternion.AngleAxis(angleToFace, Vector3.forward);

        DestroySelfCoroutine = StartCoroutine(DisableAfterTime());
    }

    IEnumerator DisableAfterTime() {
        yield return new WaitForSeconds(10);
        gameObject.SetActive(false);
    }
}
