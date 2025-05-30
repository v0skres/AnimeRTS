public interface IEnemy
{
    int CurrentHealth { get; }
    int MaxHealth { get; }
    float Speed { get; set; }
    int Weight { get; }
    int Damage { get; }
    float AttackSpeed { get; }
    int AttackRange { get; }
    bool IsAlive { get; }
    bool CanMove {  get; }

    EnemyType Type { get; }

    void Attack(ITower tower);
    void Die();
    void Move();
    void TakeDamage(int damage);
}