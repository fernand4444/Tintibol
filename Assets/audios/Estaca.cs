
using UnityEngine;

public class Estaca : MonoBehaviour
{
    public AudioClip somDaEstaca;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("A Estaca colidiu com: " + collision.gameObject.name);

        if (somDaEstaca != null)
        {
            audioSource.PlayOneShot(somDaEstaca);
        }
        else
        {
            Debug.LogError("O som da Estaca não foi colocado!");
        }
    }
}
