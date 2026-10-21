using UnityEngine;


public class DestruirEstaca : MonoBehaviour
{
    public string tagJogador = "Player";


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Não destruir a estaca ao tocar no jogador.
        // O Damage.cs cuidará do dano.
        if (collision.gameObject.CompareTag(tagJogador))
            return;


        // Destruir ao tocar no chão ou em uma plataforma.
        Destroy(gameObject);
    }
}