
using UnityEngine;
using UnityEngine.Video;

public class CutsceneVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject jogador;

    private void Start()
    {
        videoPlayer.playOnAwake = false;
        videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
        videoPlayer.targetCamera = Camera.main;

        videoPlayer.loopPointReached += FinalizarCutscene;
    }

    public void IniciarCutscene()
    {
        // Pausa o jogo
        Time.timeScale = 0f;

        // Desativa o jogador
        if (jogador != null)
        {
            jogador.SetActive(false);
        }

        // Inicia o vídeo
        videoPlayer.Play();
    }

    private void FinalizarCutscene(VideoPlayer vp)
    {
        // Retoma o tempo do jogo
        Time.timeScale = 1f;

        // Ativa o jogador novamente
        if (jogador != null)
        {
            jogador.SetActive(true);
        }

        // Para o vídeo
        videoPlayer.Stop();
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= FinalizarCutscene;
        }
    }
}
