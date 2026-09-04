using UnityEngine;

public class TextoInicio : MonoBehaviour
{
    public float tempo = 3f;

    void Start()
    {
        Invoke("Sumir", tempo);
    }

    void Sumir()
    {
        gameObject.SetActive(false);
    }
}