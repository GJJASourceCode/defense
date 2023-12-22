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
    private List<GameObject> towerPrefab;

    public int towerIndex;

    [SerializeField]
    private Tilemap ground;

    private SpawnManager spawnManager;
    private GameManager gameManager;
    private UIManager uiManager;

    private List<int> prices;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        spawnManager = FindObjectOfType<SpawnManager>();
        uiManager = FindObjectOfType<UIManager>();
        prices = new List<int>();
        for (int i = 0; i < towerPrefab.Count;i++)
        {
            prices.Add(towerPrefab[i].GetComponent<Tower>().price);
        }
    }

    void OnDisable() {
        for (int i = 0; i < towerPrefab.Count;i++)
        {
            towerPrefab[i].GetComponent<Tower>().price = prices[i];
        }
    }

    public void SpawnTower(Vector3Int tileIntPos)
    {
        
        var tower = spawnManager.SpawnObject(tileIntPos, towerPrefab[towerIndex]);
        if (tower != null)
        {
            if (gameManager.money >= tower.GetComponent<Tower>().price)
            {
                tower.GetComponent<Tower>().tilePosition = tileIntPos;
                gameManager.money -= tower.GetComponent<Tower>().price;
                towerPrefab[towerIndex].GetComponent<Tower>().price += 1;

                uiManager.UpdateMoney();
                
                
            }
            else
            {
                spawnManager.RemoveObject(tileIntPos);
            }
        }
    }
}
