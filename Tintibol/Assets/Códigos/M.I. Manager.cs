using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MPManager : MonoBehaviour
{
    [SerializeField] private string nomedacena;
    [SerializeField] private GameObject painelMenuInicial;
    [SerializeField] private GameObject painelCutscene;
    [SerializeField] private GameObject painelJogo;

    public void Jogar()
    {
        SceneManager.LoadScene(nomedacena);
    }

    public void Cutscene()
    {
        painelMenuInicial.SetActive(false);
        painelCutscene.SetActive(true);
    }

    public void Jogo()
    {
        SceneManager.LoadScene("Jogo");
        painelMenuInicial.SetActive(false);
        painelCutscene.SetActive(false);
        painelJogo.SetActive(true);
    }
}
