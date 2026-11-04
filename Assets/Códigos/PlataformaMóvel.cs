using UnityEngine;


public class PlataformaMovel : MonoBehaviour
{
    [Header("Movimento")]
    public Vector2 direcao = Vector2.up;
    public float distancia = 3f;
    public float velocidade = 2f;


    private Vector3 posicaoInicial;
    private Transform jogador;
    private bool jogadorEmCima;


    void Start()
    {
        posicaoInicial = transform.position;
    }


    void LateUpdate()
    {
        float movimento = Mathf.PingPong(
            Time.time * velocidade,
            distancia
        );


        Vector3 destino = posicaoInicial +
            (Vector3)(direcao.normalized * movimento);


        Vector3 deslocamento = destino - transform.position;


        transform.position = destino;


        if (jogadorEmCima && jogador != null)
        {
            jogador.position += deslocamento;
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;


        Collider2D jogadorColisor =
            collision.collider;


        Collider2D plataformaColisor =
            GetComponent<Collider2D>();


        if (jogadorColisor == null || plataformaColisor == null)
            return;


        if (jogadorColisor.bounds.min.y >=
            plataformaColisor.bounds.max.y - 0.2f)
        {
            jogador = collision.transform;
            jogadorEmCima = true;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            jogadorEmCima = false;
            jogador = null;
        }
    }
}