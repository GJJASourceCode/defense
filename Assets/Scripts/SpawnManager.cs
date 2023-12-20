using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnManager : MonoBehaviour
{
    public List<List<bool>> isBuilt;
    public Tilemap ground;
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

    public GameObject SpawnObject(Vector3Int tileIntPos, GameObject prefab)
    {
        if (isBuilt[tileIntPos.x - cellBounds.xMin][tileIntPos.y - cellBounds.yMin])
        {
            return null;
        }
        isBuilt[tileIntPos.x - cellBounds.xMin][tileIntPos.y - cellBounds.yMin] = true;

        var tilePos = ground.CellToWorld(tileIntPos + Vector3Int.up + Vector3Int.right);

        return Instantiate(prefab, new Vector3(tilePos.x, tilePos.y, -3f), Quaternion.identity);
    }

    private List<Vector3Int> GetSpawnablePos()
    {
        var spawnablePos = new List<Vector3Int>();
        for (int i = 0; i < cellBounds.xMax - cellBounds.xMin; i++)
        {
            for (int j = 0; j < cellBounds.yMax - cellBounds.yMin; j++)
            {
                if (
                    !isBuilt[i][j]
                    && ground.GetTile(new Vector3Int(i + cellBounds.xMin, j + cellBounds.yMin, 0))
                        != null
                )
                {
                    spawnablePos.Add(new Vector3Int(i + cellBounds.xMin, j + cellBounds.yMin, 0));
                }
            }
        }
        return spawnablePos;
    }

    public GameObject SpawnObjectAtRandomPos(GameObject prefab)
    {
        var spawnablePos = GetSpawnablePos();
        if (spawnablePos.Count == 0)
        {
            return null;
        }
        var random = spawnablePos[Random.Range(0, spawnablePos.Count)];
        return SpawnObject(random, prefab);
    }
}
