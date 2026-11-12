
using System;
using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Vida e Dano")]
    public int maxLP = 100;
    public int damage;

    [SerializeField] private int stompDamage = 1;
    [SerializeField] private float stompBounce = 8f;
    [SerializeField] private float stompTopTolerance = 0.2f;

    [Header("Movimentação")]
    public float moveSpeed;
    public bool useTransform;
    public bool shouldFlip;

    [SerializeField] private Vector2 movePosition;
    [SerializeField] private Transform moveDestination;
    [SerializeField] private int blinkHitTimes;
    [SerializeField] private float blinkHitDuration;

    [Header("Animação")]
    [SerializeField] private float deathAnimationDuration = 1f;

    [Header("Áudio de Morte")]
    [SerializeField] private AudioClip somMorte;
    [SerializeField, Range(0f, 1f)]
    private float volumeMorte = 1f;

    private static readonly int MovingHash =
        Animator.StringToHash("Moving");

    private static readonly int DieHash =
        Animator.StringToHash("Die");

    private Vector2 _initialPosition;
    private Vector2 _moveTarget;
    private bool _isReturning;
    private bool _isAlive;
    private bool _hasStompedPlayer;
    private float _originalLocalScaleX;
    private int _currentLP;

    private Animator _animator;
    private Collider2D _collider2D;

    public Action OnEnemyDied;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();

        _isAlive = true;

        if (shouldFlip)
            _originalLocalScaleX = transform.localScale.x;

        _moveTarget = useTransform
            ? moveDestination.localPosition
            : movePosition;

        _initialPosition = transform.position;
        _currentLP = maxLP;

        SetMovingAnimation(false);
    }

    void Update()
    {
        if (!_isAlive)
            return;

        MovePlatform();
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

    public void TakeDamage(int damage)
    {
        if (!_isAlive || damage <= 0)
            return;

        _currentLP -= damage;

        _currentLP = Mathf.Clamp(_currentLP, 0, maxLP);

        Debug.Log("Inimigo recebeu " + damage +
                  " de dano. Vida restante: " + _currentLP);

        if (_currentLP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (!_isAlive)
            return;

        _isAlive = false;

        Debug.Log(gameObject.name + " morreu!");

        // Toca o áudio em um objeto separado.
        if (somMorte != null)
        {
            GameObject objetoAudio =
                new GameObject("AudioMorteInimigo");

            objetoAudio.transform.position = transform.position;

            AudioSource fonteAudio =
                objetoAudio.AddComponent<AudioSource>();

            fonteAudio.clip = somMorte;
            fonteAudio.volume = volumeMorte;
            fonteAudio.playOnAwake = false;
            fonteAudio.spatialBlend = 0f;

            fonteAudio.Play();

            Destroy(objetoAudio, somMorte.length + 0.2f);

            Debug.Log("Áudio de morte iniciado!");
        }
        else
        {
            Debug.LogWarning(
                "O áudio de morte não foi colocado no Inspector!"
            );
        }

        // Ativa a animação de morte.
        if (_animator != null)
        {
            _animator.SetBool(MovingHash, false);
            _animator.SetTrigger(DieHash);
        }

        // Desativa a colisão.
        if (_collider2D != null)
            _collider2D.enabled = false;

        StartCoroutine(DestroyAfterDeath());
    }

    private IEnumerator DestroyAfterDeath()
    {
        yield return new WaitForSeconds(deathAnimationDuration);

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(
            transform.position,
            transform.position + (
                useTransform
                    ? moveDestination.localPosition
                    : (Vector3)movePosition
            ),
            useTransform ? Color.yellow : Color.red
        );
    }

    private void DealDamageToPlayer(
        Collision2D other,
        bool allowSideDamage)
    {
        if (!_isAlive)
            return;

        PlayerControler player =
            other.gameObject.GetComponentInParent<PlayerControler>();

        if (player == null || _collider2D == null)
            return;

        Rigidbody2D playerRigidbody = other.rigidbody;

        bool contactIsAtTop = false;

        float topLimit =
            _collider2D.bounds.max.y - stompTopTolerance;

        for (int i = 0; i < other.contactCount; i++)
        {
            if (other.GetContact(i).point.y >= topLimit)
            {
                contactIsAtTop = true;
                break;
            }
        }

        if (contactIsAtTop)
        {
            if (player.HasJumped &&
                playerRigidbody != null &&
                playerRigidbody.linearVelocity.y < 0f &&
                !_hasStompedPlayer)
            {
                _hasStompedPlayer = true;

                player.ConsumeJump();

                TakeDamage(stompDamage);

                playerRigidbody.linearVelocity = new Vector2(
                    playerRigidbody.linearVelocity.x,
                    Mathf.Max(
                        playerRigidbody.linearVelocity.y,
                        stompBounce
                    )
                );
            }

            return;
        }

        if (allowSideDamage)
            player.TakeDamage(damage);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        DealDamageToPlayer(other, true);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        DealDamageToPlayer(other, false);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.GetComponentInParent<PlayerControler>() != null)
            _hasStompedPlayer = false;
    }
}
