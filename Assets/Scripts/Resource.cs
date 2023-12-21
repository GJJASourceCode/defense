using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Resource : MonoBehaviour
{
    private Tilemap ground;
    private SpawnManager spawnManager;

    private void Start() {
        ground = FindObjectOfType<Tilemap>();
        spawnManager = FindObjectOfType<SpawnManager>();
    }

    private void Update() 
    {
        if (Input.GetMouseButton(1)) //우클
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int pos = ground.WorldToCell(new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0));
            if (spawnManager.GetObject(pos)== this.gameObject)
            {
               spawnManager.RemoveObject(pos);
            }
        }
    }
}
