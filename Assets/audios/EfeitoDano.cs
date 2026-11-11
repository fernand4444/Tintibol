
using UnityEngine;
using System.Collections;

public class EfeitoDano : MonoBehaviour
{
    [Header("Configurações do efeito")]
    public AudioClip somDano;
    public float tempoVermelho = 0.2f;

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private Color corOriginal;

    private Coroutine efeitoAtual;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;

        if (spriteRenderer != null)
        {
            corOriginal = spriteRenderer.color;
        }
    }

    public void AtivarEfeito()
    {
        // Toca o áudio
        if (somDano != null)
        {
            audioSource.PlayOneShot(somDano);
        }

        // Ativa o efeito vermelho
        if (spriteRenderer != null)
        {
            if (efeitoAtual != null)
            {
                StopCoroutine(efeitoAtual);
            }

            efeitoAtual = StartCoroutine(FicarVermelho());
        }
    }

    IEnumerator FicarVermelho()
    {
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(tempoVermelho);

        spriteRenderer.color = corOriginal;

        efeitoAtual = null;
    }
}
