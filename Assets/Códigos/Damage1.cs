using UnityEngine;

public class Damage1 : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerControler player = other.gameObject.GetComponent<PlayerControler>();

            if (player != null)
            {
                player.TakeDamage(999999);
            }
        }
    }
}