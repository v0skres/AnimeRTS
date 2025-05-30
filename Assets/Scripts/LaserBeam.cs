using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [Header("Combat Settings")]
    public int damage;
    public float flySpeed;
    public float rotateSpeed;

    [Header("Visuals")]
    public Transform graphics;

    //METHODS
    //Init
    public void Init(int damageAmount)
    {
        damage = damageAmount;
    }
    //Trigger with enemy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IEnemy enemy))
        {
            Debug.Log("Shot the enemy");
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
        if (collision.tag == "Out")
        {
            Destroy(gameObject);
        }
    }
    //Handle rotation and flying
    void Update()
    {
        Rotate();
        FlyForward();
    }
    void Rotate()
    {
        graphics.Rotate(new Vector3(0, 0, -rotateSpeed * Time.deltaTime));
    }
    void FlyForward()
    {
        transform.Translate(transform.right * flySpeed * Time.deltaTime, Space.World);
    }
}
