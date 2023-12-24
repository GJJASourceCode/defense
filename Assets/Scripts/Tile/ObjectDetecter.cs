using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectDetecter : MonoBehaviour
{
    [SerializeField]
    private TowerSpawner towerSpawner;

    [SerializeField]
    private Tilemap ground;
    private UIManager uiManager;
    private SpawnManager spawnManager;

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        spawnManager = FindObjectOfType<SpawnManager>();
    }

    private void Update()
    {
        if (towerSpawner.towerIndex < 8)
        {
            if (Input.GetMouseButtonDown(0)) //좌클
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int pos = ground.WorldToCell(
                    new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0)
                );
                if (ground.GetTile(pos) != null)
                {
                    if (towerSpawner.towerIndex != 3)
                        towerSpawner.SpawnTower(pos);
                    else if (towerSpawner.towerIndex == 3)
                        towerSpawner.TowerUpgrade(pos);
                }
            }
            else if (towerSpawner.towerIndex == 3)
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int pos = ground.WorldToCell(
                    new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0)
                );

                uiManager.UpgradeImage(pos);
            }
        }
    }
}
