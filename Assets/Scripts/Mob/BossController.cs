using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private PathFinding pathFinding;
    private GameObject mobSpawner;
    private GameObject house;
    public float speed;

    private Vector3Int startPos;
    private Vector3Int targetPos;
    private Vector2 moveTarget;
    private List<Node> path = null;
    private Mob mob;
    private GameManager gameManager;
    private SpawnManager spawnManager;

    void OnEnable()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
        gameManager = FindObjectOfType<GameManager>();
        pathFinding = GameObject.Find("PathFinder").GetComponent<PathFinding>();
        // mobSpawner = GameObject.Find("MobSpawner");
        house = GameObject.Find("House");
        mob = GetComponent<Mob>();

        moveTarget = new Vector2(transform.position.x, transform.position.y);
        startPos = pathFinding.ground.WorldToCell(transform.position - Vector3.up * 0.75f);
        // FindObjectOfType<SpawnManager>().SpawnObject(startPos, testPrefab);
        targetPos = pathFinding.ground.WorldToCell(house.transform.position);

        path = pathFinding.FindPath(startPos.x, startPos.y, targetPos.x, targetPos.y);
        if (path == null)
        {
            Debug.Log("Can't Find Path");
        }
        else
        {
            mob.tilePosition = path[0];
            var temp = pathFinding.ground.CellToWorld(new Vector3Int(path[0].x, path[0].y, 0));
            moveTarget = new Vector2(temp.x, temp.y + 0.15f);
        }
    }

    void Update()
    {
        // TODO 몹 데미지 안 입음 수정 필요
        if (gameManager.isPaused)
            return;
        float step = speed * Time.deltaTime;
        var currentPos = pathFinding.ground.WorldToCell(
            transform.position + new Vector3(0, -0.15f, 0)
        );
        if (path != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, moveTarget, step);
            if (
                spawnManager.GetObject(currentPos) != null
                && !spawnManager.GetObject(currentPos).TryGetComponent(out MobSpawner a)
            )
            {
                spawnManager.RemoveObject(currentPos);
            }
        }
        if (Vector2.Distance(moveTarget, transform.position) <= 0.1f)
        {
            var graph = pathFinding.ground.GetComponent<Ground>().GetGraph();
            mob.tilePosition = graph[currentPos.x - pathFinding.ground.cellBounds.xMin][
                currentPos.y - pathFinding.ground.cellBounds.yMin
            ];
            // FindObjectOfType<SpawnManager>().SpawnObject(startPos, testPrefab);
            if (currentPos == targetPos) // 집 도착
            {
                gameManager.AttackHouse();
                Destroy(gameObject);
            }
            else
            {
                Vector3 temp;
                if (currentPos.x != targetPos.x)
                {
                    temp = pathFinding.ground.CellToWorld(
                        new Vector3Int(targetPos.x, currentPos.y, 0)
                    );
                }
                else
                {
                    temp = pathFinding.ground.CellToWorld(
                        new Vector3Int(currentPos.x, targetPos.y, 0)
                    );
                }
                moveTarget = new Vector2(temp.x, temp.y + 0.15f);
            }
        }
    }
}
