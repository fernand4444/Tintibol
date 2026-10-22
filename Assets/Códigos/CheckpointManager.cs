using UnityEngine;
using UnityEngine.SceneManagement;


public static class CheckpointManager
{
    public static Vector3 ultimaPosicao;
    public static bool temCheckpoint = false;
    public static string faseCheckpoint = "";


    public static void SalvarCheckpoint(Vector3 posicao)
    {
        ultimaPosicao = posicao;
        temCheckpoint = true;
        faseCheckpoint = SceneManager.GetActiveScene().name;


        Debug.Log("Checkpoint salvo na posição: " + posicao);
    }


    public static void LimparCheckpoint()
    {
        temCheckpoint = false;
        faseCheckpoint = "";
    }
}