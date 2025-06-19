using UnityEngine;

public class DamageSpell : MonoBehaviour, ISpell
{
    [SerializeField] private int _manaCost = 5; // Приватное поле с сериализацией
    [SerializeField] private float _cooldownTime = 10f; // Время перезарядки в секундах
    public float radius = 3f;
    public int damage = 5;

    // Реализация свойства из интерфейса
    public int ManaCost => _manaCost;
    public float CooldownTime => _cooldownTime;

    public void Activate()
    {
        // Находим всех врагов в радиусе
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out IEnemy enemy))
            {
                enemy.TakeDamage(damage);
            }
        }

        // Визуальные эффекты и т.д.
        Destroy(gameObject, 2f); // Уничтожаем через 2 секунды
    }

    private void Start()
    {
        Activate();
    }
}