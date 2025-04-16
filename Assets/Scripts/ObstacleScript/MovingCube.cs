using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCube : MonoBehaviour
{
    [SerializeField] float moveDistance = 3f;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float pauseTime = 1f;
    [SerializeField] Vector3 moveDir = Vector3.right;

    private Rigidbody rb;
    private Vector3 startPos;
    private Vector3 targetPos;
    private bool moving = true;
    private float waitTimer = 0f;
    private bool isWaiting = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        moveDir = moveDir.normalized;
        startPos = transform.position;
        targetPos = startPos + moveDir * moveDistance;
    }

    void FixedUpdate()
    {
        if (isWaiting)
        {
            waitTimer += Time.fixedDeltaTime;
            if (waitTimer >= pauseTime)
            {
                isWaiting = false;
                waitTimer = 0f;
                moving = !moving;
            }
            return;
        }

        Vector3 destination = moving ? targetPos : startPos;
        Vector3 direction = (destination - rb.position).normalized;
        float step = moveSpeed * Time.deltaTime;

        if (Vector3.Distance(rb.position, destination) < step)
        {
            rb.MovePosition(destination);
            isWaiting = true;
        }
        else
        {
            rb.MovePosition(rb.position + direction * step);
        }
    }
}
