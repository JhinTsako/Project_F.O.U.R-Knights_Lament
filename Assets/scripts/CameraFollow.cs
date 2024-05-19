using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
      public Transform player;
    public float followSpeed = 5f;
    public float horizontalOffset = 3f;

    void FixedUpdate()
    {
        float targetX = player.position.x + (player.localScale.x * horizontalOffset);
        float currentX = transform.position.x;

        transform.Translate(Vector3.right * (targetX - currentX) * followSpeed * Time.deltaTime);
    }
}