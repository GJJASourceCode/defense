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
    public List<List<bool>> isBuilt;

    [SerializeField]
    private Tilemap ground;

    private BoundsInt cellBounds;

    void Awake()
    {
        cellBounds = ground.cellBounds;

        isBuilt = new List<List<bool>>();

        for (int i = 0; i < cellBounds.xMax - cellBounds.xMin; i++)
        {
            var temp = new List<bool>();
            for (int j = 0; j < cellBounds.yMax - cellBounds.yMin; j++)
            {
                temp.Add(false);
            }
            isBuilt.Add(temp);
        }
    }

    public void SpawnTower(Vector3Int tileIntPos)
    {
        // Tile tile = tileTransform.GetComponent<Tile>();

        // if (tile.IsBuildTower == true) //건설됨이면 건설x
        // {
        //     return;
        // }

        // tile.IsBuildTower = true; //건설됨 설정

        if (isBuilt[tileIntPos.x - cellBounds.xMin][tileIntPos.y - cellBounds.yMin])
        {
            return;
        }
        isBuilt[tileIntPos.x - cellBounds.xMin][tileIntPos.y - cellBounds.yMin] = true;

        var tilePos = ground.CellToWorld(tileIntPos + Vector3Int.up + Vector3Int.right);

        Instantiate(towerPrefab, new Vector3(tilePos.x, tilePos.y, -3f), Quaternion.identity); //타워 건설
    }
}
