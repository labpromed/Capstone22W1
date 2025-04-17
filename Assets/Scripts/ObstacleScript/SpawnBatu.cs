using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBatu : MonoBehaviour
{
    [SerializeField] GameObject batuPrefab;
    [SerializeField] float launchForce = 10f;
    [SerializeField] float spawnTime = 2f;

    void Start()
    {
        InvokeRepeating(nameof(LaunchBatu), 0f, spawnTime);
    }

    void LaunchBatu()
    {
        GameObject batu = Instantiate(batuPrefab,transform.position,transform.rotation);

        Rigidbody rb = batu.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = batu.AddComponent<Rigidbody>();
        }
        rb.AddForce(transform.right * launchForce, ForceMode.Impulse);
    }
}
