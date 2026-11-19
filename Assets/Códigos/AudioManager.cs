
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    [Header("Sons do jogo")]
    public AudioClip SonsdeFundo;
    public AudioClip SomdePulo;
    public AudioClip SomdeMorte;
    public AudioClip SomdeCuspe;
    public AudioClip SomdasEstacas;
    public AudioClip TomandoDano;
    public AudioClip SomdoImpulso;
    public AudioClip SomdeBolhas;
    public AudioClip SomdeFerrodoChão;

    private AudioSource musica;
    private AudioSource efeitos;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        musica = gameObject.AddComponent<AudioSource>();
        efeitos = gameObject.AddComponent<AudioSource>();

        musica.loop = true;
        musica.playOnAwake = false;
        musica.spatialBlend = 0f;

        efeitos.loop = false;
        efeitos.playOnAwake = false;
        efeitos.spatialBlend = 0f;
    }

    private void Start()
    {
        TocarSonsdeFundo();
    }

    public void TocarSonsdeFundo()
    {
        TrocarMusica(SonsdeFundo);
    }

    // Troca a música atual por outra
    public void TrocarMusica(AudioClip novaMusica)
    {
        if (novaMusica == null)
            return;

        if (musica.clip == novaMusica && musica.isPlaying)
            return;

        musica.Stop();
        musica.clip = novaMusica;
        musica.Play();
    }

    public void PararMusica()
    {
        musica.Stop();
    }

    public void TocarSomdePulo()
    {
        TocarEfeito(SomdePulo);
    }

    public void TocarSomdeMorte()
    {
        TocarEfeito(SomdeMorte);
    }

    public void TocarSomdeCuspe()
    {
        TocarEfeito(SomdeCuspe);
    }

    public void TocarSomdasEstacas()
    {
        TocarEfeito(SomdasEstacas);
    }

    public void TocarTomandoDano()
    {
        TocarEfeito(TomandoDano);
    }

    public void TocarSomdoImpulso()
    {
        TocarEfeito(SomdoImpulso);
    }

    public void TocarSomdeBolhas()
    {
        TocarEfeito(SomdeBolhas);
    }

    public void TocarSomdeFerrodoChão()
    {
        TocarEfeito(SomdeFerrodoChão);
    }

    private void TocarEfeito(AudioClip som)
    {
        if (som != null)
        {
            efeitos.PlayOneShot(som);
        }
    }
}
