using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 2 hit armor. После снятия армора входит в ярость. Анимация перехода составляет 4 секунды.
// Мотивирует игрока контролить инженеров и убирать их с линии.
// Если по нему попадает инженер он скастует ярость на краю карты в сохранности, потому что его защищает пушечное мясо
// и будет больно, но если же он будет впереди и на нём буду сфокусированны маги, то долгая анимация в 4 секунды даст
// преимущество перед берсерком.
public class ZombieBerserker : Enemy
{
    [SerializeField] private int _armor = 2;
    [SerializeField] private bool _isEnraged;

    public override void TakeDamage(int damage)
    {
        if (_armor > 0)
        {
            _armor -= 1;
        }
        else
        {
            if (!_isEnraged)
            {
                StartCoroutine(Enrage());
            }

            base.TakeDamage(damage);
        }
    }

    private IEnumerator Enrage()
    {
        base.CanMove = false;
        _isEnraged = true;
        // Анимация перехода в состояние ярости. В это время берсерк стоит на месте.
        yield return new WaitForSeconds(4);
        base.CanMove = true;
    }
}
