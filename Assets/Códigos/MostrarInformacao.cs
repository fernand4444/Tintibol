
using UnityEngine;

public class MostrarInformacao : MonoBehaviour
{
    public GameObject telaInformacao;

    [Header("Audio da Informacao")]
    public AudioClip somInformacao;

    [Range(0f, 5f)]
    public float volumeAudio = 2f;

    private AudioSource audioSource;
    private bool jaMostrou = false;

    private void Awake()
    {
        // Procura ou cria um AudioSource
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configura o áudio como 2D
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
        audioSource.loop = false;
        audioSource.ignoreListenerPause = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (jaMostrou)
            return;

        if (other.CompareTag("Player"))
        {
            if (telaInformacao != null)
            {
                telaInformacao.SetActive(true);

                // Toca o som antes de pausar o jogo
                if (somInformacao != null)
                {
                    audioSource.PlayOneShot(
                        somInformacao,
                        volumeAudio
                    );
                }

                Time.timeScale = 0f;

                jaMostrou = true;

                Debug.Log("Jogo pausado e tela aberta!");
            }
        }
    }
}
