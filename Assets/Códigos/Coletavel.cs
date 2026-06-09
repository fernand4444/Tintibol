using UnityEngine;

public class Coletavel: MonoBehaviour
{
    private void
        OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
