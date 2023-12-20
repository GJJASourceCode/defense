using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject Resource;
    public float spawnTime = 3f; // 생성주기
    public float curTime; // 현재 시간
    public Transform[] spawnPoints; // 생성위치
    public bool[] isSpawn;

    private void Start()
    {
        isSpawn = new bool[spawnPoints.Length];
        for (int i = 0; i < isSpawn.Length; i++)
            isSpawn[i] = false;
    }

    private void Update()
    {
        if (curTime >= spawnTime)
        {
            curTime = 0;
            int x = Random.Range(0, spawnPoints.Length);
            if (isSpawn[x]) // x위치에 자원 있으면 생성no
            {
                curTime = spawnTime;
                return;
            }
            SpawnResource(x);
        }

        curTime += Time.deltaTime;
    }

    private void SpawnResource(int ranNum) //자원 생성
    {
        curTime = 0;
        Instantiate(Resource, spawnPoints[ranNum]);
        isSpawn[ranNum] = true;
    }
}
