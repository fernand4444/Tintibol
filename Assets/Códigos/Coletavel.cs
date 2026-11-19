
using UnityEngine;

public class Coletavel : MonoBehaviour
{
    public static int estrelasColetadas = 0;

    [Header("Áudio da Coleta")]
    public AudioClip somColeta;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            estrelasColetadas++;

            // Toca o som ao coletar a estrela.
            if (somColeta != null)
            {
                AudioSource.PlayClipAtPoint(somColeta, transform.position);
            }

            Destroy(gameObject);

            AudioSource.PlayClipAtPoint(somColeta, transform.position);
        }
    }
}
