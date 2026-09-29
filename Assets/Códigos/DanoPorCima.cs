using UnityEngine;

public class DanoPorCima : MonoBehaviour
{
    public int dano = 25;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EnemyController inimigo = GetComponentInParent<EnemyController>();

            if (inimigo != null)
            {
                inimigo.TakeDamage(dano);
            }
        }
    }
}