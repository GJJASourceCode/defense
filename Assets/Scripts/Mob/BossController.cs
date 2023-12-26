using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private PathFinding pathFinding;
    private MobSpawner mobSpawner;
    private GameObject house;
    public float speed;

    private Vector3Int startPos;
    private Vector3Int targetPos;
    private Vector2 moveTarget;
    private List<Node> path = null;
    private Mob mob;
    private GameManager gameManager;
    private SpawnManager spawnManager;
    private float originSpeed;
    float stopforTime = 0;

    void OnEnable()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
        gameManager = FindObjectOfType<GameManager>();
        pathFinding = FindObjectOfType<PathFinding>();
        mobSpawner = FindObjectOfType<MobSpawner>();
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
        stopforTime += Time.deltaTime;
        if (gameManager.isPaused)
            return;
        float step = speed * Time.deltaTime;
        var currentPos = pathFinding.ground.WorldToCell(
            transform.position + new Vector3(0, -0.15f, 0)
        );
        if (path != null)
        {
            var graph = pathFinding.ground.GetComponent<Ground>().GetGraph();
            mob.tilePosition = graph[currentPos.x - pathFinding.ground.cellBounds.xMin][
                currentPos.y - pathFinding.ground.cellBounds.yMin
            ];
            mob.tilePosition.x = mob.tilePosition.x + pathFinding.ground.cellBounds.xMin;
            mob.tilePosition.y = mob.tilePosition.y + pathFinding.ground.cellBounds.yMin;
            transform.position = Vector2.MoveTowards(transform.position, moveTarget, step);
            if (
                spawnManager.GetObject(currentPos) != null
                && !spawnManager.GetObject(currentPos).TryGetComponent(out MobSpawner a)
            )
            {
                spawnManager.RemoveObject(currentPos);
                Stop();
            }
        }
        if (Vector2.Distance(moveTarget, transform.position) <= 0.01f)
        {
            // FindObjectOfType<SpawnManager>().SpawnObject(startPos, testPrefab);
            if (currentPos == targetPos) // 집 도착
            {
                mobSpawner.MobDie(GetComponent<Mob>());
                gameManager.AttackHouse(3);
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

    public IEnumerator Stop() 
    {
        mob.TryGetComponent<BossController>(out BossController m);
        originSpeed = m.speed;
        Debug.Log(originSpeed);
        m.speed = 0;
        yield return new WaitForSeconds(1);
        m.speed = originSpeed;
    }
}
