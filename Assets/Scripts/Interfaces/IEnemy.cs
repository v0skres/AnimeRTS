public interface IEnemy
{
    int CurrentHealth { get; }
    bool IsAlive { get; }
    int MaxHealth { get; }
    float Speed { get; }

    int Weight { get; }

    EnemyType Type { get; }

    void Die();
    void Move();
    void TakeDamage(int damage);
}