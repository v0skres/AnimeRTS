public interface ITower
{
    int health { get; set; }
    int cost { get; }
    void Die();
    void LoseHealth(int damage);
    void ApplyDamageBuff(float multiplier, float duration);
    void ResetDamageBuff();
}