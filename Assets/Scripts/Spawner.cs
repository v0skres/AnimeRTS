using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public SpriteRenderer testSprite;
    //list of towers (prefabs) that will instantiate
    public List<GameObject> towersPrefabs;
    //Transform of the spawning towers (Root Object)
    public Transform spawnTowerRoot;
    //list of towers (UI)
    public List<Image> towersUI;
    //id of tower to spawn
    int spawnID = -1;
    //SpawnPoints Tilemap
    public Tilemap spawnTilemap;
    //Tile position
    //private Vector3Int tilePos;


    void Update()
    {
        if (CanSpawn())
            DetectSpawnPoint();
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
        if (Input.GetMouseButtonDown(0))
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var cellPosDefault = spawnTilemap.WorldToCell(mousePos);
            var cellPosCentered = spawnTilemap.GetCellCenterWorld(cellPosDefault);
            if (spawnTilemap.GetColliderType(cellPosDefault) == Tile.ColliderType.Sprite)
            {
                SpawnTower(cellPosCentered);
                spawnTilemap.SetColliderType(cellPosDefault, Tile.ColliderType.None);

            }
  
        }
            

    }


    void SpawnTower(Vector3 position)
    {
        GameObject tower = Instantiate(towersPrefabs[spawnID], spawnTowerRoot);
        tower.transform.position = position;
        DeselectTowers();
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
}



