
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
        // Verifica se atingiu um inimigo
        EnemyController inimigo =
            other.GetComponentInParent<EnemyController>();

        if (inimigo != null)
        {
            // Aplica o dano configurado no projétil
            inimigo.TakeDamage(dano);

            // Destrói o projétil após o impacto
            Destroy(gameObject);
        }
    }
}
