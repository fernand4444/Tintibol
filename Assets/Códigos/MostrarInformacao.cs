using UnityEngine;


public class MostrarInformacao : MonoBehaviour
{
    public GameObject telaInformacao;


    private bool jaMostrou = false;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (jaMostrou)
            return;


        if (other.CompareTag("Player"))
        {
            if (telaInformacao != null)
            {
                telaInformacao.SetActive(true);


                Time.timeScale = 0f;


                jaMostrou = true;
                Debug.Log("Jogo pausado e tela aberta!");
            }
        }
    }
}