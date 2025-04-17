using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatuHilang : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DestroyRock"))
        {
            Destroy(gameObject);
        }
    }
}
