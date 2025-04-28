using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseTower : MonoBehaviour
{
    //FIELDS
    public int health;
    public int cost;

    private void Start()
    {

    }

    public void LoseHealth()
    {
        health--;

        if (health <= 0)
        {
            Die();
        }
    }


    void Die()
    {
        Debug.Log("DefenseTower is Dead");
        Destroy(gameObject);
    }
}
