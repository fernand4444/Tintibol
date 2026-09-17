using UnityEngine;

public class ProtecaoJogador : MonoBehaviour
{
    private bool protegido = false;
    private GameObject escudo;

    private void Start()
    {
        // Procura o escudo dentro do Player
        Transform objetoEscudo = transform.Find("Escudo");

        if (objetoEscudo != null)
        {
            escudo = objetoEscudo.gameObject;
            escudo.SetActive(false);
        }
    }

    public void AtivarProtecao()
    {
        protegido = true;

        if (escudo != null)
        {
            escudo.SetActive(true);
        }
    }

    public bool EstaProtegido()
    {
        return protegido;
    }

    public void UsarProtecao()
    {
        protegido = false;

        if (escudo != null)
        {
            escudo.SetActive(false);
        }
    }
}