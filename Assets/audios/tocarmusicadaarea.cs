
using UnityEngine;

public class TrocarMusicaArea : MonoBehaviour
{
    [Header("Música desta área")]
    public AudioClip musicaDaArea;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager manager = FindObjectOfType<AudioManager>();

            if (manager != null)
            {
                manager.TrocarMusica(musicaDaArea);
            }
        }
    }
}
