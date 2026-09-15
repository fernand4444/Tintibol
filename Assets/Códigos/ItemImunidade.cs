using UnityEngine;

public class ItemImunidade : MonoBehaviour
{
    public GameObject luzProtecao;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (luzProtecao != null)
                luzProtecao.SetActive(true);

            Destroy(gameObject);
        }
    }
}