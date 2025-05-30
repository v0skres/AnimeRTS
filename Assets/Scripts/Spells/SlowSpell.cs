using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowSpell : MonoBehaviour, ISpell
{
    [Header("Cost Settings")]
    [SerializeField] private int _manaCost = 7;
    public int ManaCost => _manaCost;

    [Header("Slow Settings")]
    public float radius = 4f;
    public float slowFactor;
    public float duration = 5f;

    [Header("Visual Effects")]
    public ParticleSystem slowEffect;
    public Color slowColor = Color.cyan;

    private List<Enemy> _affectedEnemies = new List<Enemy>();

    public void Activate()
    {
        ApplySlowToEnemies();

        if (slowEffect != null)
        {
            Instantiate(slowEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject, 2f);
    }

    private void ApplySlowToEnemies()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out Enemy enemy))
            {
                if (!_affectedEnemies.Contains(enemy))
                {
                    _affectedEnemies.Add(enemy);
                    enemy.ApplySlow(slowFactor, duration);
                }
            }
        }
    }

    private void OnDestroy()
    {
        // На случай если объект уничтожается досрочно
        foreach (var enemy in _affectedEnemies)
        {
            if (enemy != null)
            {
                enemy.ResetSlow();
            }
        }
    }

    private void Start()
    {
        Activate();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}