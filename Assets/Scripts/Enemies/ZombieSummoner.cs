using System.Collections;
using System.Linq;
using UnityEngine;

// Призывает случайных 4 существ (набор существ зависит от уровня игры т.е. на 4 уровне не сможет призвать берсерка)
// игнорируя очки спавна.
// Имеет маленький урон, но благодаря своей особенности может доставить большие проблемы толпами врагов.
public class ZombieSummoner : Enemy
{
    public int SummonInterval = 10;

    public int SummonTime = 3;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(Summon());
    }

    public IEnumerator Summon()
    {
        while (IsAlive)
        {
            foreach (var enemy in EnemyManager.Instance.EnemyCollection.Where(x => !x.TryGetComponent<ZombieSummoner>(out _)))
            {
                var line = EnemyManager.Instance.Lines.First(x => x.EnemiesWeightSum == EnemyManager.Instance.Lines.Min(x => x.EnemiesWeightSum));
                CanMove = false;
                yield return new WaitForSeconds(SummonTime);
                CanMove = true;
                var enemyGO = EnemyManager.Instance.SpawnEnemy(enemy, line, new Vector3(transform.position.x, line.PositionY));

                // DEBUG
                enemyGO.GetComponent<SpriteRenderer>().color = Color.blue;
            }

            yield return new WaitForSeconds(SummonInterval);
        }
    }
}
