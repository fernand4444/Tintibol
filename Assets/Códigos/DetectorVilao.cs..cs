
using System.Collections;
using UnityEngine;

public class DetectorVilao : MonoBehaviour
{
    public GameObject vilao;
    public float tempoEspera = 8f;

    private bool jogadorDetectado = false;

    private void Start()
    {
        if (vilao == null)
        {
            Debug.LogError("O campo Vilao não foi preenchido!");
            return;
        }

        vilao.SetActive(false);
        Debug.Log("Vilão escondido no início.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (jogadorDetectado)
            return;

        if (other.CompareTag("Player") ||
            other.transform.root.CompareTag("Player") ||
            other.GetComponentInParent<PlayerControler>() != null)
        {
            jogadorDetectado = true;
            Debug.Log("Jogador detectado! Aguardando 8 segundos.");

            StartCoroutine(AtivarVilao());
        }
    }

    private IEnumerator AtivarVilao()
    {
        yield return new WaitForSecondsRealtime(tempoEspera);

        if (vilao == null)
        {
            Debug.LogError("Vilão não está atribuído!");
            yield break;
        }

        // Ativa os objetos pais, caso estejam desativados.
        Transform pai = vilao.transform.parent;

        while (pai != null)
        {
            pai.gameObject.SetActive(true);
            pai = pai.parent;
        }

        vilao.SetActive(true);

        Debug.Log("Vilão ativado!");
        Debug.Log("ActiveSelf: " + vilao.activeSelf);
        Debug.Log("ActiveInHierarchy: " + vilao.activeInHierarchy);
    }
}
