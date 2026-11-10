

using UnityEngine;

public class GeradorBolhas : MonoBehaviour
{
    public GameObject bolha;
    public float tempo = 1f;

    void Start()
    {
        InvokeRepeating("CriarBolha", 0f, tempo);
    }

    void CriarBolha()
    {
        Instantiate(bolha, transform.position, Quaternion.identity);
    }
}
