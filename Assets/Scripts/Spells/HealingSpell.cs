using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingSpell : MonoBehaviour, ISpell
{
    [SerializeField] private int _manaCost = 15;
    [SerializeField] private float _cooldownTime = 20f; // Время перезарядки в секундах
    public int healAmount = 5;

    public int ManaCost => _manaCost;
    public float CooldownTime => _cooldownTime;

    public void Activate()
    {
        var towers = FindObjectsOfType<MonoBehaviour>();

        foreach (var obj in towers)
        {
            if (obj is ITower tower)
            {
                tower.health += healAmount;
                Debug.Log($"Healed {obj.name}");
            }
        }

        Destroy(gameObject);
    }
}
