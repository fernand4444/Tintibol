
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class ContagemRegressiva : MonoBehaviour
{
    public TextMeshProUGUI textoContagem;

    [Header("Áudio da Contagem")]
    public AudioClip somContagem;
    private AudioSource audioSource;

    private bool ativado = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }

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
        TocarSom();
        yield return new WaitForSeconds(1f);

        textoContagem.text = "2";
        TocarSom();
        yield return new WaitForSeconds(1f);

        textoContagem.text = "1";
        TocarSom();
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("MenuInicial");
    }

    private void TocarSom()
    {
        if (somContagem != null)
            audioSource.PlayOneShot(somContagem);
    }
}
