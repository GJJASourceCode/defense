using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobController : MonoBehaviour
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

    public GameObject testPrefab;

    void OnEnable()
    {
        pathFinding = GameObject.Find("PathFinder").GetComponent<PathFinding>();
        // mobSpawner = GameObject.Find("MobSpawner");
        house = GameObject.Find("House");
        mob = GetComponent<Mob>();

        moveTarget = new Vector2(transform.position.x, transform.position.y);
        startPos = pathFinding.ground.WorldToCell(
            transform.position + new Vector3(0,-0.25f,0)
        );
        FindObjectOfType<SpawnManager>().SpawnObject(startPos, testPrefab);
        targetPos = pathFinding.ground.WorldToCell(house.transform.position);

        path = pathFinding.FindPath(startPos.x, startPos.y, targetPos.x, targetPos.y);
        if (path == null)
        {
            Debug.Log("Can't Find Path");
        }
        else
        {
            mob.tilePosition = path[0];
            var temp = pathFinding.ground.CellToWorld(new Vector3Int(path[1].x, path[1].y, 0));
            moveTarget = new Vector2(temp.x, temp.y + 0.25f);
        }
    }

    void Update()
    {
        if (path != null)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, moveTarget, step);
            if (Vector2.Distance(moveTarget, transform.position) <= 0.02f)
            {
                startPos = pathFinding.ground.WorldToCell(
                    transform.position + new Vector3(0,-1f,0)
                );
                path = pathFinding.FindPath(startPos.x, startPos.y, targetPos.x, targetPos.y);
                if (path == null)
                {
                    Debug.Log("Can't Find Path");
                }
                else
                {
                    mob.tilePosition = path[0];
                    var temp = pathFinding.ground.CellToWorld(new Vector3Int(path[1].x, path[1].y, 0));
                    moveTarget = new Vector2(temp.x, temp.y + 0.25f);
                }
            }
        }
    }
}
