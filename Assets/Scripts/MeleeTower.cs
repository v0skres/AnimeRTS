using System.Collections;
using UnityEngine;

public class MeleeTower : MonoBehaviour, ITower
{
    [Header("Attack Settings")]
    public float attackRange = 1.5f;
    public float attackRate = 1f;
    [SerializeField] private int _health = 7;  // Сериализуемое поле
    [SerializeField] private int _cost = 3;
    public int damage = 1;
    [SerializeField] private LayerMask enemyLayer; // Будем использовать Default слой

    [SerializeField] private int _baseDamage = 1;
    private float _damageMultiplier = 1f;
    private Coroutine _buffCoroutine;

    public int health
    {
        get => _health;
        set => _health = value;
    }

    public int cost => _cost; // Readonly свойство

    public Color buffColor = Color.yellow;
    private Color _originalColor;

    public int Damage => Mathf.RoundToInt(_baseDamage * _damageMultiplier);

    private void Start()
    {
        _originalColor = GetComponent<SpriteRenderer>().color;
        // Автоматически настраиваем маску для Default слоя
        enemyLayer = LayerMask.GetMask("Default");
        StartCoroutine(AttackRoutine());

        Debug.Log("Настройки обнаружения:");
        Debug.Log($"Слой для поиска: {LayerMask.LayerToName(enemyLayer)}");
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackRate);
            DetectEnemies();
        }
    }

    void DetectEnemies()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        if (hits.Length == 0)
        {
            Debug.Log("Врагов не найдено. Проверьте:");
            Debug.Log($"- Дистанцию (радиус: {attackRange})");
            Debug.Log($"- Наличие Collider2D у врагов");
            Debug.Log($"- Слой врагов: {LayerMask.LayerToName(enemyLayer)}");
            return;
        }

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out IEnemy enemy))
            {
                enemy.TakeDamage(damage);
                Debug.Log($"Атакован {hit.name}", hit.gameObject);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, attackRange);
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

        _buffCoroutine = StartCoroutine(BuffDuration(duration));

        Debug.Log($"Damage buff applied: {Damage} damage (x{multiplier})");
    }

    public void ResetDamageBuff()
    {
        _damageMultiplier = 1f;
        GetComponent<SpriteRenderer>().color = _originalColor;
        Debug.Log("Damage buff ended");
    }

    private IEnumerator BuffDuration(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetDamageBuff();
    }

    public void LoseHealth(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("MeleeTower is Dead");
        Destroy(gameObject);
    }
}