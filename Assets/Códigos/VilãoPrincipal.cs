
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Vidadovilao : MonoBehaviour
{
    [Header("Vida do Vilão")]
    public Barradevida healthBar;
    public int maxLP = 100;
    public int damage;

    private bool _isAlive;
    private int _currentLP;
    private Slider _mySlider;

    private static readonly int DieHash =
        Animator.StringToHash("Die");

    private static readonly int TornadoHash =
        Animator.StringToHash("Tornado");

    private Animator _animator;
    private Collider2D _collider2D;

    [Header("Spawner")]
    public GameObject prefab;
    public Transform pontoDeSpawn;
    public float intervaloSpawn = 2f;
    public float intervaloTornado = 5f;

    private bool _TornadoAtivado = false;
    private Coroutine TornadoCoroutine;

    private void Awake()
    {
        _mySlider = GetComponent<Slider>();
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();
    }

    void Start()
    {
        _isAlive = true;
        _currentLP = maxLP;

        Initialize(maxLP, _currentLP);
    }

    public void Initialize(int maxLP, int currentLP)
    {
        if (_mySlider == null)
            return;

        _mySlider.maxValue = maxLP;
        _mySlider.value = currentLP;
    }

    public void SetHealth(int currentLP)
    {
        if (_mySlider == null)
            return;

        _mySlider.value = currentLP;
    }

    public void TakeDamage(int damage)
    {
        if (!_isAlive || damage <= 0)
            return;

        _currentLP -= damage;
        _currentLP = Mathf.Clamp(_currentLP, 0, maxLP);

        SetHealth(_currentLP);

        Debug.Log("Vilão recebeu " + damage +
                  " de dano. Vida restante: " + _currentLP);

        // Ativa o Tornado quando chegar à metade da vida
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

                yield return new WaitForSeconds(intervaloSpawn);
            }

            yield return new WaitForSeconds(intervaloTornado);
        }
    }

    private void SpawnObjeto()
    {
        if (prefab == null || pontoDeSpawn == null)
            return;

        Instantiate(
            prefab,
            pontoDeSpawn.position,
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

        if (_animator != null)
            _animator.SetTrigger(DieHash);

        if (_collider2D != null)
            _collider2D.enabled = false;
    }
}
