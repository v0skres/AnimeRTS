using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackLineTower : MonoBehaviour, ITower
{
    [SerializeField] private int _health = 5;  // Сериализуемое поле
    [SerializeField] private int _cost = 4;
    public float interval;

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

    public GameObject prefab_laserBeam;
    public Color buffColor = Color.yellow;
    private Color _originalColor;


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
        ShootLaser();
        StartCoroutine(ShootDelay());
    }
    //Shoot an item
    void ShootLaser()
    {
        //Instantiate shoot item
        GameObject shotLaser = Instantiate(prefab_laserBeam, transform);
        shotLaser.GetComponent<LaserBeam>().Init(Damage);
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
        Debug.Log("AttackLineTower is Dead");
        Destroy(gameObject);
    }
}
