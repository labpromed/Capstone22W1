using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShadow : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float rayDistance = 10f;
    [SerializeField] float shadowOffset = 0.02f;

    void Update()
    {
        RaycastHit hit;
        Vector3 origin = player.position; //+ Vector3.down;

        if (Physics.Raycast(origin, Vector3.down, out hit, rayDistance))
        {
            transform.position = hit.point + Vector3.up * shadowOffset;
        }
    }
}
