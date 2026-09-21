using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetFloat("checkpointX", transform.position.x);
            PlayerPrefs.SetFloat("checkpointY", transform.position.y);
            PlayerPrefs.Save();

            Debug.Log("CHECKPOINT SALVO: " + transform.position);
        }
    }
}