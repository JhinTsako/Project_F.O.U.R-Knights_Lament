using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingMechs))]
public class PlayerMovement : MonoBehaviour
{
    public float runSpeed = 5f;
    public float walkSpeed = 2f;
    public float airWalkSpeed = 3f;
    public float jumpImpulse = 10f;
    Vector2 moveInput;
    TouchingMechs touchingMechs;

    public float CurrentMoveSpeed {  get
        {
            if (CanMove)
            {
                if (IsMoving)
                {
                    if (touchingMechs.IsGrounded)
                    {
                        if (IsWalking)
                        {
                            return walkSpeed;
                        }
                        else
                        {
                            return runSpeed;
                        }
                    }
                    else
                    {
                        // Air speed
                        return airWalkSpeed;
                    }
                }
                else
                {
                    //idle speed is 0
                    return 0;
                }
            }
            else
            {
                //Movement lock
                return 0;
            }

        } }





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
    private bool _isWalking = false;

    public bool IsWalking
    {
        get
        {
            return _isWalking;
        }
        set
        {
            _isWalking = value;
            animator.SetBool(AnimationString.isWalking, value);
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
    public bool CanMove { get
        {
            return animator.GetBool(AnimationString.canMove);
        } }

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
        rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        IsMoving = moveInput != Vector2.zero;

        SetFacingDirection(moveInput);
    }

    public void OnWalk(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsWalking = true;
        }
        else if (context.canceled)
        {
            IsWalking = false;
        }
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
        if(context.started && touchingMechs.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationString.Jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.started && touchingMechs.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationString.attackTrigger);
        }
    }
}
