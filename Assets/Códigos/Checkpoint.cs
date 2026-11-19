
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool ativado = false;
    private AudioSource audioSource;

    [Header("Cores do Checkpoint")]
    public SpriteRenderer imagem;
    public Color corInicial = Color.white;
    public Color corAtivada = new Color(1f, 0.4f, 0.7f, 1f);

    [Header("Áudio do Checkpoint")]
    public AudioClip somCheckpoint;

    [Range(0f, 5f)]
    public float volumeAudio = 2f;

    private void Awake()
    {
        // Cria ou procura o AudioSource
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configura o áudio para ser 2D
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
        audioSource.loop = false;
    }

    private void Start()
    {
        if (imagem == null)
        {
            imagem = GetComponent<SpriteRenderer>();
        }

        if (imagem != null)
        {
            imagem.color = corInicial;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ativado)
            return;

        if (other.CompareTag("Player"))
        {
            CheckpointManager.SalvarCheckpoint(transform.position);

            ativado = true;

            if (imagem != null)
            {
                imagem.color = corAtivada;
            }

            // Reproduz o áudio em 2D
            if (somCheckpoint != null)
            {
                audioSource.PlayOneShot(somCheckpoint, volumeAudio);
            }

            Debug.Log("Checkpoint ativado!");
        }
    }
}
