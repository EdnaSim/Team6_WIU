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
        ATTACK
    }
    [SerializeField] private State currentState;

    public Animator anim;
    public GameObject rangeBox;

    public AIPath aiPath;

    private bool inRange;

    // Start is called before the first frame update
    void Start()
    {
        inRange = false;
        StartCoroutine(AttackTimer());
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    private void Idle()
    {
        aiPath.canMove = false;
        anim.SetBool("IsMoving", false);
        if(inRange)
        {
            currentState = State.CHARGE;
        }
    }
    private void Charge()
    {
        aiPath.canMove = true;
        anim.SetBool("IsMoving", true);
    }
    private void Attack()
    {
        anim.SetTrigger("Attack");
        AttackTimer();
        currentState = State.IDLE;
    }

    IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(0.5f);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(ReferenceEquals(collision.gameObject, rangeBox))
        {
            currentState = State.CHARGE;
        }
    }
}
