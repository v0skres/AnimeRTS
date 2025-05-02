using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Line
{
    public int PositionY;

    public float LineOffset = 0.5f;

    public int MaxEnemiesWeight;

    public int EnemiesWeightSum => Enemies.Sum(x => x.Weight);

    public List<Enemy> Enemies = new List<Enemy>();
}

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> EnemyCollection = new List<GameObject>();
    public List<Line> Lines = new List<Line>();
    public int SpawnInterval;

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
            if (Lines.All(x => x.EnemiesWeightSum >= x.MaxEnemiesWeight))
            {
                Debug.Log("All lines taken");
                yield return new WaitForSeconds(SpawnInterval);
                continue;
            }

            foreach (var enemyPrefab in EnemyCollection)
            {
                var line = Lines[UnityEngine.Random.Range(0, Lines.Count)];
                Debug.Log(line.EnemiesWeightSum);
                if (line.EnemiesWeightSum >= line.MaxEnemiesWeight)
                {
                    continue;
                }

                var enemyComponent = Instantiate(enemyPrefab, new Vector3(7, line.PositionY + line.LineOffset), new Quaternion()).GetComponent<Enemy>();
                enemyComponent.CurrentLine = line;
                enemyComponent.Death += EnemyComponent_Death;
                line.Enemies.Add(enemyComponent);

                yield return new WaitForSeconds(SpawnInterval);
            }
        }
    }

    private void EnemyComponent_Death(object sender, EventArgs e)
    {
        if (sender is Enemy enemy)
        {
            enemy.CurrentLine.Enemies.Remove(enemy);
        }
    }
}
