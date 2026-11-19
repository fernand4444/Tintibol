
using UnityEngine;

public class ProtecaoJogador : MonoBehaviour
{
    private bool protegido = false;
    private GameObject escudo;

    private void Start()
    {
        // Procura o escudo dentro do jogador.
        Transform objetoEscudo = transform.Find("Escudo");

        if (objetoEscudo != null)
        {
            escudo = objetoEscudo.gameObject;
            escudo.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Escudo não encontrado dentro do jogador!");
        }
    }

    public void AtivarProtecao()
    {
        protegido = true;

        if (escudo != null)
        {
            escudo.SetActive(true);
        }

        Debug.Log("Proteção ativada!");
    }

    public bool EstaProtegido()
    {
        return protegido;
    }

    public bool UsarProtecao()
    {
        if (!protegido)
            return false;

        protegido = false;

        if (escudo != null)
        {
            escudo.SetActive(false);
        }

        Debug.Log("Escudo bloqueou o dano!");

        return true;
    }
}
