using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Schema;
using TMPro.EditorUtilities;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;

    [SerializeField]
    private Tilemap ground;

    private SpawnManager spawnManager;
    private GameManager gameManager;
    private UIManager uiManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        spawnManager = FindObjectOfType<SpawnManager>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public void SpawnTower(Vector3Int tileIntPos)
    {
        var tower = spawnManager.SpawnObject(tileIntPos, towerPrefab);
        if (tower != null)
        {
            if (gameManager.money >= tower.GetComponent<Tower>().price)
            {
                tower.GetComponent<Tower>().tilePosition = tileIntPos;
                gameManager.money -= tower.GetComponent<Tower>().price;
                uiManager.UpdateMoney();
            }
            else
            {
                spawnManager.RemoveObject(tileIntPos);
            }
        }
    }
}
