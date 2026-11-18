using UnityEngine;
using UnityEngine.SceneManagement;

public class BotaoMenuInicial : MonoBehaviour
{
    public void IrParaMenuInicial()
    {
        SceneManager.LoadScene("MenuInicial");
    }
}