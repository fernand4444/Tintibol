
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControler : MonoBehaviour
{
    [Header("Sistema de Vida")]
    public int maxLP = 100;
    private int _currentLP;
    private float _lastDamageTime = -Mathf.Infinity;
    public float damageCooldown = 3f;
    public Barradevida healthBar;

    [Header("Movimentação")]
    public float velocidade = 10f;
    public float forcaPulo = 10f;
    public float knockbackForce = 8f;
    public float knockbackUpwardFactor = 0.75f;

    public bool noChao = false;
    public bool andando = false;
    public bool cuspindo = false;

    [Header("Ataque de Cuspe")]
    public GameObject prefabCuspe;
    public Transform pontoCuspe;
    public float tempoCuspindo = 1f;
    public float tempoParaAtacar = 0.3f;
    public float velocidadeCuspe = 10f;

    [Header("Áudio")]
    public AudioClip somPulo;

    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private AudioSource _audioSource;

    private Vector2 _lastMoveDirection = Vector2.right;
    private Vector2 _attackDirection = Vector2.right;

    private bool _hasJumped;
    private bool _isReloading;
    private bool _isSpitting;

    public bool HasJumped => _hasJumped;

    public void ConsumeJump()
    {
        _hasJumped = false;
    }

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        _currentLP = maxLP;

        if (healthBar != null)
            healthBar.Initialize(maxLP, _currentLP);

        if (CheckpointManager.temCheckpoint &&
            CheckpointManager.faseCheckpoint ==
            SceneManager.GetActiveScene().name)
        {
            transform.position = CheckpointManager.ultimaPosicao;

            if (_rigidbody2D != null)
                _rigidbody2D.linearVelocity = Vector2.zero;

            Debug.Log("Jogador retornou ao checkpoint!");
        }
    }


    public void Cuspir()
    {
        if (_isSpitting || _animator == null)
            return;

        _attackDirection = _lastMoveDirection;


        if (_attackDirection == Vector2.zero)
        {
            _attackDirection = _spriteRenderer != null &&
                               _spriteRenderer.flipX
                ? Vector2.left
                : Vector2.right;
        }

        StartCoroutine(AnimacaoCuspir());
    }

    private IEnumerator AnimacaoCuspir()
    {
        _isSpitting = true;
        cuspindo = true;

        float duracao = Mathf.Max(0f, tempoCuspindo);

        float momentoAtaque = Mathf.Clamp(
            tempoParaAtacar,
            0f,
            duracao
        );

        if (momentoAtaque > 0f)
            yield return new WaitForSeconds(momentoAtaque);

        ExecutarAtaqueCuspe();

        float tempoRestante = duracao - momentoAtaque;

        if (tempoRestante > 0f)
            yield return new WaitForSeconds(tempoRestante);

        cuspindo = false;
        _isSpitting = false;
    }

    private void ExecutarAtaqueCuspe()
    {
        if (prefabCuspe == null || pontoCuspe == null)
        {
            Debug.LogWarning(
                "Prefab do cuspe ou ponto de disparo não configurado!"
            );
            return;
        }

        float direcao = Mathf.Sign(_attackDirection.x);

        Vector3 offset = pontoCuspe.position - transform.position;

        offset.x = Mathf.Abs(offset.x) * direcao;

        Vector3 posicaoDisparo = transform.position + offset;

        GameObject cuspe = Instantiate(
            prefabCuspe,
            posicaoDisparo,
            Quaternion.identity
        );

        Rigidbody2D rbCuspe = cuspe.GetComponent<Rigidbody2D>();

        if (rbCuspe != null)
        {
            rbCuspe.linearVelocity = new Vector2(
                direcao * velocidadeCuspe,
                0f
            );
        }
        else
        {
            Debug.LogWarning(
                "O prefab do cuspe não possui Rigidbody2D!"
            );
        }

        SpriteRenderer spriteCuspe =
            cuspe.GetComponent<SpriteRenderer>();

        if (spriteCuspe != null)
            spriteCuspe.flipX = direcao < 0f;
    }

    public void RecuperarVida(int quantidade)
    {
        _currentLP += quantidade;

        if (_currentLP > maxLP)
            _currentLP = maxLP;

        if (healthBar != null)
            healthBar.SetHealth(_currentLP);
    }

    public void TakeDamage(
        int amount,
        bool applyKnockback = true,
        bool ignoreCooldown = false)
    {
        if (_currentLP <= 0 || amount <= 0)
            return;

        if (!ignoreCooldown &&
            Time.time - _lastDamageTime < damageCooldown)
            return;

        _lastDamageTime = Time.time;

        _currentLP -= amount;

        if (_currentLP < 0)
            _currentLP = 0;

        if (healthBar != null)
            healthBar.SetHealth(_currentLP);

        if (applyKnockback && amount > 0)
            ApplyDamageKnockback();

        KillPlayer();
    }

    private void ApplyDamageKnockback()
    {
        if (_rigidbody2D == null)
            return;

        Vector2 direction = _lastMoveDirection;

        if (direction == Vector2.zero)
        {
            if (_rigidbody2D.linearVelocity.x != 0)
            {
                direction = new Vector2(
                    Mathf.Sign(_rigidbody2D.linearVelocity.x),
                    0f
                );
            }
            else
            {
                direction = _spriteRenderer != null &&
                            _spriteRenderer.flipX
                    ? Vector2.left
                    : Vector2.right;
            }
        }

        Vector2 knockbackDirection = new Vector2(
            -direction.x,
            knockbackUpwardFactor
        ).normalized;

        _rigidbody2D.linearVelocity =
            knockbackDirection * knockbackForce;
    }

    private void KillPlayer()
    {
        if (_currentLP <= 0 && !_isReloading)
        {
            _isReloading = true;

            if (_rigidbody2D != null)
            {
                _rigidbody2D.linearVelocity = new Vector2(
                    0f,
                    _rigidbody2D.linearVelocity.y
                );
            }

            SceneManager.LoadScene(
                SceneManager.GetActiveScene().name
            );
        }
    }

    private bool EhChao(Collision2D collision)
    {
        return collision.gameObject.CompareTag("chao") ||
               collision.gameObject.GetComponent<PlataformaMovel>() != null;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (EhChao(collision))
        {
            foreach (ContactPoint2D contato in collision.contacts)
            {
                if (contato.normal.y > 0.5f)
                {
                    noChao = true;
                    return;
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (EhChao(collision))
        {
            foreach (ContactPoint2D contato in collision.contacts)
            {
                if (contato.normal.y > 0.5f)
                {
                    noChao = true;
                    _hasJumped = false;
                    return;
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (EhChao(collision))
            noChao = false;
    }

    private void AtualizarAnimacoes()
    {
        if (_animator == null)
            return;

        _animator.SetBool("Cuspindo", cuspindo);

        _animator.SetBool(
            "Andando",
            andando && !cuspindo
        );

        _animator.SetBool(
            "Pulando",
            !noChao && !cuspindo
        );
    }

    void Update()
    {
        if (_currentLP <= 0)
        {
            KillPlayer();
            return;
        }

        andando = false;

        if (Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(
                -velocidade * Time.deltaTime,
                0f,
                0f
            );

            if (_spriteRenderer != null)
                _spriteRenderer.flipX = true;

            _lastMoveDirection = Vector2.left;

            if (noChao)
                andando = true;
        }

        if (Input.GetKey(KeyCode.RightArrow) ||
            Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(
                velocidade * Time.deltaTime,
                0f,
                0f
            );

            if (_spriteRenderer != null)
                _spriteRenderer.flipX = false;

            _lastMoveDirection = Vector2.right;

            if (noChao)
                andando = true;
        }

        if ((Input.GetKeyDown(KeyCode.UpArrow) ||
             Input.GetKeyDown(KeyCode.W) ||
             Input.GetKeyDown(KeyCode.Space)) &&
            noChao)
        {
            _hasJumped = true;

            if (_rigidbody2D != null)
            {
                _rigidbody2D.AddForce(
                    Vector2.up * forcaPulo,
                    ForceMode2D.Impulse
                );
            }

            if (somPulo != null && _audioSource != null)
                _audioSource.PlayOneShot(somPulo);

            Debug.Log("Jump");
        }

        if (Input.GetKeyDown(KeyCode.Z) ||
            Input.GetKeyDown(KeyCode.L))
        {
            Cuspir();
        }

        AtualizarAnimacoes();
    }
}
