using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject telaPause;

    private bool jogoPausado = false;

    void Start()
    {
        telaPause.SetActive(false);
    }

    public void PausarJogo()
    {
        jogoPausado = true;
        telaPause.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ContinuarJogo()
    {
        jogoPausado = false;
        telaPause.SetActive(false);
        Time.timeScale = 1f;
    }
}