using UnityEngine;


public class MostrarInformacao : MonoBehaviour
{
    public GameObject telaInformacao;


    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Objeto que encostou: " + other.name);
        Debug.Log("Tag do objeto: " + other.tag);


        if (other.CompareTag("Player"))
        {
            Debug.Log("PLAYER DETECTADO!");


            if (telaInformacao != null)
            {
                telaInformacao.SetActive(true);
                Debug.Log("TELA ATIVADA!");
            }
            else
            {
                Debug.LogError("A tela não foi ligada ao script!");
            }
        }
    }
}