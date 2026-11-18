
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class ContagemRegressiva : MonoBehaviour
{
    public TextMeshProUGUI textoContagem;

    private bool ativado = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !ativado)
        {
            ativado = true;
            StartCoroutine(Contar());
        }
    }

    IEnumerator Contar()
    {
        textoContagem.gameObject.SetActive(true);

        textoContagem.text = "3";
        yield return new WaitForSeconds(1f);

        textoContagem.text = "2";
        yield return new WaitForSeconds(1f);

        textoContagem.text = "1";
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("MenuInicial");
    }
}
