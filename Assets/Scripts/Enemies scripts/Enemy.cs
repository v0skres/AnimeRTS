using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    public int MaxHealth
    {
        get => _maxHealth;
        protected set => _maxHealth = value;
    }

    [SerializeField] private int _maxHealth;

    public int CurrentHealth
    {
        get => _currentHealth;
        protected set => _currentHealth = value;
    }

    [SerializeField] private int _currentHealth;

    public float Speed
    {
        get => _speed;
        protected set => _speed = value;
    }

    [SerializeField] private float _speed;

    public bool IsAlive
    {
        get => _isAlive;
        protected set => _isAlive = value;
    }

    [SerializeField] private bool _isAlive;

    public float CurrentLine
    {
        get => _currentLine;
        set => _currentLine = value;
    }

    [SerializeField] private float _currentLine;

    public event EventHandler Death;

    protected virtual void Start()
    {
        IsAlive = true;

    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {
        Move();
    }

    public virtual void TakeDamage(int damage)
    {
        if (damage > CurrentHealth)
        {
            Die();
        }
        else
        {
            CurrentHealth -= damage;
        }
    }

    public virtual void Die()
    {
        CurrentHealth = 0;
        IsAlive = false;
        OnDeath(this, new EventArgs());
        Destroy(gameObject);
    }

    public virtual void Move()
    {
        if (IsAlive)
        {
            transform.position = new Vector3(transform.position.x, CurrentLine);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(-10000, CurrentLine), Time.deltaTime * Speed);
        }
    }

    protected virtual void OnDeath(object sender, EventArgs e)
    {
        Death?.Invoke(this, e);
    }
}
