using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using Cainos.LucidEditor;

public class Elevator : MonoBehaviour
{
    [FoldoutGroup("Params")] public Vector2 lengthRange = new Vector2(2, 5);
    [FoldoutGroup("Params")] public float moveSpeedMax = 3.0f;
    [FoldoutGroup("Params")] public float moveAcc = 10.0f;
    [FoldoutGroup("Params")] public float delayBeforeMove = 3.0f;
    [FoldoutGroup("Params")] public State startState = State.Up;

    [FoldoutGroup("Reference")] public SpriteRenderer platform;
    [FoldoutGroup("Reference")] public SpriteRenderer chainL;
    [FoldoutGroup("Reference")] public SpriteRenderer chainR;

    private Collider2D elevatorCollider;

    [FoldoutGroup("Runtime"), ShowInInspector]
    public float Length
    {
        get { return length; }
        set
        {
            if (value < 0) value = 0.0f;
            this.length = value;

            platform.transform.localPosition = new Vector3(0.0f, -value, 0.0f);
            chainL.size = new Vector2(0.09375f, value - 8 * 0.03125f);
            chainR.size = new Vector2(0.09375f, value - 8 * 0.03125f);
        }
    }
    private float length;

    [FoldoutGroup("Runtime"), ShowInInspector]
    public State CurState
    {
        get { return curState; }
        set
        {
            curState = value;
        }
    }
    private State curState;

    private float curSpeed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(StartMoveAfterDelay());
        }
    }

    private void Start()
    {
        curState = startState;
        Length = curState == State.Up ? lengthRange.y : lengthRange.x;

        elevatorCollider = GetComponent<Collider2D>();
        if (elevatorCollider)
        {
            elevatorCollider.isTrigger = true;
        }
    }

    private IEnumerator StartMoveAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeMove); 
        MoveElevator();
    }

    private void Update()
    {
        if (!IsWaiting)
        {
            Length += curSpeed * Time.deltaTime;

            if (curState == State.Up && Length <= lengthRange.x)
            {
                curState = State.Down;
                IsWaiting = true;
            }
            else if (curState == State.Down && Length >= lengthRange.y)
            {
                curState = State.Up;
                IsWaiting = true;
            }
        }
    }

    private void MoveElevator()
    {
        if (curState == State.Up)
        {
            curSpeed = -moveSpeedMax;
        }
        else if (curState == State.Down)
        {
            curSpeed = moveSpeedMax;
        }

        IsWaiting = false;
    }

    private bool IsWaiting = false;

    public enum State
    {
        Up,
        Down
    }
}
//Funny code, wish it didn't take 1 month to figure it out but hey, at least it works right? Thanks to Cainos for this script, I didn't have to modify it a lot!