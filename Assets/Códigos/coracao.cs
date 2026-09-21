using UnityEngine;

public class Coracao : MonoBehaviour
{
    public int vida = 20;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControler player = other.GetComponent<PlayerControler>();

            if (player != null)
            {
                player.RecuperarVida(vida);

                Destroy(gameObject);
            }
        }
    }
}