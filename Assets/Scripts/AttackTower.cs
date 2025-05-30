using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTower : MonoBehaviour, ITower 
{
    //FIELDS
    [SerializeField] private int _health = 5;  // Сериализуемое поле
    [SerializeField] private int _cost = 4;
    //damage
    public int damage;
    //prefab (shooting item)
    public GameObject prefab_shootItem;
    //shoot interval
    public float interval;

    public Color buffColor = Color.yellow;
    private Color _originalColor;


    [SerializeField] private int _baseDamage = 2;
    private float _damageMultiplier = 1f;
    private Coroutine _buffCoroutine;

    public int health
    {
        get => _health;
        set => _health = value;
    }

    public int cost => _cost; // Readonly свойство

    public int Damage => Mathf.RoundToInt(_baseDamage * _damageMultiplier);


    //METHODS
    //init (start the shooting interval)
    void Start()
    {
        _originalColor = GetComponent<SpriteRenderer>().color;
        StartCoroutine(ShootDelay());
    }
    //Interval for shooting
    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(interval);
        ShootItem();
        StartCoroutine(ShootDelay());
    }
    //Shoot an item
    void ShootItem()
    {
        //Instantiate shoot item
        GameObject shotItem = Instantiate(prefab_shootItem, transform);
        shotItem.GetComponent<ShootItem>().Init(damage);
    }

    public void ApplyDamageBuff(float multiplier, float duration)
    {
        // Отменяем предыдущий бафф если есть
        if (_buffCoroutine != null)
        {
            StopCoroutine(_buffCoroutine);
        }

        _damageMultiplier = multiplier;
        GetComponent<SpriteRenderer>().color = buffColor;

        _buffCoroutine = StartCoroutine(BuffDuration(duration));

        Debug.Log($"Damage buff applied: {Damage} damage (x{multiplier})");
    }

    public void ResetDamageBuff()
    {
        _damageMultiplier = 1f;
        GetComponent<SpriteRenderer>().color = _originalColor;
        Debug.Log("Damage buff ended");
    }

    private IEnumerator BuffDuration(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetDamageBuff();
    }

    public void LoseHealth(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("AttackTower is Dead");
        Destroy(gameObject);
    }
}