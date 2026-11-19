
using UnityEngine;

public class ItemImunidade : MonoBehaviour
{
    [Header("Escudo")]
    public GameObject escudo;

    [Header("Audio da Coleta")]
    public AudioClip somColeta;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ProtecaoJogador protecao = collision.gameObject.GetComponent<ProtecaoJogador>();

            if (protecao != null)
            {
                protecao.AtivarProtecao();
            }

            // Toca o som da coleta
            if (somColeta != null)
            {
                AudioSource.PlayClipAtPoint(somColeta, transform.position);
            }

            Destroy(gameObject);
        }
    }
}
