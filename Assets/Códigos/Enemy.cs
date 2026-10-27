using System.Collections;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    public int maxLP;
    public int damage;


    [SerializeField] private int stompDamage = 1;
    [SerializeField] private float stompBounce = 8f;


    public float moveSpeed;
    public bool useTransform;
    public bool shouldFlip;


    [SerializeField] private Vector2 movePosition;
    [SerializeField] private Transform moveDestination;
    [SerializeField] private int blinkHitTimes;
    [SerializeField] private float blinkHitDuration;


    [Header("Animation")]
    [SerializeField] private float deathAnimationDuration = 1f;


    private static readonly int MovingHash =
        Animator.StringToHash("Moving");


    private static readonly int DieHash =
        Animator.StringToHash("Die");


    private Vector2 _initialPosition;
    private Vector2 _moveTarget;


    private bool _isReturning;
    private bool _isAlive;


    private float _originalLocalScaleX;
    private int _currentLP;


    private Animator _animator;
    private Collider2D _collider2D;
    private AudioSource _audioSource;
    private SpriteRenderer _spriteRenderer;


    void Start()
    {
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();
        _audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();


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


        // Atualiza a animação de movimento.
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
        // Impede que o inimigo receba dano após morrer.
        if (!_isAlive)
            return;


        _currentLP -= damage;
        _currentLP = Mathf.Clamp(_currentLP, 0, maxLP);


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


        // Interrompe a animação de movimento.
        if (_animator != null)
        {
            _animator.SetBool(MovingHash, false);
            _animator.SetTrigger(DieHash);
        }


        // Desativa a colisão para evitar novos danos.
        if (_collider2D != null)
            _collider2D.enabled = false;


        // Aguarda a animação de morte antes de destruir o objeto.
        if (_animator != null)
            StartCoroutine(DestroyAfterDeath());
        else
            Destroy(gameObject);
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
        bool isEntering)
    {
        if (!_isAlive)
            return;


        if (!other.gameObject.CompareTag("Player"))
            return;


        if (!other.gameObject.TryGetComponent<PlayerControler>(
                out var player))
            return;


        Rigidbody2D playerRigidbody = other.rigidbody;


        bool playerIsFalling =
            playerRigidbody == null ||
            playerRigidbody.linearVelocity.y <= 0f;


        bool playerLandedOnTop = false;


        for (int i = 0; i < other.contactCount; i++)
        {
            if (other.GetContact(i).normal.y < -0.5f)
            {
                playerLandedOnTop = true;
                break;
            }
        }


        if (playerLandedOnTop)
        {
            if (isEntering && playerIsFalling)
            {
                TakeDamage(stompDamage);


                if (playerRigidbody != null)
                {
                    playerRigidbody.linearVelocity =
                        new Vector2(
                            playerRigidbody.linearVelocity.x,
                            stompBounce
                        );
                }
            }


            return;
        }


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
}