
using UnityEngine;
using System.Collections;

public class AtaqueChaoEletrico : MonoBehaviour
{
    [Header("Configuração do Ataque")]
    public float intervalo = 15f;
    public float tempoPiscando = 2f;
    public float velocidadePiscada = 0.15f;

    [Header("Chão")]
    public ChaoEletrico chao;

    private SpriteRenderer[] sprites;

    private void Start()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();

        StartCoroutine(CicloAtaque());
    }

    private IEnumerator CicloAtaque()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervalo);

            // Vilão começa a piscar
            float tempo = 0f;

            while (tempo < tempoPiscando)
            {
                foreach (SpriteRenderer sprite in sprites)
                {
                    if (sprite != null)
                        sprite.enabled = !sprite.enabled;
                }

                yield return new WaitForSeconds(velocidadePiscada);

                tempo += velocidadePiscada;
            }

            // Garante que o vilão volte a aparecer
            foreach (SpriteRenderer sprite in sprites)
            {
                if (sprite != null)
                    sprite.enabled = true;
            }

            // Ativa o chão perigoso
            if (chao != null)
                chao.AtivarDano();
        }
    }
}
