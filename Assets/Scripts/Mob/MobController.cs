using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobController : MonoBehaviour
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

    void OnEnable()
    {
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
        if (gameManager.isPaused)
            return;
        float step = speed * Time.deltaTime;
        if (path != null)
            transform.position = Vector2.MoveTowards(transform.position, moveTarget, step);
        if (Vector2.Distance(moveTarget, transform.position) <= 0.1f)
        {
            startPos = pathFinding.ground.WorldToCell(
                transform.position + new Vector3(0, -0.15f, 0)
            );
            // FindObjectOfType<SpawnManager>().SpawnObject(startPos, testPrefab);
            path = pathFinding.FindPath(startPos.x, startPos.y, targetPos.x, targetPos.y);
            if (path == null)
            {
                Debug.Log("Can't Find Path");
            }
            else if (path.Count == 1) // 집 도착
            {
                mobSpawner.MobDie(GetComponent<Mob>());
                gameManager.AttackHouse(1);
                Destroy(gameObject);
            }
            else
            {
                mob.tilePosition = path[0];
                var temp = pathFinding.ground.CellToWorld(new Vector3Int(path[1].x, path[1].y, 0));
                moveTarget = new Vector2(temp.x, temp.y + 0.15f);
            }
        }
    }
}
