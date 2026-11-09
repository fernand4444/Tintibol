using UnityEngine;


public class FisicaAgua : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerControler jogador =
            other.GetComponent<PlayerControler>();


        if (jogador != null)
        {
            jogador.EntrarNaAgua();
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerControler jogador =
            other.GetComponent<PlayerControler>();


        if (jogador != null)
        {
            jogador.SairDaAgua();
        }
    }
}