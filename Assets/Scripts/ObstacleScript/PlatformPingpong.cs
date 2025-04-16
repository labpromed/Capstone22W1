using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPingpong : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    [SerializeField] float distance = 3f;
    [SerializeField] Vector3 moveDir = Vector3.right;
    private Vector3 startPos;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveDir = moveDir.normalized;
        rb.isKinematic = true;
        startPos = transform.position;
    }

    void FixedUpdate()
    {
        float pingPong = Mathf.PingPong(Time.time * speed, distance * 2) - distance;
        Vector3 targetPos = startPos + moveDir * pingPong;
        rb.MovePosition(targetPos);
    }
}
