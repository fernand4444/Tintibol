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

    public float velocidade = 10f;
    public float forcaPulo = 10f;

    public bool noChao = false;
    public bool andando = false;

    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _animator = gameObject.GetComponent<Animator>();

        _currentLP = maxLP;
        if (healthBar != null)
        {
            healthBar.Initialize(maxLP, _currentLP);
        }
    }

     private void KillPlayer()
    {
        if (_currentLP <= 0)
        {

            //_rigidbody2D.bodyType = RigidbodyType2D.Static;
            _rigidbody2D.linearVelocity = new Vector2(0, _rigidbody2D.linearVelocity.y);

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
       
        }
        
    }

    public void TakeDamage(int amount)
    {
        if (Time.time - _lastDamageTime < damageCooldown)
            return;

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

        KillPlayer();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "chao")
        {
            noChao = true;
        }

    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "chao")
        {
            noChao = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        andando = false;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            gameObject.transform.position += new Vector3(-velocidade * Time.deltaTime, 0, 0);
            _spriteRenderer.flipX = true;
            Debug.Log("LeftArrow");

            if (noChao == true)
            {
                andando = true;
            }
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            gameObject.transform.position += new Vector3(velocidade * Time.deltaTime, 0, 0);
            _spriteRenderer.flipX = false;
            Debug.Log("RightArrow");

            if (noChao == true)
            {
                andando = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && noChao == true)
        {
            _rigidbody2D.AddForce(new Vector2(0, 1) * forcaPulo, ForceMode2D.Impulse);
            Debug.Log("Jump");
        }
        
        if (_currentLP <= 0)
            {
                KillPlayer();
                return;
            }

        _animator.SetBool("Andando", noChao);
        _animator.SetBool("Pulando", noChao);
    }
}
