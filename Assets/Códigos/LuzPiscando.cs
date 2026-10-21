using UnityEngine;
using UnityEngine.Rendering.Universal;


public class LuzPiscando : MonoBehaviour
{
    public Light2D luz;


    public float intensidadeFraca = 0.3f;
    public float intensidadeForte = 1f;
    public float velocidade = 2f;


    private void Update()
    {
        if (luz == null)
            return;


        float variacao = (Mathf.Sin(Time.time * velocidade) + 1f) / 2f;


        luz.intensity = Mathf.Lerp(
            intensidadeFraca,
            intensidadeForte,
            variacao
        );
    }
}
