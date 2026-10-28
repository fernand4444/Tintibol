using UnityEngine;


public class FecharTelaInformacao : MonoBehaviour
{
    public GameObject telaInformacao;


    public void FecharTela()
    {
        telaInformacao.SetActive(false);


        Time.timeScale = 1f;
    }
}