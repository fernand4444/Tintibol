using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class Temporizador : MonoBehaviour
{
    public float tempoInicial = 60f;
    public TextMeshProUGUI textoTempo;


    private float tempoRestante;
    private bool tempoAcabou = false;


    void Start()
    {
        tempoRestante = tempoInicial;
        AtualizarTexto();
    }


    void Update()
    {
        if (tempoAcabou)
            return;


        tempoRestante -= Time.deltaTime;


        if (tempoRestante <= 0)
        {
            tempoRestante = 0;
            tempoAcabou = true;


            AtualizarTexto();


            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex
            );


            return;
        }


        AtualizarTexto();
    }


    void AtualizarTexto()
    {
        int segundos = Mathf.CeilToInt(tempoRestante);


        textoTempo.text = "TEMPO: " + segundos;
    }
}