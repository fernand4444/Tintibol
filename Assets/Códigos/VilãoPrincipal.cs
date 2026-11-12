
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Vidadovilao : MonoBehaviour
{
    [Header("Vida do Vilão")]
    public Slider healthBar;
    public int maxLP = 100;
    public int damage;

    private bool _isAlive;
    private int _currentLP;

    [Header("Invulnerabilidade")]
    private float _lastDamageTime = -Mathf.Infinity;
    public float damageCooldown = 3f;

    [Header("Efeito Visual de Dano")]
    public Color corDano = Color.red;

    private SpriteRenderer[] spriteRenderers;
    private Color[] coresOriginais;
    private Coroutine efeitoInvulnerabilidade;

    private static readonly int DieHash =
        Animator.StringToHash("Die");

    private static readonly int TornadoHash =
        Animator.StringToHash("Tornado");

    private Animator _animator;
    private Collider2D _collider2D;

    private float tempo;

    [Header("Spawner")]
    public GameObject prefab;
    public Transform pontoDeSpawn;
    public Transform pontoDeSpawn2;
    public Transform pontoDeSpawn3;
    public Transform pontoDeSpawn4;
    public Transform pontoDeSpawn5;

    public int SpawnerCount = 10;
    public float intervalo = 2f;
    public float raio = 5f;
    public float intervaloTornado = 5f;

    private bool _TornadoAtivado = false;
    private Coroutine TornadoCoroutine;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();

        // Encontra todos os sprites do vilão
        spriteRenderers =
            GetComponentsInChildren<SpriteRenderer>(true);

        // Guarda as cores originais
        coresOriginais = new Color[spriteRenderers.Length];

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            coresOriginais[i] = spriteRenderers[i].color;
        }
    }

    void Start()
    {
        _isAlive = true;
        _currentLP = maxLP;

        Initialize(maxLP, _currentLP);
    }

    public void Initialize(int maxLP, int currentLP)
    {
        if (healthBar == null)
        {
            Debug.LogError("Barra de vida não foi atribuída no Inspector!");
            return;
        }

        healthBar.minValue = 0;
        healthBar.maxValue = maxLP;
        healthBar.value = currentLP;
    }

    public void SetHealth(int currentLP)
    {
        if (healthBar == null)
            return;

        healthBar.value = currentLP;
    }

    public void TakeDamage(int damage)
    {
        if (!_isAlive || damage <= 0)
            return;

        // Verifica a invulnerabilidade
        if (Time.time - _lastDamageTime < damageCooldown)
            return;

        // Registra o momento do dano
        _lastDamageTime = Time.time;

        _currentLP -= damage;
        _currentLP = Mathf.Clamp(_currentLP, 0, maxLP);

        SetHealth(_currentLP);

        Debug.Log("Vilão recebeu " + damage +
                  " de dano. Vida restante: " + _currentLP);

        // Ativa o efeito vermelho
        AtivarEfeitoInvulnerabilidade();

        if (_currentLP <= maxLP / 2 && !_TornadoAtivado)
        {
            _TornadoAtivado = true;
            TornadoCoroutine = StartCoroutine(CicloTornado());
        }

        if (_currentLP <= 0)
        {
            Die();
            Destroy(gameObject);
        }
    }

    private void AtivarEfeitoInvulnerabilidade()
    {
        if (efeitoInvulnerabilidade != null)
        {
            StopCoroutine(efeitoInvulnerabilidade);
        }

        efeitoInvulnerabilidade =
            StartCoroutine(EfeitoVermelho());
    }

    private IEnumerator EfeitoVermelho()
    {
        // Deixa o vilão vermelho
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (spriteRenderers[i] != null)
                spriteRenderers[i].color = corDano;
        }

        // Aguarda o tempo de invulnerabilidade
        yield return new WaitForSeconds(damageCooldown);

        // Restaura as cores originais
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (spriteRenderers[i] != null)
                spriteRenderers[i].color = coresOriginais[i];
        }

        efeitoInvulnerabilidade = null;
    }

    private IEnumerator CicloTornado()
    {
        while (_isAlive)
        {
            _animator.SetTrigger(TornadoHash);

            yield return new WaitUntil(() =>
                _animator.GetCurrentAnimatorStateInfo(0).IsName("Tornado")
                || !_isAlive);

            if (!_isAlive)
                yield break;

            while (_isAlive &&
                   _animator.GetCurrentAnimatorStateInfo(0).IsName("Tornado") &&
                   _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                SpawnObjeto();

                yield return new WaitForSeconds(intervalo);
            }

            yield return new WaitForSeconds(intervaloTornado);
        }
    }

    private void SpawnObjeto()
    {
        if (prefab == null || pontoDeSpawn == null)
            return;

        Vector3 pos = pontoDeSpawn.position + new Vector3(
            UnityEngine.Random.Range(-raio, raio),
            UnityEngine.Random.Range(-raio, raio),
            0f
        );

        GameObject novoObjeto = Instantiate(
            prefab,
            pos,
            Quaternion.identity
        );

    }

    private void Die()
    {
        if (!_isAlive)
            return;

        _isAlive = false;

        if (TornadoCoroutine != null)
            StopCoroutine(TornadoCoroutine);

        if (efeitoInvulnerabilidade != null)
            StopCoroutine(efeitoInvulnerabilidade);

        if (_animator != null)
            _animator.SetTrigger(DieHash);

        if (_collider2D != null)
            _collider2D.enabled = false;
    }

    void Update()
    {
        if (!_isAlive)
            return;

        tempo += Time.deltaTime;

        if (tempo >= intervalo)
        {
            SpawnObjeto();
            tempo = 0f;
        }
    }
}
