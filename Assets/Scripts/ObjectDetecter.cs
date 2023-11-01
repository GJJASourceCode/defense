using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetecter : MonoBehaviour
{
    [SerializeField]
    private TowerSpawner towerSpawner;

    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) //좌클
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            var hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if (hit != null)
            {
                if (hit.transform.CompareTag("Tile"))
                {
                    towerSpawner.SpawnTower(hit.transform);
                }
            }
        }
    }
}
