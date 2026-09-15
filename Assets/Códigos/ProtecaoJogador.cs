using UnityEngine;

public class ProtecaoJogador : MonoBehaviour
{
    public GameObject protecaoVisual;

    private bool protegido = false;

    public bool EstaProtegido()
    {
        return protegido;
    }

    public void AtivarProtecao()
    {
        protegido = true;

        protecaoVisual.SetActive(true);
    }
}
