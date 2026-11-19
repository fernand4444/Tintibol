
using UnityEngine;

public class MostrarInformacao : MonoBehaviour
{
    public GameObject telaInformacao;

    [Header("Audio da Informacao")]
    public AudioClip somInformacao;

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

                // Toca o som quando a informação aparece
                if (somInformacao != null)
                {
                    AudioSource.PlayClipAtPoint(
                        somInformacao,
                        transform.position
                    );
                }

                Time.timeScale = 0f;

                jaMostrou = true;

                Debug.Log("Jogo pausado e tela aberta!");
            }
        }
    }
}
