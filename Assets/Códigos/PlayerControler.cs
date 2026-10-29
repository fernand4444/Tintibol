using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerControler : MonoBehaviour
{
    public int maxLP = 100;
    private int _currentLP;
    private float _lastDamageTime = -Mathf.Infinity;
    public float damageCooldown = 3f;
    public Barradevida healthBar;


    public void RecuperarVida(int quantidade)
    {
        _currentLP += quantidade;


        if (_currentLP > maxLP)
        {
            _currentLP = maxLP;
        }


        if (healthBar != null)
        {
            healthBar.SetHealth(_currentLP);
        }
    }


    public float velocidade = 10f;
    public float forcaPulo = 10f;
    public float knockbackForce = 8f;
    public float knockbackUpwardFactor = 0.75f;
    public bool noChao = false;
    public bool andando = false;




    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private AudioSource _audioSource;


    public AudioClip somPulo;
    private Vector2 _lastMoveDirection = Vector2.right;
    private bool _hasJumped;


    public bool HasJumped => _hasJumped;


    public void ConsumeJump()
    {
        _hasJumped = false;
    }


    private bool _isReloading = false;
    void Start()
    {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _animator = gameObject.GetComponent<Animator>();
        _audioSource = gameObject.GetComponent<AudioSource>();


        _currentLP = maxLP;


        if (healthBar != null)
        {
            healthBar.Initialize(maxLP, _currentLP);
        }


        if (CheckpointManager.temCheckpoint &&
            CheckpointManager.faseCheckpoint ==
            SceneManager.GetActiveScene().name)
        {
            transform.position = CheckpointManager.ultimaPosicao;


            if (_rigidbody2D != null)
            {
                _rigidbody2D.linearVelocity = Vector2.zero;
            }


            Debug.Log("Jogador retornou ao checkpoint!");
        }
    }


    private void KillPlayer()
    {
        if (_currentLP <= 0 && !_isReloading)
        {
            _isReloading = true;


            if (_rigidbody2D != null)
            {
                _rigidbody2D.linearVelocity =
                    new Vector2(0, _rigidbody2D.linearVelocity.y);
            }


            SceneManager.LoadScene(
                SceneManager.GetActiveScene().name
            );
        }
    }


    public void TakeDamage(
        int amount,
        bool applyKnockback = true,
        bool ignoreCooldown = false)
    {
        if (!ignoreCooldown &&
            Time.time - _lastDamageTime < damageCooldown)
        {
            return;
        }


        _lastDamageTime = Time.time;
        _currentLP -= amount;


        if (_currentLP < 0)
        {
            _currentLP = 0;
        }


        if (healthBar != null)
        {
            healthBar.SetHealth(_currentLP);
        }


        if (applyKnockback && amount > 0)
        {
            ApplyDamageKnockback();
        }


        KillPlayer();
    }


    private void ApplyDamageKnockback()
    {
        Vector2 direction = _lastMoveDirection;


        if (direction == Vector2.zero)
        {
            if (_rigidbody2D.linearVelocity.x != 0)
            {
                direction = new Vector2(
                    Mathf.Sign(_rigidbody2D.linearVelocity.x), 0f);
            }
            else
            {
                direction = _spriteRenderer.flipX
                    ? Vector2.left
                    : Vector2.right;
            }
        }


        Vector2 knockbackDirection =
            new Vector2(-direction.x, knockbackUpwardFactor).normalized;


        _rigidbody2D.linearVelocity =
            knockbackDirection * knockbackForce;
    }


    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "chao")
        {
            noChao = true;
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("chao"))
        {
            noChao = true;
            _hasJumped = false;
        }
    }


    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "chao")
        {
            noChao = false;
        }
    }


    void Update()
    {
        andando = false;


        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-velocidade * Time.deltaTime, 0, 0);


            _spriteRenderer.flipX = true;
            _lastMoveDirection = Vector2.left;


            Debug.Log("Left");


            if (noChao == true)
            {
                andando = true;
            }
        }


        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(velocidade * Time.deltaTime, 0, 0);


            _spriteRenderer.flipX = false;
            _lastMoveDirection = Vector2.right;


            Debug.Log("Right");


            if (noChao == true)
            {
                andando = true;
            }
        }


        if ((Input.GetKeyDown(KeyCode.UpArrow) ||
             Input.GetKeyDown(KeyCode.W) ||
             Input.GetKeyDown(KeyCode.Space)) &&
            noChao)
        {
            _hasJumped = true;
            _rigidbody2D.AddForce(
                Vector2.up * forcaPulo,
                ForceMode2D.Impulse
            );


            if (somPulo != null && _audioSource != null)
            {
                _audioSource.PlayOneShot(somPulo);
            }


            Debug.Log("Jump");
        }


        if (_currentLP <= 0)
        {
            KillPlayer();
            return;
        }


        _animator.SetBool("Andando", andando);
        _animator.SetBool("Pulando", !noChao);
    }
}