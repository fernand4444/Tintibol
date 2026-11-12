
using UnityEngine;
using System.Collections;

public class TeletransporteVilao : MonoBehaviour
{
    [Header("Referência do Furacão")]
    public GameObject furacao;

    [Header("Tempo de Alternância")]
    public float tempoInicial = 8f;
    public float tempoVilaoVisivel = 7.5f;
    public float tempoFuracaoVisivel = 4f;

    [Header("Configuração do Teletransporte")]
    public float intervalo = 4f;
    public float distanciaLateral = 2f;

    [Header("Configuração da Transparência")]
    public float tempoDesaparecer = 0.5f;
    public float tempoAparecer = 0.5f;

    [Header("Referência do Jogador")]
    public string tagJogador = "Player";

    private Transform jogador;

    private SpriteRenderer[] spritesVilao;
    private SpriteRenderer[] spritesFuracao;

    private Collider2D[] colisoresVilao;
    private Collider2D[] colisoresFuracao;

    private bool teletransportando = false;

    private void Start()
    {
        GameObject objetoJogador =
            GameObject.FindGameObjectWithTag(tagJogador);

        if (objetoJogador != null)
        {
            jogador = objetoJogador.transform;
        }
        else
        {
            Debug.LogWarning("Jogador não encontrado!");
        }

        if (furacao == null)
        {
            Debug.LogError("Arraste o objeto do furacão para o Inspector!");
            enabled = false;
            return;
        }

        spritesVilao =
            GetComponentsInChildren<SpriteRenderer>(true);

        colisoresVilao =
            GetComponentsInChildren<Collider2D>(true);

        furacao.SetActive(true);

        spritesFuracao =
            furacao.GetComponentsInChildren<SpriteRenderer>(true);

        colisoresFuracao =
            furacao.GetComponentsInChildren<Collider2D>(true);

        // Vilão começa visível
        AlterarVisibilidadeImediata(spritesVilao, 1f);
        AtivarColisoes(colisoresVilao, true);

        // Furacão começa invisível
        AlterarVisibilidadeImediata(spritesFuracao, 0f);
        AtivarColisoes(colisoresFuracao, false);

        StartCoroutine(ControlarCiclo());
    }

    private IEnumerator ControlarCiclo()
    {
        // Primeira aparição do vilão
        yield return new WaitForSeconds(tempoInicial);

        while (true)
        {
            // Faz o vilão desaparecer
            AtivarColisoes(colisoresVilao, false);

            yield return StartCoroutine(
                AlterarTransparencia(
                    spritesVilao, 1f, 0f, tempoDesaparecer
                )
            );

            // Faz o furacão aparecer
            yield return StartCoroutine(
                AlterarTransparencia(
                    spritesFuracao, 0f, 1f, tempoAparecer
                )
            );

            AtivarColisoes(colisoresFuracao, true);

            // Furacão permanece ativo
            yield return new WaitForSeconds(tempoFuracaoVisivel);

            // Furacão desaparece
            AtivarColisoes(colisoresFuracao, false);

            yield return StartCoroutine(
                AlterarTransparencia(
                    spritesFuracao, 1f, 0f, tempoDesaparecer
                )
            );

            // Vilão principal reaparece
            yield return StartCoroutine(
                AlterarTransparencia(
                    spritesVilao, 0f, 1f, tempoAparecer
                )
            );

            AtivarColisoes(colisoresVilao, true);

            // Vilão continua perseguindo o jogador
            yield return StartCoroutine(
                PeriodoVilaoAtivo()
            );
        }
    }

    private IEnumerator PeriodoVilaoAtivo()
    {
        float tempo = 0f;

        while (tempo < tempoVilaoVisivel)
        {
            float espera = Mathf.Min(
                intervalo,
                tempoVilaoVisivel - tempo
            );

            yield return new WaitForSeconds(espera);

            tempo += espera;

            if (tempo < tempoVilaoVisivel && jogador != null)
            {
                yield return StartCoroutine(
                    ExecutarTeletransporte()
                );
            }
        }
    }

    private IEnumerator ExecutarTeletransporte()
    {
        if (teletransportando)
            yield break;

        teletransportando = true;

        AtivarColisoes(colisoresVilao, false);

        yield return StartCoroutine(
            AlterarTransparencia(
                spritesVilao, 1f, 0f, tempoDesaparecer
            )
        );

        // Escolhe um lado aleatório do jogador
        float lado = Random.value < 0.5f ? -1f : 1f;

        Vector3 destino = jogador.position;

        destino.x += lado * distanciaLateral;

        transform.position = destino;

        yield return StartCoroutine(
            AlterarTransparencia(
                spritesVilao, 0f, 1f, tempoAparecer
            )
        );

        AtivarColisoes(colisoresVilao, true);

        teletransportando = false;
    }

    private void AtivarColisoes(
        Collider2D[] lista,
        bool ativo)
    {
        foreach (Collider2D col in lista)
        {
            if (col != null)
                col.enabled = ativo;
        }
    }

    private void AlterarVisibilidadeImediata(
        SpriteRenderer[] lista,
        float alpha)
    {
        foreach (SpriteRenderer sprite in lista)
        {
            if (sprite != null)
            {
                Color cor = sprite.color;
                cor.a = alpha;
                sprite.color = cor;
            }
        }
    }

    private IEnumerator AlterarTransparencia(
        SpriteRenderer[] lista,
        float inicio,
        float fim,
        float duracao)
    {
        if (duracao <= 0f)
        {
            AlterarVisibilidadeImediata(lista, fim);
            yield break;
        }

        float tempo = 0f;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;

            float alpha = Mathf.Lerp(
                inicio,
                fim,
                Mathf.Clamp01(tempo / duracao)
            );

            AlterarVisibilidadeImediata(lista, alpha);

            yield return null;
        }

        AlterarVisibilidadeImediata(lista, fim);
    }
}
