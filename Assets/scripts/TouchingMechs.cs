using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingMechs : MonoBehaviour
{
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;

    CapsuleCollider2D TouchColl;
    Animator animator;

    RaycastHit2D[] groundHits = new RaycastHit2D[5];

    [SerializeField] private bool _isGrounded;

    public bool IsGrounded { get { 
            return _isGrounded;
        } private set {
            _isGrounded = value;
            animator.SetBool(AnimationString.isGrounded, value);

        } }

    private void Awake()
    {
        TouchColl = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        IsGrounded = TouchColl.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
    }
}
