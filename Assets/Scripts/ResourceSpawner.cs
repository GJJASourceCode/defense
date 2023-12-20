using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject resource;
    public float spawnTime = 3f; // 생성주기
    private float time; // 현재 시간

    private void Update()
    {
        if (time >= spawnTime)
        {
            time = 0;
            FindObjectOfType<SpawnManager>().SpawnObjectAtRandomPos(resource);
        }

        time += Time.deltaTime;
    }
}
