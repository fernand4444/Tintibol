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


    [Header("Sistema de Galhos")]
    public LayerMask camadaGalhos;
    public float velocidadeSoltarGalho = 1f;


    private bool _agarradoNoGalho = false;
    private float gravidadeAntesDoGalho;


    [Header("Ataque de Cuspe")]
    public GameObject prefabCuspe;
    public Transform pontoCuspe;
    public float tempoCuspindo = 1f;
    public float tempoParaAtacar = 0.3f;
    public float velocidadeCuspe = 10f;


    [Header("Natacao")]
    public float velocidadeNado = 5f;
    public float resistenciaAgua = 1f;
    public float gravidadeAfundando = 0.5f;


    private bool dentroDaAgua = false;
    private float gravidadeOriginal;
    private float resistenciaOriginal;
    private Vector2 movimentoNado;


    [Header("Efeito Visual da Agua")]
    public Color corNaAgua = new Color(0.45f, 0.75f, 1f, 1f);


    private SpriteRenderer[] todosSpriteRenderers;
    private Color[] coresOriginais;


    [Header("Audio")]
    public AudioClip somPulo;
    public AudioClip somDano;


    [Header("Efeito Visual de Dano")]
    public float tempoVermelho = 0.2f;
    private Coroutine efeitoDanoCoroutine;


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


        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }


        _audioSource.playOnAwake = false;


        todosSpriteRenderers =
            GetComponentsInChildren<SpriteRenderer>(true);


        coresOriginais = new Color[todosSpriteRenderers.Length];


        for (int i = 0; i < todosSpriteRenderers.Length; i++)
        {
            coresOriginais[i] = todosSpriteRenderers[i].color;
        }


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


    // =================================
    // SISTEMA DE GALHOS
    // =================================


    private bool EhGalho(Collision2D collision)
    {
        return ((1 << collision.gameObject.layer) & camadaGalhos) != 0;
    }


    private void AgarrarNoGalho()
    {
        if (_agarradoNoGalho || dentroDaAgua)
            return;


        _agarradoNoGalho = true;


        gravidadeAntesDoGalho = _rigidbody2D.gravityScale;


        _rigidbody2D.gravityScale = 0f;
        _rigidbody2D.linearVelocity = Vector2.zero;


        noChao = false;
        andando = false;
        _hasJumped = false;


        Debug.Log("Bolti se agarrou ao galho!");
    }


    private void ControlarGalho()
    {
        if (_rigidbody2D == null)
            return;


        // Mantém Bolti parado enquanto estiver agarrado.
        _rigidbody2D.linearVelocity = Vector2.zero;


        // Pular para se desgrudar.
        if (Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.UpArrow))
        {
            _agarradoNoGalho = false;


            _rigidbody2D.gravityScale = gravidadeAntesDoGalho;


            float horizontal = 0f;


            if (Input.GetKey(KeyCode.A) ||
                Input.GetKey(KeyCode.LeftArrow))
                horizontal = -1f;


            if (Input.GetKey(KeyCode.D) ||
                Input.GetKey(KeyCode.RightArrow))
                horizontal = 1f;


            Vector2 direcaoPulo = new Vector2(
                horizontal * velocidadeSoltarGalho,
                forcaPulo
            );


            _rigidbody2D.AddForce(
                direcaoPulo,
                ForceMode2D.Impulse
            );


            _hasJumped = true;


            if (somPulo != null && _audioSource != null)
                _audioSource.PlayOneShot(somPulo);


            Debug.Log("Bolti soltou do galho!");
        }
    }


    // =================================
    // SISTEMA DE AGUA
    // =================================


    public void EntrarNaAgua()
    {
        if (dentroDaAgua)
            return;


        if (_agarradoNoGalho)
        {
            _agarradoNoGalho = false;


            if (_rigidbody2D != null)
                _rigidbody2D.gravityScale = gravidadeAntesDoGalho;
        }


        dentroDaAgua = true;
        movimentoNado = Vector2.zero;


        if (_rigidbody2D != null)
        {
            gravidadeOriginal = _rigidbody2D.gravityScale;
            resistenciaOriginal = _rigidbody2D.linearDamping;


            _rigidbody2D.gravityScale = 0f;
            _rigidbody2D.linearDamping = resistenciaAgua;
            _rigidbody2D.linearVelocity = Vector2.zero;
        }


        noChao = false;
        _hasJumped = false;


        AplicarCorDaAgua();


        Debug.Log("Bolti entrou na agua!");
    }


    public void SairDaAgua()
    {
        if (!dentroDaAgua)
            return;


        dentroDaAgua = false;
        movimentoNado = Vector2.zero;


        if (_rigidbody2D != null)
        {
            _rigidbody2D.gravityScale = gravidadeOriginal;
            _rigidbody2D.linearDamping = resistenciaOriginal;
            _rigidbody2D.linearVelocity = Vector2.zero;
        }


        noChao = false;


        RestaurarCorOriginal();


        Debug.Log("Bolti saiu da agua!");
    }


    private void AplicarCorDaAgua()
    {
        if (todosSpriteRenderers == null)
            return;


        for (int i = 0; i < todosSpriteRenderers.Length; i++)
        {
            if (todosSpriteRenderers[i] != null)
            {
                todosSpriteRenderers[i].color = corNaAgua;
            }
        }
    }


    private void RestaurarCorOriginal()
    {
        if (todosSpriteRenderers == null ||
            coresOriginais == null)
            return;


        for (int i = 0; i < todosSpriteRenderers.Length; i++)
        {
            if (todosSpriteRenderers[i] != null)
            {
                todosSpriteRenderers[i].color = coresOriginais[i];
            }
        }
    }


    private void MovimentarNaAgua()
    {
        float horizontal = 0f;
        float vertical = 0f;


        if (Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.A))
            horizontal = -1f;


        if (Input.GetKey(KeyCode.RightArrow) ||
            Input.GetKey(KeyCode.D))
            horizontal = 1f;


        if (Input.GetKey(KeyCode.UpArrow) ||
            Input.GetKey(KeyCode.W))
            vertical = 1f;


        if (Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.S))
            vertical = -1f;


        movimentoNado = new Vector2(horizontal, vertical);


        if (movimentoNado.magnitude > 1f)
            movimentoNado.Normalize();


        if (horizontal < 0)
        {
            if (_spriteRenderer != null)
                _spriteRenderer.flipX = true;


            _lastMoveDirection = Vector2.left;
        }
        else if (horizontal > 0)
        {
            if (_spriteRenderer != null)
                _spriteRenderer.flipX = false;


            _lastMoveDirection = Vector2.right;
        }


        andando = movimentoNado.magnitude > 0.1f;
        noChao = false;


        if (Input.GetKeyDown(KeyCode.Z) ||
            Input.GetKeyDown(KeyCode.L))
        {
            Cuspir();
        }
    }


    void FixedUpdate()
    {
        if (!dentroDaAgua || _rigidbody2D == null)
            return;


        if (movimentoNado.magnitude > 0.1f)
        {
            _rigidbody2D.gravityScale = 1.5f;
            _rigidbody2D.linearDamping = resistenciaAgua;


            _rigidbody2D.linearVelocity =
                movimentoNado * velocidadeNado;
        }
        else
        {
            _rigidbody2D.gravityScale = gravidadeAfundando;
            _rigidbody2D.linearDamping = resistenciaOriginal;
        }
    }


    // =================================
    // ATAQUE DE CUSPE
    // =================================


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
                "Prefab do cuspe ou ponto de disparo nao configurado!"
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
                "O prefab do cuspe nao possui Rigidbody2D!"
            );
        }


        SpriteRenderer spriteCuspe =
            cuspe.GetComponent<SpriteRenderer>();


        if (spriteCuspe != null)
            spriteCuspe.flipX = direcao < 0f;
    }


    // =================================
    // VIDA E DANO
    // =================================


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


        AtivarEfeitoDano();


        if (applyKnockback && amount > 0)
            ApplyDamageKnockback();


        KillPlayer();
    }


    private void AtivarEfeitoDano()
    {
        if (somDano != null && _audioSource != null)
            _audioSource.PlayOneShot(somDano);


        if (efeitoDanoCoroutine != null)
            StopCoroutine(efeitoDanoCoroutine);


        efeitoDanoCoroutine = StartCoroutine(EfeitoVermelho());
    }


    private IEnumerator EfeitoVermelho()
    {
        for (int i = 0; i < todosSpriteRenderers.Length; i++)
        {
            if (todosSpriteRenderers[i] != null)
                todosSpriteRenderers[i].color = Color.red;
        }


        yield return new WaitForSeconds(tempoVermelho);


        for (int i = 0; i < todosSpriteRenderers.Length; i++)
        {
            if (todosSpriteRenderers[i] != null)
            {
                if (dentroDaAgua)
                    todosSpriteRenderers[i].color = corNaAgua;
                else
                    todosSpriteRenderers[i].color = coresOriginais[i];
            }
        }


        efeitoDanoCoroutine = null;
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


    // =================================
    // COLISOES E CHAO
    // =================================


    private bool EhChao(Collision2D collision)
    {
        return collision.gameObject.CompareTag("chao") ||
               collision.gameObject.GetComponent<PlataformaMovel>() != null;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (EhGalho(collision))
        {
            AgarrarNoGalho();
            return;
        }


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


    void OnCollisionStay2D(Collision2D collision)
    {
        if (EhGalho(collision))
        {
            if (!_agarradoNoGalho)
                AgarrarNoGalho();


            return;
        }


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


    void OnCollisionExit2D(Collision2D collision)
    {
        if (EhChao(collision))
            noChao = false;
    }


    // =================================
    // ANIMACOES
    // =================================


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
            !dentroDaAgua && !noChao && !cuspindo &&
            !_agarradoNoGalho
        );
    }


    // =================================
    // MOVIMENTACAO PRINCIPAL
    // =================================


    void Update()
    {
        if (_currentLP <= 0)
        {
            KillPlayer();
            return;
        }


        if (dentroDaAgua)
        {
            MovimentarNaAgua();
            AtualizarAnimacoes();
            return;
        }


        if (_agarradoNoGalho)
        {
            ControlarGalho();
            AtualizarAnimacoes();
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