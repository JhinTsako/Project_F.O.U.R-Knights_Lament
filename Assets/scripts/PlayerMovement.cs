using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingMechs))]
public class PlayerMovement : MonoBehaviour
{
    public float runSpeed = 5f;
    public float jumpImpulse = 10f;
    Vector2 moveInput;
    TouchingMechs touchingMechs;

    private bool _isMoving = false;
    public bool IsMoving { get 
        {
            return _isMoving;
        } private set
        {
            _isMoving = value;
            animator.SetBool(AnimationString.isMoving, value);
        }
    }
    public bool _isFacingRight = true;
    public bool IsFacingRight { get { return _isFacingRight; } private set {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }

            _isFacingRight = value;

        }
    }

    Rigidbody2D rb;
    Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingMechs = GetComponent<TouchingMechs>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * runSpeed, rb.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        IsMoving = moveInput != Vector2.zero;

        SetFacingDirection(moveInput);
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if(moveInput.x > 0 && !IsFacingRight)
            IsFacingRight = true;

        else if (moveInput.x < 0 && IsFacingRight)
            IsFacingRight = false;

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.started && touchingMechs.IsGrounded)
        {
            animator.SetTrigger(AnimationString.Jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }
}
