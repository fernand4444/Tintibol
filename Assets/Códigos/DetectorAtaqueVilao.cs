
using UnityEngine;
using System.Collections;

public class DetectorAtaqueVilao : MonoBehaviour
{
    [Header("Vilão")]
    public GameObject vilao;
    public Animator animacaoVilao;
    public string animacaoAtaque = "Ataque";

    [Header("Tempo")]
    public float tempoPreso = 5f;

    [Header("Dano")]
    public int dano = 20;
    public float intervaloDano = 0.5f;

    [Header("Movimentação do jogador")]
    public MonoBehaviour[] scriptsMovimento;

    private bool ativado = false;
    private bool vilaoAtivo = false;
    private bool jogadorEncostando = false;

    private GameObject jogador;
    private Coroutine rotinaDano;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || ativado)
            return;

        ativado = true;
        jogador = other.gameObject;

        StartCoroutine(Ataque());
    }

    IEnumerator Ataque()
    {
        // Prende o jogador
        foreach (MonoBehaviour script in scriptsMovimento)
        {
            if (script != null)
                script.enabled = false;
        }

        // Faz o vilão aparecer
        if (vilao != null)
            vilao.SetActive(true);

        vilaoAtivo = true;

        // Inicia a animação
        if (animacaoVilao != null)
            animacaoVilao.SetTrigger(animacaoAtaque);

        // Espera durante o ataque
        yield return new WaitForSeconds(tempoPreso);

        // Encerra o ataque
        vilaoAtivo = false;

        if (rotinaDano != null)
            StopCoroutine(rotinaDano);

        // Libera o jogador
        foreach (MonoBehaviour script in scriptsMovimento)
        {
            if (script != null)
                script.enabled = true;
        }

        // Faz o vilão desaparecer
        if (vilao != null)
            vilao.SetActive(false);

        // Desativa o detector
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D colisao)
    {
        if (vilaoAtivo && colisao.gameObject.CompareTag("Player"))
        {
            jogadorEncostando = true;

            if (rotinaDano == null)
                rotinaDano = StartCoroutine(CausarDano());
        }
    }

    private void OnCollisionExit2D(Collision2D colisao)
    {
        if (colisao.gameObject.CompareTag("Player"))
        {
            jogadorEncostando = false;
        }
    }

    IEnumerator CausarDano()
    {
        while (vilaoAtivo && jogadorEncostando)
        {
            if (jogador != null)
            {
                jogador.SendMessage(
                    "TakeDamage",
                    dano,
                    SendMessageOptions.DontRequireReceiver
                );
            }

            yield return new WaitForSeconds(intervaloDano);
        }

        rotinaDano = null;
    }
}
