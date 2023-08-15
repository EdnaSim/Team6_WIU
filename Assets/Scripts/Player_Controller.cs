using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    Rigidbody2D rb;

    [Header("Movement")]
    [SerializeField] float MovementSpeed;
    float dirH;
    float dirV;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //get input for moving
        dirH = Input.GetAxis("Horizontal");
        dirV = Input.GetAxis("Vertical");
    }
    private void FixedUpdate() {//move player
        rb.AddForce(new Vector2(dirH * MovementSpeed, dirV * MovementSpeed));
    }
}
