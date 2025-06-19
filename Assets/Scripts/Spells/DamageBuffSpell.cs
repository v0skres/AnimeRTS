using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBuffSpell : MonoBehaviour, ISpell
{
    [Header("Cost Settings")]
    [SerializeField] private int _manaCost = 10;
    [SerializeField] private float _cooldownTime = 5f; // Время перезарядки в секундах
    public int ManaCost => _manaCost;
    public float CooldownTime => _cooldownTime;

    [Header("Buff Settings")]
    public float radius = 5f;
    public float damageMultiplier = 1.5f; // 1.5 = +50% урона
    public float duration = 5f;

    [Header("Visual Effects")]
    public ParticleSystem buffEffect;
    public Color buffColor = Color.yellow;

    private List<ITower> _affectedTowers = new List<ITower>();

    public void Activate()
    {
        ApplyBuffToTowers();

        if (buffEffect != null)
        {
            Instantiate(buffEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject, 2f);
    }

    private void ApplyBuffToTowers()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out ITower tower))
            {
                if (!_affectedTowers.Contains(tower))
                {
                    _affectedTowers.Add(tower);
                    tower.ApplyDamageBuff(damageMultiplier, duration);
                }
            }
        }
    }

    private void OnDestroy()
    {
        foreach (var tower in _affectedTowers)
        {
            if (tower != null)
            {
                (tower as MonoBehaviour).StartCoroutine(RemoveBuffAfterDelay(tower, 0.1f));
            }
        }
    }

    private IEnumerator RemoveBuffAfterDelay(ITower tower, float delay)
    {
        yield return new WaitForSeconds(delay);
        tower.ResetDamageBuff();
    }

    private void Start()
    {
        Activate();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}