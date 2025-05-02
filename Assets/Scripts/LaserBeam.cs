using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    //FIELDS
    //graphics (the sprite renderer)
    public Transform graphics;
    //damage
    public int damage;
    //speed
    public float flySpeed, rotateSpeed;

    //METHODS
    //Init
    public void Init(int dmg)
    {
        damage = dmg;
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
        transform.Translate(transform.right * flySpeed * Time.deltaTime);
    }
}
