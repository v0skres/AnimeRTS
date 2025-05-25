using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public virtual float Speed
    {
        get => _speed;
        protected set => _speed = value;
    }
    [SerializeField] private float _speed;

    public int Damage
    {
        get => _damage;
        protected set => _damage = value;
    }
    [SerializeField] private int _damage;

    public float AttackSpeed
    {
        get => _attackSpeed;
        protected set => _attackSpeed = value;
    }
    [SerializeField] private float _attackSpeed = 1;

    public int AttackRange
    {
        get => _attackRange;
        protected set => _attackRange = value;
    }
    [SerializeField] private int _attackRange;

    public bool IsAlive
    {
        get => _isAlive;
        protected set => _isAlive = value;
    }
    [SerializeField] private bool _isAlive;

    public bool CanMove
    {
        get => _canMove;
        protected set => _canMove = value;
    }
    [SerializeField] private bool _canMove = true;

    public Line CurrentLine
    {
        get => _currentLine;
        set => _currentLine = value;
    }
    [SerializeField] private Line _currentLine;

    public int Weight
    {
        get => _weight;
        protected set => _weight = value;
    }
    [SerializeField] private int _weight;

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
        CurrentLine.Enemies.Remove(this);
        OnDeath(this, new EventArgs());
        Destroy(gameObject);
    }

    public virtual void Move()
    {
        if (IsAlive && CanMove)
        {
            var linePos = CurrentLine.PositionY + CurrentLine.LineOffset;
            transform.position = new Vector3(transform.position.x, linePos);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(-10000, linePos), Time.deltaTime * Speed);
        }
    }

    public virtual void Attack(ITower tower)
    {
        tower.LoseHealth(Damage);
    }

    protected virtual void OnDeath(object sender, EventArgs e)
    {
        Death?.Invoke(this, e);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out ITower tower))
        {
            CanMove = false;

            StartCoroutine(AttackCoroutine(tower));
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out ITower tower))
        {
            CanMove = true;

            StopCoroutine(AttackCoroutine(tower));
        }
    }

    protected IEnumerator AttackCoroutine(ITower tower)
    {
        while (true)
        {
            Attack(tower);
            yield return new WaitForSeconds(AttackSpeed);
        }
    }
}
