using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseTower : MonoBehaviour, ITower
{
    //FIELDS
    public int health;
    public int cost;

    private void Start()
    {

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
        Debug.Log("DefenseTower is Dead");
        Destroy(gameObject);
    }
}
