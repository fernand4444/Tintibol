using UnityEngine;
using TMPro;

public class ContadorEstrelas : MonoBehaviour
{
    public TextMeshProUGUI textoEstrelas;

    [Header("Quantidade de estrelas nessa fase")]
    public int totalEstrelas = 5;

    void Start()
    {
        Coletavel.estrelasColetadas = 0;
        AtualizarTexto();
    }

    void Update()
    {
        AtualizarTexto();
    }

    void AtualizarTexto()
    {
        textoEstrelas.text = "⭐ " + Coletavel.estrelasColetadas + "/" + totalEstrelas;
    }
}