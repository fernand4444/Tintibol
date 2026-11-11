
using UnityEngine;

public class AudioTeclas : MonoBehaviour
{
    public AudioClip som;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.Z))
        {
            if (som != null)
            {
                audioSource.PlayOneShot(som);
            }
        }
    }
}
