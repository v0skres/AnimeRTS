using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseTower : MonoBehaviour, ITower
{
    [SerializeField] private int _health = 10;  // Сериализуемое поле
    [SerializeField] private int _cost = 3;
    public int contactDamage = 1; // Урон при столкновении с врагом
    public float damageCooldown = 1f; // Задержка между получением урона

    private float lastDamageTime;
    private Rigidbody2D rb;
    private Collider2D towerCollider;

    public Color buffColor = Color.yellow;
    private Color _originalColor;

    private float _damageMultiplier = 1f;
    private Coroutine _buffCoroutine;

    public int health
    {
        get => _health;
        set => _health = value;
    }

    public int cost => _cost; // Readonly свойство

    private void Start()
    {
        _originalColor = GetComponent<SpriteRenderer>().color;
        rb = GetComponent<Rigidbody2D>();
        towerCollider = GetComponent<Collider2D>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.isKinematic = true; // Башня не должна двигаться
            rb.freezeRotation = true;
        }

        if (towerCollider == null)
        {
            towerCollider = gameObject.AddComponent<BoxCollider2D>();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Time.time - lastDamageTime < damageCooldown) return;

        if (collision.gameObject.TryGetComponent(out IEnemy enemy))
        {
            enemy.TakeDamage(contactDamage);
            LoseHealth(1);
            lastDamageTime = Time.time;
        }
    }

    public void ApplyDamageBuff(float multiplier, float duration)
    {
        // Отменяем предыдущий бафф если есть
        if (_buffCoroutine != null)
        {
            StopCoroutine(_buffCoroutine);
        }

        _damageMultiplier = multiplier;
        GetComponent<SpriteRenderer>().color = buffColor;

        //_buffCoroutine = StartCoroutine(BuffDuration(duration));

        //Debug.Log($"Damage buff applied: {Damage} damage (x{multiplier})");
    }

    public void ResetDamageBuff()
    {
        _damageMultiplier = 1f;
        GetComponent<SpriteRenderer>().color = _originalColor;
        Debug.Log("Damage buff ended");
    }

    //private IEnumerator BuffDuration(float delay)
    //{
    //yield return new WaitForSeconds(delay);
    //ResetDamageBuff();
    //}

    public void LoseHealth(int damage)
    {
        health -= damage;
        Debug.Log("Башня получила урон: " + damage + ". Осталось здоровья: " + health);

        if (health <= 0)
        {
            Die();
        }
    }


    public void Die()
    {
        Debug.Log("DefenseTower is Dead");
        Destroy(gameObject);
    }
}
