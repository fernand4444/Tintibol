
using UnityEngine;
using System.Collections;

public class TeletransporteVilao : MonoBehaviour
{
    [Header("Configuração do Teletransporte")]
    public float intervalo = 10f;

    [Header("Distância do Jogador")]
    public float distanciaLateral = 2f;

    [Header("Configuração da Transparência")]
    public float tempoDesaparecer = 0.5f;
    public float tempoAparecer = 0.5f;

    [Header("Referência do Jogador")]
    public string tagJogador = "Player";

    private Transform jogador;

    private float contador;
    private bool teletransportando = false;

    private SpriteRenderer[] sprites;

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
            Debug.LogWarning("Jogador com a tag Player não encontrado!");
        }

        sprites = GetComponentsInChildren<SpriteRenderer>();

        contador = intervalo;
    }

    private void Update()
    {
        if (teletransportando || jogador == null)
            return;

        contador -= Time.deltaTime;

        if (contador <= 0)
        {
            StartCoroutine(ExecutarTeletransporte());
        }
    }

    private IEnumerator ExecutarTeletransporte()
    {
        teletransportando = true;

        // Desaparece
        yield return StartCoroutine(
            AlterarTransparencia(1f, 0f, tempoDesaparecer)
        );

        // Escolhe aleatoriamente um dos lados do jogador
        float lado = Random.value < 0.5f ? -1f : 1f;

        Vector3 destino = jogador.position;

        destino.x += lado * distanciaLateral;

        // Teletransporta para o lado do jogador
        transform.position = destino;

        // Aparece novamente
        yield return StartCoroutine(
            AlterarTransparencia(0f, 1f, tempoAparecer)
        );

        contador = intervalo;
        teletransportando = false;
    }

    private IEnumerator AlterarTransparencia(
        float inicio,
        float fim,
        float duracao)
    {
        float tempo = 0f;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;

            float transparencia = Mathf.Lerp(
                inicio,
                fim,
                Mathf.Clamp01(tempo / duracao)
            );

            foreach (SpriteRenderer sprite in sprites)
            {
                if (sprite != null)
                {
                    Color cor = sprite.color;
                    cor.a = transparencia;
                    sprite.color = cor;
                }
            }

            yield return null;
        }

        foreach (SpriteRenderer sprite in sprites)
        {
            if (sprite != null)
            {
                Color cor = sprite.color;
                cor.a = fim;
                sprite.color = cor;
            }
        }
    }
}
