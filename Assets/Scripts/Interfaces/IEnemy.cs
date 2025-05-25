public interface IEnemy
{
    int CurrentHealth { get; }
    int MaxHealth { get; }
    float Speed { get; }
    int Weight { get; }
    int Damage { get; }
    float AttackSpeed { get; }
    int AttackRange { get; }
    bool IsAlive { get; }
    bool CanMove {  get; }

    void Attack(ITower tower);
    void Die();
    void Move();
    void TakeDamage(int damage);
}