using UnityEngine;


public class TeletransporteBuraco : MonoBehaviour
{
    public Transform destino;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = destino.position;
        }
    }
}