
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioTornado : MonoBehaviour
{
    [Header("Áudio do Tornado")]
    public AudioClip somTornado;

    [Range(0f, 5f)]
    public float volume = 1f;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;
    public AudioMixerGroup grupoAudio;

    public string parametroVolume = "VolumeTornado";

    private AudioSource audioSource;

    public static bool TornadoAtivo { get; private set; }

    public static void DefinirTornadoAtivo(bool ativo)
    {
        TornadoAtivo = ativo;
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = true;
        audioSource.spatialBlend = 0f;
        audioSource.outputAudioMixerGroup = grupoAudio;
        audioSource.clip = somTornado;
    }

    private void Update()
    {
        if (audioMixer != null)
        {
            float volumeDB = volume <= 0f
                ? -80f
                : 20f * Mathf.Log10(volume);

            audioMixer.SetFloat(parametroVolume, volumeDB);
        }
        else
        {
            audioSource.volume = Mathf.Clamp01(volume);
        }

        if (TornadoAtivo && somTornado != null)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = somTornado;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
        }
    }

    private void OnDisable()
    {
        if (audioSource != null)
            audioSource.Stop();
    }
}
