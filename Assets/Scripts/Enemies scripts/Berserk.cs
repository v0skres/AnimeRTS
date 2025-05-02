using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserk : Enemy
{
    // Игнорирует первый полученный урон. Скорость быстрая
    [SerializeField] private bool _hasTakenDamage;

    public override void TakeDamage(int damage)
    {
        if (_hasTakenDamage)
        {
            base.TakeDamage(damage);
        }
        else
        {
            _hasTakenDamage = true;
        }
    }
}
