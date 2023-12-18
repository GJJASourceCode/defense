using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobController : MonoBehaviour
{
    private PathFinding pathFinding;
    private GameObject mobSpawner;
    private GameObject house;
    public float speed;

    private Vector2 moveTarget;
    private List<Node> path = null;
    private int pathIndex;

    void OnEnable()
    {
        pathFinding = GameObject.Find("PathFinder").GetComponent<PathFinding>();
        mobSpawner = GameObject.Find("MobSpawner");
        house = GameObject.Find("House");

        moveTarget = new Vector2(transform.position.x, transform.position.y);
        var startPos = pathFinding.ground.WorldToCell(mobSpawner.transform.position - Vector3.up*0.5f);
        var targetPos = pathFinding.ground.WorldToCell(house.transform.position);

        // pathFinding.towerSpawner.SpawnTower(startPos);
        // pathFinding.towerSpawner.SpawnTower(targetPos);

        // Debug.Log(startPos);
        // Debug.Log(targetPos);

        path = pathFinding.FindPath(startPos.x, startPos.y, targetPos.x, targetPos.y);
        if (path == null)
        {
            Debug.Log("Can't Find Path");
        }
        else{
            var temp = pathFinding.ground.CellToWorld(new Vector3Int(path[0].x,path[0].y,0));
            moveTarget = new Vector2(temp.x,temp.y+0.25f);
            pathIndex = 0;
        }
    }

    void Update() {
        
        if (path != null)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, moveTarget, step);
            if (Vector2.Distance(moveTarget, transform.position) <= 0.1f)
            {
                if (pathIndex < path.Count - 1)
                {
                    pathIndex++;
                    var temp = pathFinding.ground.CellToWorld(new Vector3Int(path[pathIndex].x,path[pathIndex].y,0));
                    moveTarget = new Vector2(temp.x,temp.y+0.25f);
                }
            }

        }
    }
}
