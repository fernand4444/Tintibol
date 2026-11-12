
using UnityEngine;
using System.Collections;

public class ChaoEletrico : MonoBehaviour
{
    [Header("Configuração do Dano")]
    public int dano = 20;
    public float duracaoDano = 4f;
    public float intervaloDano = 1f;

    [Header("Efeito de Piscar")]
    public Color corPerigosa = Color.red;
    public float velocidadePiscada = 0.2f;

    private SpriteRenderer sprite;
    private Color corOriginal;
    private bool ativo = false;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        if (sprite != null)
            corOriginal = sprite.color;
    }

    public void AtivarDano()
    {
        if (!ativo)
            StartCoroutine(EfeitoEletrico());
    }

    private IEnumerator EfeitoEletrico()
    {
        ativo = true;

        float tempo = 0f;
        float tempoPiscada = 0f;
        float tempoDano = 0f;
        bool vermelho = false;

        while (tempo < duracaoDano)
        {
            tempo += Time.deltaTime;
            tempoPiscada += Time.deltaTime;
            tempoDano += Time.deltaTime;

            // Pisca o chão
            if (tempoPiscada >= velocidadePiscada)
            {
                tempoPiscada = 0f;
                vermelho = !vermelho;

                if (sprite != null)
                    sprite.color = vermelho ? corPerigosa : corOriginal;
            }

            // Aplica dano no jogador
            if (tempoDano >= intervaloDano)
            {
                tempoDano = 0f;

                Collider2D colisor = GetComponent<Collider2D>();

                if (colisor != null)
                {
                    Collider2D[] jogadores = Physics2D.OverlapBoxAll(
                        colisor.bounds.center,
                        colisor.bounds.size,
                        0f
                    );

                    foreach (Collider2D col in jogadores)
                    {
                        PlayerControler player =
                            col.GetComponentInParent<PlayerControler>();

                        if (player != null && player.CompareTag("Player"))
                            player.TakeDamage(dano);
                    }
                }
            }

            yield return null;
        }

        // Para a piscada e restaura a cor original
        if (sprite != null)
            sprite.color = corOriginal;

        ativo = false;
    }
}
