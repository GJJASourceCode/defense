using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnManager : MonoBehaviour
{
    public List<List<bool>> isBuilt;
    public Tilemap ground;
    private BoundsInt cellBounds;
    public List<List<GameObject>> building;


    void Awake()
    {
        cellBounds = ground.cellBounds;
        isBuilt = new List<List<bool>>();
        building = new List<List<GameObject>>();

        for (int i = 0; i <= cellBounds.xMax - cellBounds.xMin; i++)
        {
            var temp = new List<bool>();
            var temp2 = new List<GameObject>();
            for (int j = 0; j <= cellBounds.yMax - cellBounds.yMin; j++)
            {
                temp.Add(false);
                temp2.Add(null);
            }
            isBuilt.Add(temp);
            building.Add(temp2);
        }

    }

    public GameObject GetObject(Vector3Int tileIntPos)
    {
        if (tileIntPos.x<cellBounds.xMin||tileIntPos.x>cellBounds.xMax||tileIntPos.y<cellBounds.yMin||tileIntPos.y>cellBounds.yMax||!isBuilt[tileIntPos.x - cellBounds.xMin][tileIntPos.y - cellBounds.yMin])
        {
            return null;
        }
        return building[tileIntPos.x - cellBounds.xMin][tileIntPos.y - cellBounds.yMin];
    }

    public GameObject SpawnObject(Vector3Int tileIntPos, GameObject prefab)
    {
        if (tileIntPos.x<cellBounds.xMin||tileIntPos.x>cellBounds.xMax||tileIntPos.y<cellBounds.yMin||tileIntPos.y>cellBounds.yMax||isBuilt[tileIntPos.x - cellBounds.xMin][tileIntPos.y - cellBounds.yMin])
        {
            return null;
        }

        isBuilt[tileIntPos.x - cellBounds.xMin][tileIntPos.y - cellBounds.yMin] = true;
        // TODO: 건물 짓고 길 확인 후 없으면 부수는 걸로 알고리즘 수정
        var mobSpawner = FindObjectOfType<MobSpawner>();
        if (mobSpawner)
        {

            var pathFinding = FindObjectOfType<PathFinding>();
            var startPos = pathFinding.ground.WorldToCell(mobSpawner.transform.position - Vector3.up * 1f);
            var house = GameObject.Find("House");
            var targetPos = pathFinding.ground.WorldToCell(house.transform.position);

            var path = pathFinding.FindPath(startPos.x, startPos.y, targetPos.x, targetPos.y);
            if (path == null)
            {
                isBuilt[tileIntPos.x - cellBounds.xMin][tileIntPos.y - cellBounds.yMin] = false;
                return null;

            }
                
        }

        var tilePos = ground.CellToWorld(tileIntPos + Vector3Int.up + Vector3Int.right);
        building[tileIntPos.x - cellBounds.xMin][tileIntPos.y - cellBounds.yMin] =  Instantiate(prefab, new Vector3(tilePos.x, tilePos.y, -3f), Quaternion.identity);
        return building[tileIntPos.x - cellBounds.xMin][tileIntPos.y - cellBounds.yMin];
    }

    private List<Vector3Int> GetSpawnablePos()
    {
        var spawnablePos = new List<Vector3Int>();
        for (int i = 0; i <= cellBounds.xMax - cellBounds.xMin; i++)
        {
            for (int j = 0; j <= cellBounds.yMax - cellBounds.yMin; j++)
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

    public GameObject SpawnObjectAtEdge(GameObject prefab)
    {
        Vector3Int currentPos = Vector3Int.zero;
        for (int i = 0; i <= cellBounds.xMax - cellBounds.xMin; i++)
        {
            for (int j = 0; j <= cellBounds.yMax - cellBounds.yMin; j++)
            {
                if (
                    !isBuilt[i][j]
                    && ground.GetTile(new Vector3Int(i + cellBounds.xMin, j + cellBounds.yMin, 0))
                        != null
                )
                {
                    currentPos = new Vector3Int(i + cellBounds.xMin, j + cellBounds.yMin, 0);
                    break;
                }
            }
            if (currentPos != Vector3Int.zero) break;
        }
        var spawnablePos = new List<Vector3Int>();
        spawnablePos.Add(currentPos);

        while (true)
        {
            if (ground.GetTile(new Vector3Int(currentPos.x+1, currentPos.y-1, 0)) != null)
            {
            currentPos = new Vector3Int(currentPos.x +1 , currentPos.y-1, currentPos.z);
            spawnablePos.Add(currentPos);
            }
            else{
                break;
            }
        }
        while (true)
        {
            if (ground.GetTile(new Vector3Int(currentPos.x+1, currentPos.y+1, 0)) != null)
            {
            currentPos = new Vector3Int(currentPos.x +1 , currentPos.y+1, currentPos.z);
            spawnablePos.Add(currentPos);
            }
            else{
                break;
            }
        }
        while (true)
        {
            if (ground.GetTile(new Vector3Int(currentPos.x-1, currentPos.y+1, 0)) != null)
            {
            currentPos = new Vector3Int(currentPos.x-1 , currentPos.y+1, currentPos.z);
            spawnablePos.Add(currentPos);
            }
            else{
                break;
            }
        }
        while (true)
        {
            if (ground.GetTile(new Vector3Int(currentPos.x-1, currentPos.y-1, 0)) != null)
            {
            currentPos = new Vector3Int(currentPos.x -1 , currentPos.y-1, currentPos.z);
            spawnablePos.Add(currentPos);
            }
            else{
                break;
            }
        }
        Debug.Log(spawnablePos.Count);
        var random = spawnablePos[Random.Range(0, spawnablePos.Count)];
        return SpawnObject(random, prefab);

    }

    public void RemoveObject(Vector3Int tileIntPos)
    {
        var b = building[tileIntPos.x - cellBounds.xMin][tileIntPos.y - cellBounds.yMin];
        building[tileIntPos.x - cellBounds.xMin][tileIntPos.y - cellBounds.yMin] = null;
        isBuilt[tileIntPos.x - cellBounds.xMin][tileIntPos.y - cellBounds.yMin] = false;
        Destroy(b);
    }
}
