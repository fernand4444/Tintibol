
using UnityEngine;

public class ItemImunidade : MonoBehaviour
{
    [Header("Escudo")]
    public GameObject escudo;

    [Header("Audio da Coleta")]
    public AudioClip somColeta;

    [Range(0f, 5f)]
    public float volumeAudio = 2f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ProtecaoJogador protecao = collision.gameObject.GetComponent<ProtecaoJogador>();

            if (protecao != null)
            {
                protecao.AtivarProtecao();
            }

            // Cria um objeto separado para reproduzir o áudio
            if (somColeta != null)
            {
                GameObject objetoAudio = new GameObject("AudioColetaEscudo");

                AudioSource audioSource = objetoAudio.AddComponent<AudioSource>();

                audioSource.clip = somColeta;
                audioSource.volume = volumeAudio;
                audioSource.spatialBlend = 0f;
                audioSource.Play();

                Destroy(objetoAudio, somColeta.length);
            }

            // Destrói o item coletado
            Destroy(gameObject);
        }
    }
}
