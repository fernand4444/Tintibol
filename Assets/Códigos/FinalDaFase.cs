using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalDaFase : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("fase 2");
        }
    }
}