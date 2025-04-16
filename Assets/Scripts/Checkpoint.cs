using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            TDMovement player = other.GetComponent<TDMovement>();
            if (player != null)
            {
                player.SetRespawnPoint(transform.position);
                Debug.Log("Checkpoint!");
            }
        }
    }
}
