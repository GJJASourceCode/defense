using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject mobSpawnerPrefab;
    private SpawnManager spawnManager;
    public int money;
    public int houseHP;
    private UIManager uiManager;
    public int wave = 1;
    private MobSpawner mobSpawner;
    public int maxWaveMobCount; // 현재 웨이브에 소환될 몹 개수
    public bool isPaused = false;

    void Start()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
        uiManager = FindObjectOfType<UIManager>();
        var spawner = spawnManager.SpawnObjectAtEdge(mobSpawnerPrefab);
        if (spawner == null)
            Debug.Log("null");
        mobSpawner = spawner.GetComponent<MobSpawner>();
        Wave1();
    }

    public void AttackHouse()
    {
        houseHP--;
        uiManager.UpdateHP();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            uiManager.PauseImage(isPaused);
        }

        if (wave == 1)
        {
            if (mobSpawner.waveMobCount >= maxWaveMobCount)
            {
                mobSpawner.waveMobCount = 0;
                wave++;
                Wave2();
            }
        }
        else if (wave == 2)
        {
            if (mobSpawner.waveMobCount >= maxWaveMobCount)
            {
                mobSpawner.waveMobCount = 0;
                wave++;
                Wave3();
            }
        }
        else if (wave == 3)
        {
            if (mobSpawner.waveMobCount >= maxWaveMobCount)
            {
                mobSpawner.waveMobCount = 0;
                wave++;
                Wave4();
            }
        }
        else if (wave == 4)
        {
            if (mobSpawner.waveMobCount >= maxWaveMobCount)
            {
                mobSpawner.waveMobCount = 0;
                wave++;
                Wave5();
            }
        }
    }

    void Wave1()
    {
        maxWaveMobCount = 15;
        var temp = new List<int> { 0 };
        mobSpawner.spawnableMobIndexes = temp;
    }

    void Wave2()
    {
        maxWaveMobCount = 10;
        var temp = new List<int> { 2 };
        mobSpawner.spawnableMobIndexes = temp;
    }

    void Wave3()
    {
        maxWaveMobCount = 10;
        var temp = new List<int> { 1 };
        mobSpawner.spawnableMobIndexes = temp;
    }

    void Wave4()
    {
        maxWaveMobCount = 20;
        var temp = new List<int> { 1, 2 };
        mobSpawner.spawnableMobIndexes = temp;
    }

    void Wave5()
    {
        maxWaveMobCount = 10;
        var temp = new List<int> { 2 };
        mobSpawner.spawnableMobIndexes = temp;
        mobSpawner.SpawnBoss(0); // 첫번재 보스 소환
    }
}
