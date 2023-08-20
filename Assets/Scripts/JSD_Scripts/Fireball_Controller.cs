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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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
            Debug.Log("Hit");
            anim.Play("Fireball_Explosion");
            //rb.velocity = new Vector2(moveDirection.x * 0, moveDirection.y * 0);
            Destroy(gameObject, 0.6f);
        }
    }
}
