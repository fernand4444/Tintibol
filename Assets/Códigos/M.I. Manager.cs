using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MPManager : MonoBehaviour
{
    [SerializeField] private string nomedacena;
    [SerializeField] private GameObject painelMenuInicial;
    [SerializeField] private GameObject painelCutscene;
    [SerializeField] private GameObject painelJogo;
    [SerializeField] private VideoPlayer videoPlayer;

    public void Jogar()
    {
        SceneManager.LoadScene(nomedacena);
    }

    public void Cutscene()
    {
        painelMenuInicial.SetActive(false);
        painelCutscene.SetActive(true);
    }

    private void Start()
    {
        if (videoPlayer == null && painelCutscene != null)
        {
            videoPlayer = painelCutscene.GetComponentInChildren<VideoPlayer>();
        }

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
        }
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene("Jogo");
    }

    public void Jogo()
    {
        SceneManager.LoadScene("Jogo");
        painelMenuInicial.SetActive(false);
        painelCutscene.SetActive(false);
        painelJogo.SetActive(true);
    }
}
