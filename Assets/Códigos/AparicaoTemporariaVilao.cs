
using UnityEngine;
using System.Collections;

public class AparicaoTemporariaVilao : MonoBehaviour
{
    [Header("Tempo de Aparição")]
    public float tempoInicial = 9f;
    public float tempoVisivel = 10f;

    [Header("Configuração da Transparência")]
    public float tempoDesaparecer = 0.5f;
    public float tempoAparecer = 0.5f;

    private SpriteRenderer[] sprites;
    private Collider2D[] colisores;

    private void Start()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>(true);
        colisores = GetComponentsInChildren<Collider2D>(true);

        // Começa invisível e sem colisões
        AlterarVisibilidadeImediata(0f);
        AtivarColisoes(false);

        StartCoroutine(ControlarAparicao());
    }

    private IEnumerator ControlarAparicao()
    {
        while (true)
        {
            // Aguarda o tempo inicial de cada ciclo
            yield return new WaitForSeconds(tempoInicial);

            // Faz o vilão aparecer
            yield return StartCoroutine(
                AlterarTransparencia(0f, 1f, tempoAparecer)
            );

            // Ativa as colisões
            AtivarColisoes(true);

            // Permanece visível
            yield return new WaitForSeconds(tempoVisivel);

            // Desativa as colisões
            AtivarColisoes(false);

            // Faz o vilão desaparecer
            yield return StartCoroutine(
                AlterarTransparencia(1f, 0f, tempoDesaparecer)
            );
        }
    }

    private void AlterarVisibilidadeImediata(float alpha)
    {
        foreach (SpriteRenderer sprite in sprites)
        {
            if (sprite != null)
            {
                Color cor = sprite.color;
                cor.a = alpha;
                sprite.color = cor;
            }
        }
    }

    private void AtivarColisoes(bool ativo)
    {
        foreach (Collider2D col in colisores)
        {
            if (col != null)
            {
                col.enabled = ativo;
            }
        }
    }

    private IEnumerator AlterarTransparencia(
        float inicio,
        float fim,
        float duracao)
    {
        if (duracao <= 0f)
        {
            AlterarVisibilidadeImediata(fim);
            yield break;
        }

        float tempo = 0f;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;

            float transparencia = Mathf.Lerp(
                inicio,
                fim,
                Mathf.Clamp01(tempo / duracao)
            );

            AlterarVisibilidadeImediata(transparencia);

            yield return null;
        }

        AlterarVisibilidadeImediata(fim);
    }
}
