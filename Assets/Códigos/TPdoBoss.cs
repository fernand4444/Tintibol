
using UnityEngine;
using System.Collections;

public class TeletransporteVilao : MonoBehaviour
{
    [Header("Referência das Animações")]
    public GameObject furacao;
    public GameObject terceiraAnimacao;

    [Header("Tempo de Alternância")]
    public float tempoInicial = 8f;
    public float tempoVilaoVisivel = 7.5f;
    public float tempoFuracaoVisivel = 4f;
    public float tempoTerceiraAnimacao = 4f;

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
    private SpriteRenderer[] spritesTerceira;

    private Collider2D[] colisoresVilao;
    private Collider2D[] colisoresFuracao;
    private Collider2D[] colisoresTerceira;

    private void Start()
    {
        GameObject objetoJogador =
            GameObject.FindGameObjectWithTag(tagJogador);

        if (objetoJogador != null)
            jogador = objetoJogador.transform;

        if (furacao == null || terceiraAnimacao == null)
        {
            Debug.LogError("Preencha os campos das animações no Inspector!");
            enabled = false;
            return;
        }

        spritesVilao =
            GetComponentsInChildren<SpriteRenderer>(true);

        colisoresVilao =
            GetComponentsInChildren<Collider2D>(true);

        furacao.SetActive(true);
        terceiraAnimacao.SetActive(true);

        spritesFuracao =
            furacao.GetComponentsInChildren<SpriteRenderer>(true);

        spritesTerceira =
            terceiraAnimacao.GetComponentsInChildren<SpriteRenderer>(true);

        colisoresFuracao =
            furacao.GetComponentsInChildren<Collider2D>(true);

        colisoresTerceira =
            terceiraAnimacao.GetComponentsInChildren<Collider2D>(true);

        // Vilão começa visível
        AlterarVisibilidadeImediata(spritesVilao, 1f);
        AtivarColisoes(colisoresVilao, true);

        // Outras animações começam invisíveis
        AlterarVisibilidadeImediata(spritesFuracao, 0f);
        AlterarVisibilidadeImediata(spritesTerceira, 0f);

        AtivarColisoes(colisoresFuracao, false);
        AtivarColisoes(colisoresTerceira, false);

        StartCoroutine(ControlarCiclo());
    }

    private IEnumerator ControlarCiclo()
    {
        yield return new WaitForSeconds(tempoInicial);

        while (true)
        {
            // VILÃO DESAPARECE
            AtivarColisoes(colisoresVilao, false);

            yield return StartCoroutine(
                AlterarTransparencia(
                    spritesVilao, 1f, 0f, tempoDesaparecer
                )
            );

            // FURACÃO APARECE
            yield return StartCoroutine(
                AlterarTransparencia(
                    spritesFuracao, 0f, 1f, tempoAparecer
                )
            );

            AtivarColisoes(colisoresFuracao, true);

            yield return new WaitForSeconds(tempoFuracaoVisivel);

            // FURACÃO DESAPARECE
            AtivarColisoes(colisoresFuracao, false);

            yield return StartCoroutine(
                AlterarTransparencia(
                    spritesFuracao, 1f, 0f, tempoDesaparecer
                )
            );

            // TERCEIRA ANIMAÇÃO APARECE
            yield return StartCoroutine(
                AlterarTransparencia(
                    spritesTerceira, 0f, 1f, tempoAparecer
                )
            );

            AtivarColisoes(colisoresTerceira, true);

            yield return new WaitForSeconds(tempoTerceiraAnimacao);

            // TERCEIRA ANIMAÇÃO DESAPARECE
            AtivarColisoes(colisoresTerceira, false);

            yield return StartCoroutine(
                AlterarTransparencia(
                    spritesTerceira, 1f, 0f, tempoDesaparecer
                )
            );

            // VILÃO PRINCIPAL REAPARECE
            yield return StartCoroutine(
                AlterarTransparencia(
                    spritesVilao, 0f, 1f, tempoAparecer
                )
            );

            AtivarColisoes(colisoresVilao, true);

            // VILÃO PERSEGUE O JOGADOR
            yield return StartCoroutine(PeriodoVilaoAtivo());
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
                yield return StartCoroutine(ExecutarTeletransporte());
            }
        }
    }

    private IEnumerator ExecutarTeletransporte()
    {
        AtivarColisoes(colisoresVilao, false);

        yield return StartCoroutine(
            AlterarTransparencia(
                spritesVilao, 1f, 0f, tempoDesaparecer
            )
        );

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
