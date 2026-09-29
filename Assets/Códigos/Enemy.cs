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

    private Vector2 _initialPosition;

    private Vector2 _moveTarget;
    private bool _isReturning;
    private float _originalLocalScaleX;

    private int _currentLP;

    private Animator _animator;

    private bool _isAlive;

    private Collider2D _collider2D;

    private AudioSource _audioSource;

    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();
        _audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _isAlive = true;

        if (shouldFlip)
            _originalLocalScaleX = transform.localScale.x;

        _moveTarget = useTransform ? moveDestination.localPosition : movePosition;
        _initialPosition = transform.position;
        _currentLP = maxLP;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isAlive)
            MovePlatform();
    }

    private void MovePlatform()
    {
        Vector2 target = _initialPosition + _moveTarget;
        Vector2 destination = _isReturning ? _initialPosition : target;

        if (Vector2.Distance(transform.position, destination) < 1f)
        {
            _isReturning = !_isReturning;
            destination = _isReturning ? _initialPosition : target;
        }

        if (shouldFlip)
        {
            transform.localScale = new Vector3(
                _originalLocalScaleX * (_isReturning ? -1 : 1),
                transform.localScale.y,
                transform.localScale.z
            );
        }

        transform.position +=
            (Vector3)((destination - (Vector2)transform.position).normalized)
            * moveSpeed * Time.deltaTime;
    }

    public void TakeDamage(int damage)
    {
        _currentLP -= damage;

        if (_currentLP <= 0)
        {
            _currentLP = 0;
            _isAlive = false;

            if (_collider2D != null)
                _collider2D.enabled = false;

            Destroy(gameObject);
        }

        _currentLP = Mathf.Clamp(_currentLP, 0, maxLP);
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

    private void DealDamageToPlayer(Collision2D other, bool isEntering)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;

        if (!other.gameObject.TryGetComponent<PlayerControler>(out var player))
            return;

        Rigidbody2D playerRigidbody = other.rigidbody;
        bool playerIsFalling = playerRigidbody == null || playerRigidbody.linearVelocity.y <= 0f;
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
                    playerRigidbody.linearVelocity = new Vector2(playerRigidbody.linearVelocity.x, stompBounce);
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