
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
        bool vermelho = false;

        while (tempo < duracaoDano)
        {
            // Faz o chão alternar entre a cor original e vermelho
            vermelho = !vermelho;

            if (sprite != null)
                sprite.color = vermelho ? corPerigosa : corOriginal;

            // Causa dano ao jogador
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

            yield return new WaitForSeconds(intervaloDano);
            tempo += intervaloDano;
        }

        // Volta à cor original
        if (sprite != null)
            sprite.color = corOriginal;

        ativo = false;
    }
}
