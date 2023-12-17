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

    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0)) //좌클
        {
            //     ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            //     var hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            //     if (hit)
            //     {
            //         if (hit.transform.CompareTag("Tile"))
            //         {
            //             towerSpawner.SpawnTower(hit.transform);
            //         }
            //     }
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int pos = ground.WorldToCell(new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0));
            // Vector3Int pos = new Vector3Int(cO.x + 1, cO.y + 1, 0);
            // Debug.Log(ground.GetTile(pos).name);
            if (ground.GetTile(pos) != null)
            {
                towerSpawner.SpawnTower(pos);
                Debug.Log(pos);
            }
        }
    }
}
