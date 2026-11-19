
using UnityEngine;

public class Coracao : MonoBehaviour
{
    public int vida = 20;

    [Header("Audio da Coleta")]
    public AudioClip somColeta;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControler player = other.GetComponent<PlayerControler>();

            if (player != null)
            {
                player.RecuperarVida(vida);

                // Toca o som da coleta
                if (somColeta != null)
                {
                    AudioSource.PlayClipAtPoint(somColeta, transform.position);
                }

                Destroy(gameObject);
            }
        }
    }
}
