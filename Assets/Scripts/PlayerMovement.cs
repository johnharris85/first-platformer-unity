using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // User a class variable to hold the player RigidBody rather than initializing in Update()
    // For performance
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    private Animator anim;
    private SpriteRenderer spr;
    private float dirX;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpSpeed = 14f;
    [SerializeField] private LayerMask jumpableGround;

    private enum MovementState { idle, running, jumping, falling};

    [SerializeField] private AudioSource jumpSound;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        // GetAxis doesn't instantly go back to 0 so you get a sliding effect
        // So we use GetAxisRaw instead
        dirX = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded()) 
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            jumpSound.Play();
        }

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState animState;
        
        if (dirX > 0f)
        {
            animState = MovementState.running;
            spr.flipX = false;
        }
        else if (dirX < 0f)
        {
            animState = MovementState.running;
            spr.flipX = true;
        }
        else
        {
            animState = MovementState.idle;
        }

        // Velocity precision is slightly off, so we don't set this to 0
        if (rb.velocity.y > .1f)
        {
            animState = MovementState.jumping;
        } 
        else if (rb.velocity.y < -.1f)
        {
            animState = MovementState.falling;
        }

        anim.SetInteger("state", (int)animState);
    }

    private bool IsGrounded() {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
