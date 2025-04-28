using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTower : MonoBehaviour
{
    //FIELDS
    public int health;
    public int cost;
    //damage
    public int damage;
    //prefab (shooting item)
    public GameObject prefab_shootItem;
    //shoot interval
    public float interval;


    //METHODS
    //init (start the shooting interval)
    void Start()
    {
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

    public void LoseHealth()
    {
        health--;

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