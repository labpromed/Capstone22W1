using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDCamera : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float distance = 5f;
    [SerializeField] float height = 2f;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] float minYAngle = -35f;
    [SerializeField] float maxYAngle = 60f;
    [SerializeField] float SmoothFollow = 10f;

    private float currentYaw = 0f;
    private float currentPitch = 0f;

    void LateUpdate()
    {
        if (!target) return;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        currentYaw += mouseX * rotationSpeed;
        currentPitch -= mouseY * rotationSpeed;
        currentPitch = Mathf.Clamp(currentPitch, minYAngle, maxYAngle);

        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);

        Vector3 targetPos = target.position + Vector3.up * height;
        Vector3 desiredPosition = targetPos - rotation * Vector3.forward * distance;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, SmoothFollow * Time.deltaTime);
        transform.LookAt(targetPos);
    }
}