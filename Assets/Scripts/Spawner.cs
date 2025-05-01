using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Spawner : MonoBehaviour, IPointerClickHandler
{
    public List<GameObject> towersPrefabs;
    public Transform spawnTowerRoot;
    public List<Image> towersUI;
    public Tilemap spawnTilemap;
    private int spawnID = -1;
    private GameObject selectedTower; // Выбранная башня для перемещения

    void Update()
    {
        if (CanSpawn())
            DetectSpawnPoint();
        else
            DetectTowerMove(); // Обработка перемещения
    }

    void DetectTowerMove()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = spawnTilemap.WorldToCell(mousePos);
            Vector3 cellCenter = spawnTilemap.GetCellCenterWorld(cellPos);

            // Если кликнули по башне — выбираем её
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag("Tower"))
            {
                SelectTowerForMove(hit.collider.gameObject);
                return;
            }

            // Если башня выбрана и кликнули на новую клетку — перемещаем
            if (selectedTower != null && spawnTilemap.GetColliderType(cellPos) == Tile.ColliderType.Sprite)
            {
                MoveTower(cellCenter);
            }
        }
    }

    void SelectTowerForMove(GameObject tower)
    {
        selectedTower = tower;
        // Визуальная подсветка (например, измените цвет)
        tower.GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    void MoveTower(Vector3 newPos)
    {
        if (selectedTower == null) return;

        // Возвращаем коллайдер старой клетки
        Vector3Int oldCell = spawnTilemap.WorldToCell(selectedTower.transform.position);
        spawnTilemap.SetColliderType(oldCell, Tile.ColliderType.Sprite);

        // Ставим башню в центр новой клетки
        selectedTower.transform.position = newPos;

        // Отключаем коллайдер новой клетки
        Vector3Int newCell = spawnTilemap.WorldToCell(newPos);
        spawnTilemap.SetColliderType(newCell, Tile.ColliderType.None);

        // Снимаем выделение
        selectedTower.GetComponent<SpriteRenderer>().color = Color.white;
        selectedTower = null;
    }



    bool CanSpawn()
    {
        if (spawnID == -1)
            return false;
        else
            return true;
    }


    void DetectSpawnPoint()
    {
        //Detect when mouse is clicked (first touch clicked)
        if (Input.GetMouseButtonDown(0))
        {
            //get the world space postion of the mouse
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //get the position of the cell in the tilemap
            var cellPosDefault = spawnTilemap.WorldToCell(mousePos);
            //get the center position of the cell
            var cellPosCentered = spawnTilemap.GetCellCenterWorld(cellPosDefault);
            //check if we can spawn in that cell (collider)
            if (spawnTilemap.GetColliderType(cellPosDefault) == Tile.ColliderType.Sprite)
            {
                int towerCost = TowerCost(spawnID);
                //Check if currency is enough to spawn
                if (GameManager.instance.currency.EnoughCurrency(towerCost))
                {
                    //Use the amount of cost from the currency available
                    GameManager.instance.currency.Use(towerCost);
                    //Spawn the tower
                    SpawnTower(cellPosCentered);
                    //Disable the collider
                    spawnTilemap.SetColliderType(cellPosDefault, Tile.ColliderType.None);
                }
                else
                {
                    Debug.Log("Not Enough Currency");
                }
            }
        }
    }

    public int TowerCost(int id)
    {
        switch (id)
        {
            case 0: return towersPrefabs[id].GetComponent<MoneyTower>().cost;
            case 1: return towersPrefabs[id].GetComponent<DefenseTower>().cost;
            case 2: return towersPrefabs[id].GetComponent<AttackTower>().cost;
            case 3: return towersPrefabs[id].GetComponent<AttackLineTower>().cost;
            default: return -1;
        }
    }


    void SpawnTower(Vector3 position)
    {
        GameObject tower = Instantiate(towersPrefabs[spawnID], spawnTowerRoot);
        tower.transform.position = position;

        DeselectTowers();
    }


    public void RevertCellState(Vector3Int pos)
    {
        spawnTilemap.SetColliderType(pos, Tile.ColliderType.Sprite);
    }


    public void SelectTower(int id)
    {
        DeselectTowers();
        spawnID = id;
        towersUI[spawnID].color = Color.white;
    }


    public void DeselectTowers()
    {
        spawnID = -1;
        foreach (var t in towersUI)
        {
            t.color = new Color(0.5f, 0.5f, 0.5f);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}



