using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Transform cameraTransform;

    [Range(0f, 1f)]
    public float parallaxX = 0.2f;

    [Range(0f, 1f)]
    public float parallaxY = 0f;

    private Vector3 posicaoInicialFundo;
    private Vector3 posicaoInicialCamera;

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        posicaoInicialFundo = transform.position;
        posicaoInicialCamera = cameraTransform.position;
    }

    void LateUpdate()
    {
        if (cameraTransform == null)
            return;

        float movimentoX =
            cameraTransform.position.x - posicaoInicialCamera.x;

        float movimentoY =
            cameraTransform.position.y - posicaoInicialCamera.y;

        float novaPosicaoX =
            posicaoInicialFundo.x + movimentoX * parallaxX;

        float novaPosicaoY =
            posicaoInicialFundo.y + movimentoY * parallaxY;

        transform.position = new Vector3(
            novaPosicaoX,
            novaPosicaoY,
            posicaoInicialFundo.z
        );
    }
}