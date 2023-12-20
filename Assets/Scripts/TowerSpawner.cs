using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Schema;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;

    [SerializeField]
    private Tilemap ground;

    private BoundsInt cellBounds;
    private SpawnManager spawnManager;

    void Awake()
    {
        cellBounds = ground.cellBounds;
        spawnManager = FindObjectOfType<SpawnManager>();
    }

    public void SpawnTower(Vector3Int tileIntPos)
    {
        var tower = spawnManager.SpawnObject(tileIntPos, towerPrefab);
        if (tower != null)
            tower.GetComponent<Tower>().tilePosition = tileIntPos;
    }
}
