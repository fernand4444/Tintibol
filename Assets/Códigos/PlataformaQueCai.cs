
using UnityEngine;
using System.Collections;

public class PlataformaQueCai : MonoBehaviour
{
    public float tempoParaCair = 2f;
    public float tempoTremendo = 0.5f;
    public float velocidadeQueda = 8f;
    public float tempoParaVoltar = 3f;

    private bool jogadorEmCima = false;
    private bool vaiCair = false;
    private bool voltou = false;

    private Vector3 posicaoInicial;
    private Rigidbody2D rb;

    void Start()
    {
        posicaoInicial = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D colisao)
    {
        if (colisao.gameObject.CompareTag("Player"))
        {
            jogadorEmCima = true;

            if (!vaiCair)
                StartCoroutine(EsperarECair());
        }

        if (vaiCair && !voltou &&
            colisao.gameObject.CompareTag("chao"))
        {
            voltou = true;
            StartCoroutine(VoltarParaPosicaoInicial());
        }
    }

    void OnCollisionExit2D(Collision2D colisao)
    {
        if (colisao.gameObject.CompareTag("Player"))
        {
            jogadorEmCima = false;
        }
    }

    IEnumerator EsperarECair()
    {
        yield return new WaitForSeconds(tempoParaCair);

        if (!jogadorEmCima || vaiCair)
            yield break;

        vaiCair = true;

        yield return new WaitForSeconds(tempoTremendo);

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 3f;
            rb.constraints = RigidbodyConstraints2D.None;
        }
    }

    IEnumerator VoltarParaPosicaoInicial()
    {
        yield return new WaitForSeconds(tempoParaVoltar);

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        transform.position = posicaoInicial;

        jogadorEmCima = false;
        vaiCair = false;
        voltou = false;
    }
}
