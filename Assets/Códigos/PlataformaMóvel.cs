using UnityEngine;

public class PlataformaMovel : MonoBehaviour
{
    public Vector2 pontoDestino;
    public float velocidade = 2f;
    private Vector2 posicaoInicial;
    private bool voltando;

    void Start()
    {
        posicaoInicial = transform.position;
    }

    void Update()
    {
        Vector2 alvo = voltando ? posicaoInicial : posicaoInicial + pontoDestino;

        transform.position = Vector2.MoveTowards(
            transform.position,
            alvo,
            velocidade * Time.deltaTime
        );

        if ((Vector2)transform.position == alvo)
        {
            voltando = !voltando;
        }
    }
}