using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;

    Vector2 dir;

    [SerializeField] float walkSpeed;
    [SerializeField] Party playerParty;
    [SerializeField] SpriteRenderer spriteRenderer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Movement();
        FlipSprite();
    }

    public void OnMove(InputValue value)
    {
        dir = value.Get<Vector2>();
        dir.Normalize();
        animator.SetBool("IsWalking", dir.magnitude > 0);
    }

    void Movement()
    {
        rb.velocity = dir * walkSpeed;
    }

    void FlipSprite()
    {
        // If moving right and sprite is facing left, flip the sprite
        if (dir.x > 0 && spriteRenderer.flipX)
        {
            spriteRenderer.flipX = false;
        }
        // If moving left and sprite is facing right, flip the sprite
        else if (dir.x < 0 && !spriteRenderer.flipX)
        {
            spriteRenderer.flipX = true;
        }
    }

    public Party GetParty()
    {
        return playerParty;
    }

}
