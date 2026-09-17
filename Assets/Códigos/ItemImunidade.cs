using UnityEngine;

public class ItemImunidade : MonoBehaviour
{
    public GameObject escudo;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ProtecaoJogador protecao = collision.gameObject.GetComponent<ProtecaoJogador>();

            if (protecao != null)
            {
                protecao.AtivarProtecao();
            }

            Destroy(gameObject);
        }
    }
}