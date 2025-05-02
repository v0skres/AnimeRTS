using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> EnemyCollection = new List<GameObject>();
    public Dictionary<float, List<Enemy>> Enemies = new Dictionary<float, List<Enemy>>();
    public float[] Lines = new float[4]
    {
        2.5f,
        3.5f,
        4.5f,
        5.5f
    };

    public void Init()
    {

    }

    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            float line = UnityEngine.Random.Range(0f, Lines.LastOrDefault());
            foreach (var enemyPrefab in EnemyCollection)
            {
                var enemyComponent = Instantiate(enemyPrefab, new Vector3(7, line), new Quaternion()).GetComponent<Enemy>();
                enemyComponent.CurrentLine = line;
                enemyComponent.Death += EnemyComponent_Death;
                if (Enemies.ContainsKey(line))
                {
                    Enemies[line].Add(enemyComponent);
                }
                else
                {
                    Enemies.Add(line, new List<Enemy> { enemyComponent });
                }

                line++;
                yield return new WaitForSeconds(5);
            }
        }
    }

    private void EnemyComponent_Death(object sender, EventArgs e)
    {
        if (sender is Enemy enemy && Enemies.ContainsKey(enemy.CurrentLine))
        {
            Enemies[enemy.CurrentLine].Remove(enemy);
        }
    }
}
