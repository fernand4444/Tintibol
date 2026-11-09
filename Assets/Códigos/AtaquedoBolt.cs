
using UnityEngine;

public class ProjetilCuspe : MonoBehaviour
{
    [Header("Configuração do Ataque")]
    public int dano = 20;
    public float tempoVida = 3f;

    private void Start()
    {
        Destroy(gameObject, tempoVida);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se atingiu um inimigo comum
        EnemyController inimigo =
            other.GetComponentInParent<EnemyController>();

        if (inimigo != null)
        {
            inimigo.TakeDamage(dano);
            Destroy(gameObject);
            return;
        }

        // Verifica se atingiu o vilão
        Vidadovilao vilao =
            other.GetComponentInParent<Vidadovilao>();

        if (vilao != null)
        {
            vilao.TakeDamage(dano);
            Destroy(gameObject);
            return;
        }
    }
}
