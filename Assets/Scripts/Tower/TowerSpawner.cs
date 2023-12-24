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
    public List<GameObject> towerPrefab;

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
        for (int i = 0; i < towerPrefab.Count; i++)
        {
            prices.Add(towerPrefab[i].GetComponent<Tower>().price);
        }
    }

    void OnDisable()
    {
        for (int i = 0; i < towerPrefab.Count; i++)
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
                int towerNum = towerIndex;
                tower.GetComponent<Tower>().tilePosition = tileIntPos;
                gameManager.money -= tower.GetComponent<Tower>().price;
                towerPrefab[towerIndex].GetComponent<Tower>().price += 1;

                uiManager.UpdateMoney();
                uiManager.UpdatePrice(towerNum);
            }
            else
            {
                spawnManager.RemoveObject(tileIntPos);
            }
        }
    }

    public void TowerUpgrade(Vector3Int tileIntPos)
    {
        var tower = spawnManager.GetObject(tileIntPos);
        if (tower != null)
            Debug.Log("Upgrade");
    }
}
