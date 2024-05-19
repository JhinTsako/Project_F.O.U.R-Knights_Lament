using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 5f;
    public float horizontalOffset = 3f;
    public float verticalOffset = 3f;
    public Vector2 boundsX = new Vector2(-5f, 5f);
    public Vector2 boundsY = new Vector2(-5f, 5f);

    void FixedUpdate()
    {
        float targetX = player.position.x + (player.localScale.x * horizontalOffset);
        float targetY = player.position.y + verticalOffset;

        Vector3 currentPosition = transform.position;

        Vector3 targetPosition = new Vector3(
            Mathf.Lerp(currentPosition.x, targetX, followSpeed * Time.deltaTime),
            Mathf.Lerp(currentPosition.y, targetY, followSpeed * Time.deltaTime),
            currentPosition.z
        );

        targetPosition.x = Mathf.Clamp(targetPosition.x, boundsX.x, boundsX.y);
        targetPosition.y = Mathf.Clamp(targetPosition.y, boundsY.x, boundsY.y);

        transform.position = targetPosition;
    }
}
