using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackLineTower : MonoBehaviour
{
    //FIELDS
    public int health;
    public int cost;
    //damage
    public int damage;
    //prefab (shooting item)
    public GameObject prefab_laserBeam;
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
        ShootLaser();
        StartCoroutine(ShootDelay());
    }
    //Shoot an item
    void ShootLaser()
    {
        //Instantiate shoot item
        GameObject shotLaser = Instantiate(prefab_laserBeam, transform);
        shotLaser.GetComponent<LaserBeam>().Init(damage);
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
        Debug.Log("AttackLineTower is Dead");
        Destroy(gameObject);
    }
}
