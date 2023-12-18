using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    public GameObject mobPrefab;
    public float spawnTime;

    private float time = 0f;

    void Update()
    {
        time += Time.deltaTime;

        if (time >= spawnTime)
        {
            time = 0f;

            Instantiate(mobPrefab, transform.position, Quaternion.identity);
        }
    }
}
