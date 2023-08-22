using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball_Controller : MonoBehaviour
{
    public float MoveSpeed = 5f;

    Rigidbody2D rb;

    //public GameObject player;

    Vector2 moveDirection;
    private Animator anim;

    private HealthManager healthManager;

    public LayerMask targetMask;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //Physics2D.IgnoreLayerCollision(LayerMask.GetMask("Enemy"), LayerMask.GetMask("Enemy"));
        //moveDirection = (transform.position - transform.position).normalized * MoveSpeed;
        //rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate((transform.right * MoveSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            anim.Play("Fireball_Explosion");
            Player_HealthManager.Player_hm.TakeDamage(5f, gameObject);
            Destroy(gameObject, 0.6f);
        }
    }
}
