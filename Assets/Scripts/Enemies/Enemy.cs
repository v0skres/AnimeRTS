using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    public static event Action<Enemy> OnEnemyDeath;
    [SerializeField] private int manaReward = 5;
    private float _originalSpeed;
    private Color _originalColor;
    private float _originalDrag;
    private bool _isSlowed = false;

    private bool _isPaused = false;
    private Vector2 _savedVelocity;

    protected Rigidbody2D rb;
    protected Collider2D enemyCollider;
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
        set => _speed = value;
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

    public EnemyType Type
    {
        get => _type;
        protected set => _type = value;
    }

    [SerializeField] private EnemyType _type;

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
        _originalSpeed = _speed;
        rb = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.isKinematic = false;
            rb.gravityScale = 0;
            rb.freezeRotation = true;
        }

        if (enemyCollider == null)
        {
            enemyCollider = gameObject.AddComponent<BoxCollider2D>();
        }
    }

    public void SetPaused(bool paused)
    {
        _isPaused = paused;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            if (paused)
            {
                _savedVelocity = rb.velocity;
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
            }
            else
            {
                rb.isKinematic = false;
                rb.velocity = _savedVelocity;
            }
        }

        // Для аниматоров
        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.enabled = !paused;
        }

        // Для частиц
        ParticleSystem ps = GetComponent<ParticleSystem>();
        if (ps != null)
        {
            if (paused) ps.Pause();
            else ps.Play();
        }
    }

    protected virtual void Update()
    {
        if (_isPaused) return;
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
        if (ManaSystem.instance != null)
        {
            ManaSystem.instance.Gain(manaReward);
        }
        else
        {
            Debug.LogWarning("ManaSystem instance not found!");
        }

        OnDeath(this, new EventArgs());
        Destroy(gameObject);
    }

    public virtual void Move()
    {
        if (IsAlive)
        {
            var linePos = CurrentLine.PositionY + CurrentLine.LineOffset;
            Vector2 targetPosition = new Vector2(-10000, linePos);

            // Используем физику для движения
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            rb.velocity = direction * Speed;
        }
    }

    public virtual void Attack(ITower tower)
    {
        tower.LoseHealth(Damage);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ProtectedObject"))
        {
            Debug.Log("Enemy reached protected object!");
            GameOver.Instance?.GameOver1();
            Destroy(gameObject); // Уничтожаем врага после достижения цели
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<DefenseTower>(out var tower))
        {
            rb.velocity = Vector2.zero;
        }
    }

    public void ApplySlow(float factor, float duration)
    {
        if (_isSlowed) return;

        _isSlowed = true;
        _speed = _originalSpeed * factor;

        var sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            _originalColor = sr.color;
            sr.color = Color.cyan;
        }

        if (TryGetComponent<Rigidbody2D>(out var rb))
        {
            _originalDrag = rb.drag;
            rb.drag = 10f;
        }

        StartCoroutine(ResetAfterDelay(duration));
    }

    public void ResetSlow()
    {
        if (!_isSlowed) return;

        _speed = _originalSpeed;
        _isSlowed = false;

        var sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = _originalColor;
        }

        if (TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.drag = _originalDrag;
        }
    }

    private IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetSlow();
    }

    protected IEnumerator AttackCoroutine(ITower tower)
    {
        while (true)
        {
            Attack(tower);
            yield return new WaitForSeconds(AttackSpeed);
        }
    }

    protected virtual void OnDeath(object sender, EventArgs e)
    {
        Death?.Invoke(this, e);
    }
}
