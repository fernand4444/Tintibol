
using UnityEngine;
using UnityEngine.SceneManagement;

public class BotaoTutorial : MonoBehaviour
{
    public void IrParaTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
