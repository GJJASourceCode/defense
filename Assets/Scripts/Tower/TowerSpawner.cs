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
                if (tower.TryGetComponent<ArcherTower>(out ArcherTower a) == true)
                    towerPrefab[towerIndex].GetComponent<Tower>().price += 3;
                else if (tower.TryGetComponent<WizardTower>(out WizardTower w) == true)
                    towerPrefab[towerIndex].GetComponent<Tower>().price += 5;
                else if (tower.TryGetComponent<SlowTower>(out SlowTower s) == true)
                    towerPrefab[towerIndex].GetComponent<Tower>().price += 5;
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
        {
            if (tower.TryGetComponent<ArcherTower>(out ArcherTower a) == true)
            {
                if (a.level == 1 && gameManager.money >= towerPrefab[3].GetComponent<Tower>().price)
                {
                    spawnManager.RemoveObject(tileIntPos);
                    var newTower = spawnManager.SpawnObject(tileIntPos, towerPrefab[3]);
                    gameManager.money -= newTower.GetComponent<Tower>().price;
                    newTower.GetComponent<Tower>().tilePosition = tileIntPos;
                    uiManager.UpdateMoney();
                }
                else if (
                    a.level == 2 && gameManager.money >= towerPrefab[4].GetComponent<Tower>().price
                )
                {
                    spawnManager.RemoveObject(tileIntPos);
                    var newTower = spawnManager.SpawnObject(tileIntPos, towerPrefab[4]);
                    gameManager.money -= newTower.GetComponent<Tower>().price;
                    newTower.GetComponent<Tower>().tilePosition = tileIntPos;
                    uiManager.UpdateMoney();
                }
            }
            else if (tower.TryGetComponent<WizardTower>(out WizardTower w) == true)
            {
                if (w.level == 1 && gameManager.money >= towerPrefab[5].GetComponent<Tower>().price)
                {
                    spawnManager.RemoveObject(tileIntPos);
                    var newTower = spawnManager.SpawnObject(tileIntPos, towerPrefab[5]);
                    gameManager.money -= newTower.GetComponent<Tower>().price;
                    newTower.GetComponent<Tower>().tilePosition = tileIntPos;
                    uiManager.UpdateMoney();
                }
                else if (
                    w.level == 2 && gameManager.money >= towerPrefab[6].GetComponent<Tower>().price
                )
                {
                    spawnManager.RemoveObject(tileIntPos);
                    var newTower = spawnManager.SpawnObject(tileIntPos, towerPrefab[6]);
                    gameManager.money -= newTower.GetComponent<Tower>().price;
                    newTower.GetComponent<Tower>().tilePosition = tileIntPos;
                    uiManager.UpdateMoney();
                }
            }
            else if (
                tower.TryGetComponent<SlowTower>(out SlowTower s) == true
                && gameManager.money >= towerPrefab[7].GetComponent<Tower>().price
            )
            {
                spawnManager.RemoveObject(tileIntPos);
                var newTower = spawnManager.SpawnObject(tileIntPos, towerPrefab[7]);
                gameManager.money -= newTower.GetComponent<Tower>().price;
                newTower.GetComponent<Tower>().tilePosition = tileIntPos;
                uiManager.UpdateMoney();
            }
        }
    }
}
