
using System.Collections;
using System.Collections.Generic;
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

    private static readonly int MovingHash =
        Animator.StringToHash("Moving");

    private Animator _animator;
    private Collider2D _collider2D;

    private float tempo;

    [Header("Movimentação")]
    public float moveSpeed;
    public bool useTransform;
    public bool shouldFlip;

    [SerializeField] private Vector2 movePosition;
    [SerializeField] private Transform moveDestination;

    private Vector2 _initialPosition;
    private Vector2 _moveTarget;
    private bool _isReturning;
    private float _originalLocalScaleX;

    [Header("Spawner")]
    public GameObject prefab;
    public Transform pontoDeSpawn;
    public Transform pontoDeSpawn2;
    public Transform pontoDeSpawn3;
    public Transform pontoDeSpawn4;
    public Transform pontoDeSpawn5;

    public float intervalo = 2f;
    public float raio = 5f;

    private bool _TornadoAtivado = false;
    private Coroutine TornadoCoroutine;

    // Guarda todos os objetos criados pelo Spawner
    private List<GameObject> objetosSpawnados = new List<GameObject>();

    [Header("Teletransporte do Jogador")]
    public GameObject jogador;
    public Transform pontoDeTeletransporte;
    public float tempoParaTeletransportar = 5f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();

        spriteRenderers =
            GetComponentsInChildren<SpriteRenderer>(true);

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

        if (shouldFlip)
            _originalLocalScaleX = transform.localScale.x;

        if (useTransform && moveDestination != null)
            _moveTarget = moveDestination.localPosition;
        else
            _moveTarget = movePosition;

        _initialPosition = transform.position;

        SetMovingAnimation(false);
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

        if (Time.time - _lastDamageTime < damageCooldown)
            return;

        _lastDamageTime = Time.time;

        _currentLP -= damage;
        _currentLP = Mathf.Clamp(_currentLP, 0, maxLP);

        SetHealth(_currentLP);

        Debug.Log("Vilão recebeu " + damage +
                  " de dano. Vida restante: " + _currentLP);

        if (_currentLP <= 0)
        {
            // Impede novos danos
            _isAlive = false;

            // Para as corrotinas
            if (TornadoCoroutine != null)
                StopCoroutine(TornadoCoroutine);

            if (efeitoInvulnerabilidade != null)
                StopCoroutine(efeitoInvulnerabilidade);

            // Desativa o Animator do vilão
            if (_animator != null)
                _animator.enabled = false;

            // Desativa o collider
            if (_collider2D != null)
                _collider2D.enabled = false;

            // Faz o vilão desaparecer imediatamente
            foreach (SpriteRenderer sprite in spriteRenderers)
            {
                if (sprite != null)
                    sprite.enabled = false;
            }

            // Destrói todos os furacões criados
            DestruirFuracoes();

            Debug.Log("Vilão e furacões desapareceram!");

            // Inicia a contagem de 5 segundos
            StartCoroutine(AguardarTeletransporte());

            return;
        }

        AtivarEfeitoInvulnerabilidade();

        if (_currentLP <= maxLP / 2 && !_TornadoAtivado)
        {
            _TornadoAtivado = true;
        }
    }

    private void DestruirFuracoes()
    {
        foreach (GameObject objeto in objetosSpawnados)
        {
            if (objeto != null)
            {
                Destroy(objeto);
            }
        }

        objetosSpawnados.Clear();
    }

    private IEnumerator AguardarTeletransporte()
    {
        yield return new WaitForSeconds(tempoParaTeletransportar);

        TeletransportarJogador();

        Destroy(gameObject);
    }

    private void TeletransportarJogador()
    {
        if (jogador == null || pontoDeTeletransporte == null)
        {
            Debug.LogError("Jogador ou ponto de teletransporte não atribuído!");
            return;
        }

        jogador.transform.position = pontoDeTeletransporte.position;

        Debug.Log("Jogador teletransportado!");
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
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (spriteRenderers[i] != null)
                spriteRenderers[i].color = corDano;
        }

        yield return new WaitForSeconds(damageCooldown);

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (spriteRenderers[i] != null)
                spriteRenderers[i].color = coresOriginais[i];
        }

        efeitoInvulnerabilidade = null;
    }

    private void SpawnObjeto()
    {
        if (prefab == null || pontoDeSpawn == null || !_isAlive)
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

        objetosSpawnados.Add(novoObjeto);
    }

    private void MovePlatform()
    {
        Vector2 target = _initialPosition + _moveTarget;

        Vector2 destination = _isReturning
            ? _initialPosition
            : target;

        if (Vector2.Distance(transform.position, destination) < 1f)
        {
            _isReturning = !_isReturning;

            destination = _isReturning
                ? _initialPosition
                : target;
        }

        if (shouldFlip && moveSpeed > 0f)
        {
            transform.localScale = new Vector3(
                _originalLocalScaleX * (_isReturning ? -1 : 1),
                transform.localScale.y,
                transform.localScale.z
            );
        }

        Vector2 direction =
            (destination - (Vector2)transform.position).normalized;

        transform.position +=
            (Vector3)(direction * moveSpeed * Time.deltaTime);

        bool isMoving =
            moveSpeed > 0f &&
            Vector2.Distance(transform.position, destination) > 0.05f;

        SetMovingAnimation(isMoving);
    }

    private void SetMovingAnimation(bool isMoving)
    {
        if (_animator != null)
            _animator.SetBool(MovingHash, isMoving);
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
        {
            _animator.SetBool(MovingHash, false);
            _animator.SetTrigger(DieHash);
        }

        if (_collider2D != null)
            _collider2D.enabled = false;
    }

    void Update()
    {
        if (!_isAlive)
            return;

        MovePlatform();

        tempo += Time.deltaTime;

        if (tempo >= intervalo)
        {
            SpawnObjeto();
            tempo = 0f;
        }
    }
}
