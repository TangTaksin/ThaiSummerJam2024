using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;

    Vector2 dir;
    [SerializeField] float walkSpeed;

    [SerializeField] Party playerParty;
    [SerializeField] BattleInfo battleInfo;

    private void Start()
    {
        rb= GetComponent<Rigidbody2D>();
        transform.position = battleInfo.position;
    }

    private void Update()
    {
        Movement();
    }

    public void OnMove(InputValue value)
    {
        dir = value.Get<Vector2>();
        dir.Normalize();
    }

    void Movement()
    {
        rb.velocity = dir * walkSpeed;
    }    

    public Party GetParty()
    {
        return playerParty;
    }

}
