
using UnityEngine;

public class AudioArea : MonoBehaviour
{
    [Header("Configuração do Áudio")]
    public AudioClip som;

    [Range(0f, 1f)]
    public float volume = 1f;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
        audioSource.volume = volume;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (som != null)
            {
                audioSource.PlayOneShot(som);
            }
            else
            {
                Debug.LogWarning("Nenhum áudio foi colocado na área!");
            }
        }
    }
}
