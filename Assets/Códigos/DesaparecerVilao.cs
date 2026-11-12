
using System.Collections;
using UnityEngine;

public class DesaparecerVilao : MonoBehaviour
{
    public float tempoInicial = 8f;
    public float tempoDesaparecido = 5f;

    private SpriteRenderer[] sprites;
    private Collider2D[] colisores;

    void Start()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();
        colisores = GetComponentsInChildren<Collider2D>();

        StartCoroutine(ExecutarDesaparecimento());
    }

    IEnumerator ExecutarDesaparecimento()
    {
        // Aguarda 8 segundos após o início do jogo
        yield return new WaitForSeconds(tempoInicial);

        // Vilão desaparece
        foreach (SpriteRenderer sprite in sprites)
            sprite.enabled = false;

        foreach (Collider2D col in colisores)
            col.enabled = false;

        // Aguarda 5 segundos desaparecido
        yield return new WaitForSeconds(tempoDesaparecido);

        // Vilão reaparece
        foreach (SpriteRenderer sprite in sprites)
            sprite.enabled = true;

        foreach (Collider2D col in colisores)
            col.enabled = true;
    }
}
