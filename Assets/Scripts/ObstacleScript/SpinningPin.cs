using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningPin : MonoBehaviour
{
    [SerializeField] float spinSpeed = 100f;

    private Rigidbody rb;
    private Quaternion currentRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        currentRotation = rb.rotation;
    }

    void FixedUpdate()
    {
        float angle = spinSpeed * Time.fixedDeltaTime;
        Quaternion deltaRotation = Quaternion.Euler(0f, 0f, angle);
        currentRotation *= deltaRotation;
        rb.MoveRotation(currentRotation);
    }
}
