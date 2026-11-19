
using UnityEngine;

public class Coracao : MonoBehaviour
{
    public int vida = 20;

    [Header("Audio da Coleta")]
    public AudioClip somColeta;

    [Range(0f, 5f)]
    public float volumeAudio = 2f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControler player = other.GetComponent<PlayerControler>();

            if (player != null)
            {
                player.RecuperarVida(vida);

                // Cria um objeto separado para tocar o áudio
                if (somColeta != null)
                {
                    GameObject objetoAudio = new GameObject("AudioColetaCoracao");

                    AudioSource audioSource = objetoAudio.AddComponent<AudioSource>();

                    audioSource.clip = somColeta;
                    audioSource.volume = volumeAudio;
                    audioSource.spatialBlend = 0f;
                    audioSource.Play();

                    Destroy(objetoAudio, somColeta.length);
                }

                // Destrói o coração
                Destroy(gameObject);
            }
        }
    }
}
