using UnityEngine;
using UnityEngine.SceneManagement;

public class MPManager : MonoBehaviour
{
    public void Jogar()
    {
        SceneManager.LoadScene("Cutscene");
    }
}
