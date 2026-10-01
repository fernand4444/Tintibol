using UnityEngine;

public class Coletavel : MonoBehaviour
{
    public static int estrelasColetadas = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            estrelasColetadas++;

            Destroy(gameObject);
        }
    }
}