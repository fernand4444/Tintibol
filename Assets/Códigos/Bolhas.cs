
using UnityEngine;

public class Bolhas : MonoBehaviour
{
    public float velocidade = 2f;

    void Update()
    {
        transform.Translate(Vector2.up * velocidade * Time.deltaTime);
    }
}
