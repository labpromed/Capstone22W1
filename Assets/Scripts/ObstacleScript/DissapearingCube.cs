using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissapearingCube : MonoBehaviour
{
    [SerializeField] GameObject cube;
    [SerializeField] float Dissapear = 2f;
    [SerializeField] float Reappear = 2f;

    void Start()
    {
        StartCoroutine(Dissapearing());
    }

    IEnumerator Dissapearing()
    {
        while (true)
        {
            yield return new WaitForSeconds(Dissapear);
            cube.SetActive(false);

            yield return new WaitForSeconds(Reappear);
            cube.SetActive(true);
        }
    }
}
