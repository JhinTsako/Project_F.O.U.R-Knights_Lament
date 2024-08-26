using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageTrigger : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;

    Animator animator;

    [SerializeField]
    private int _maxHealth;

    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
    }

    [SerializeField]
    private int _health = 100;

    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;

            if(_health <= 0)
            {
                IsAlive = false;
            }
        }
    }

    [SerializeField]
    private bool _isAlive = true;
    private bool isUnstoppable = false;
    
    private float timeSinceHit = 0;
    private float invisibilityTimer = 1f;

    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationString.isAlive, value);
            //Debug.Log("IsAlive set " + value);
        }
    }

    //Never forget to respect the velocitay...fml
    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(AnimationString.lockVelocity);
        }
        set
        {
            animator.SetBool(AnimationString.lockVelocity, value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isUnstoppable)
        {
            if(timeSinceHit > invisibilityTimer)
            {
                isUnstoppable = false;
                timeSinceHit = 0;
            }

            timeSinceHit += Time.deltaTime;
        }

        //Hit(1);
    }

    public bool Hit(int damage, Vector2 knockback)
    {
        if(IsAlive && !isUnstoppable)
        {
            Health -= damage;
            isUnstoppable = true;
            animator.SetTrigger(AnimationString.hitTrigger);
            LockVelocity = true;
            damageableHit?.Invoke(damage, knockback);
            return true;
            
        }

        return false;
    }
}
