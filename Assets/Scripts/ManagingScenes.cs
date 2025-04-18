using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagingScenes : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("InGameUI", LoadSceneMode.Additive);
    }
}
