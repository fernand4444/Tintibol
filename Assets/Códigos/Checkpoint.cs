using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetFloat("CheckpointX", transform.position.x);
            PlayerPrefs.SetFloat("CheckpointY", transform.position.y);
            PlayerPrefs.SetInt("TemCheckpoint", 1);
            PlayerPrefs.Save();

            Debug.Log("Novo checkpoint salvo: " + gameObject.name);
        }
    }
}