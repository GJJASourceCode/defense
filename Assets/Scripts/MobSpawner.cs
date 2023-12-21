using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MobSpawner : MonoBehaviour
{
    public GameObject mobPrefab;
    public float spawnTime;

    private float timeforSpawn = 0f;

    public float showPathTime;
    private float timeforPath = 0f;

    public List<Mob> mobList;
    private PathFinding pathFinding;

    [SerializeField]
    private TileBase defaultTile;

    [SerializeField]
    private TileBase pathTile;

    private List<Node> path;

    void Awake()
    {
        mobList = new List<Mob>();
    }

    void Start()
    {
        pathFinding = FindObjectOfType<PathFinding>();
    }

    void Update()
    {
        timeforSpawn += Time.deltaTime;
        timeforPath += Time.deltaTime;

        if (timeforSpawn >= spawnTime)
        {
            timeforSpawn = 0f;

            var mob = Instantiate(mobPrefab, transform.position - Vector3.up*0.25f, Quaternion.identity);
            mobList.Add(mob.GetComponent<Mob>());
        }

        if (timeforPath >= showPathTime)
        {
            timeforPath = 0f;
            ShowPath();
        }
    }

    public void MobDie(Mob mob)
    {
        mobList.Remove(mob);
    }

    private void ShowPath()
    {
        var startPos = pathFinding.ground.WorldToCell(transform.position - Vector3.up * 1f);
        var house = GameObject.Find("House");
        var targetPos = pathFinding.ground.WorldToCell(house.transform.position);

        path = pathFinding.FindPath(startPos.x, startPos.y, targetPos.x, targetPos.y);

        for (int i = 0; i < path.Count; i++)
        {
            var tilePos = new Vector3Int(path[i].x, path[i].y, 0);
            pathFinding.ground.SetTile(tilePos, pathTile);
            StartCoroutine(ReplaceTiletoDefault());
        }
    }

    IEnumerator ReplaceTiletoDefault()
    {
        yield return new WaitForSeconds(showPathTime / 2);

        for (int i = 0; i < path.Count; i++)
        {
            var tilePos = new Vector3Int(path[i].x, path[i].y, 0);
            pathFinding.ground.SetTile(tilePos, defaultTile);
        }
    }
}
