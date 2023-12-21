using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject mobSpawnerPrefab;
    private SpawnManager spawnManager;
    void Start()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
        
        var spawner = spawnManager.SpawnObjectAtEdge(mobSpawnerPrefab);
        if (spawner == null) Debug.Log("null");
    }
}
