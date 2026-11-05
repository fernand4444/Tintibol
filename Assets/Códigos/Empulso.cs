
using UnityEngine;

public class Empulso : MonoBehaviour
{
    [Header("Impulso")]
    public Vector2 direcaoArremesso = new Vector2(1f, 1f);
    public float forcaArremesso = 15f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        Collider2D jogadorColisor = collision.collider;
        Collider2D plataformaColisor = GetComponent<Collider2D>();

        if (jogadorColisor == null || plataformaColisor == null)
            return;

        if (jogadorColisor.bounds.min.y >=
            plataformaColisor.bounds.max.y - 0.2f)
        {
            Rigidbody2D rbJogador =
                collision.gameObject.GetComponent<Rigidbody2D>();

            if (rbJogador == null)
                return;

            rbJogador.linearVelocity = Vector2.zero;

            Vector2 impulso = direcaoArremesso.normalized * forcaArremesso;

            rbJogador.AddForce(impulso, ForceMode2D.Impulse);
        }
    }
}
