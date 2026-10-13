using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject telaPause;

    private bool jogoPausado = false;

    void Start()
    {
        telaPause.SetActive(false);
        jogoPausado = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PausarOuContinuar();
        }
    }

    public void PausarOuContinuar()
    {
        if (jogoPausado)
        {
            ContinuarJogo();
        }
        else
        {
            PausarJogo();
        }
    }

    public void PausarJogo()
    {
        jogoPausado = true;
        telaPause.SetActive(true);
        Time.timeScale = 0f;

        Debug.Log("Pausando");
    }

    public void ContinuarJogo()
    {
        jogoPausado = false;
        telaPause.SetActive(false);
        Time.timeScale = 1f;
    }
}