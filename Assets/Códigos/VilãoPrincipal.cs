using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vidadovilao : MonoBehaviour
{
    public Barradevida healthBar;
    public int maxLP = 100;
    public int damage;

    private bool _isAlive;
    private int _currentLP;
    private Slider _mySlider;
    private static readonly int DieHash =
        Animator.StringToHash("Die");
    private Animator _animator;
    private Collider2D _collider2D;

    void Start()
    {
        _isAlive = true;
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();
    }
    private void Awake()
    {
        _mySlider = GetComponent<Slider>();
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
        if (!_isAlive)
            return;

        if (damage <= 0)
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

        if (_animator != null)
        {
            _animator.SetTrigger(DieHash);
        }

        if (_collider2D != null)
            _collider2D.enabled = false;

        else
            Destroy(gameObject);
    }
}