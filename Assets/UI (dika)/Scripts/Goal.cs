using UnityEngine;

public class Goal : MonoBehaviour
{
    public GameManager gameManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            FindObjectOfType<GameManager>().WinGame();

        }
    }

}
