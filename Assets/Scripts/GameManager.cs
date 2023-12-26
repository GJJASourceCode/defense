using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public GameObject mobSpawnerPrefab;
    private SpawnManager spawnManager;
    private ResourceSpawner resourceSpawner;
    public int money;
    public int houseHP;
    private UIManager uiManager;
    public int wave = 1;
    private MobSpawner mobSpawner;
    public int maxWaveMobCount; // 현재 웨이브에 소환될 몹 개수
    public bool isPaused = false;
    public float timeforStart = 0;

    void Start()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
        uiManager = FindObjectOfType<UIManager>();
        resourceSpawner = FindObjectOfType<ResourceSpawner>();
        var spawner = spawnManager.SpawnObjectAtEdge(mobSpawnerPrefab);
        if (spawner == null)
            Debug.Log("null");
        mobSpawner = spawner.GetComponent<MobSpawner>();
        Wave1();
        resourceSpawner.spawnTime = 0.9f;
    }

    public void AttackHouse(int damage)
    {
        houseHP -= damage;
        uiManager.UpdateHP();
    }

    void Update()
    {
        if (houseHP <= 0 )
        {
            SceneManager.LoadScene("GameOVer");
        }

        timeforStart += Time.deltaTime;
        if(timeforStart >= 3)
            resourceSpawner.spawnTime = 3;
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
        else if (wave == 5)
        {
            if (mobSpawner.waveMobCount >= maxWaveMobCount)
            {
                mobSpawner.waveMobCount = 0;
                wave++;
                Wave6();
            }
        }
        else if (wave == 6)
        {
            if (mobSpawner.waveMobCount >= maxWaveMobCount)
            {
                mobSpawner.waveMobCount = 0;
                wave++;
                Wave7();
            }
        }
        else if (wave == 7)
        {
            if (mobSpawner.waveMobCount >= maxWaveMobCount)
            {
                mobSpawner.waveMobCount = 0;
                wave++;
                Wave8();
            }
        }
        else if (wave == 8)
        {
            if (mobSpawner.waveMobCount >= maxWaveMobCount)
            {
                mobSpawner.waveMobCount = 0;
                wave++;
                Wave9();
            }
        }
        else if (wave == 9)
        {
            if (mobSpawner.waveMobCount >= maxWaveMobCount)
            {
                mobSpawner.waveMobCount = 0;
                wave++;
                Wave10();
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
        maxWaveMobCount = 20;
        var temp = new List<int> { 0, 2 };
        mobSpawner.spawnableMobIndexes = temp;
    }

    void Wave3()
    {
        maxWaveMobCount = 15;
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

    void Wave6()
    {
        maxWaveMobCount = 20;
        var temp = new List<int> { 3 };
        mobSpawner.spawnableMobIndexes = temp;
    }

    void Wave7()
    {
        maxWaveMobCount = 20;
        var temp = new List<int> { 3, 5 };
        mobSpawner.spawnableMobIndexes = temp;
    }

    void Wave8()
    {
        maxWaveMobCount = 20;
        var temp = new List<int> { 4 };
        mobSpawner.spawnableMobIndexes = temp;
    }

    void Wave9()
    {
        maxWaveMobCount = 20;
        var temp = new List<int> { 4, 5 };
        mobSpawner.spawnableMobIndexes = temp;
    }

    void Wave10()
    {
        maxWaveMobCount = 20;
        var temp = new List<int> { 5 };
        mobSpawner.spawnableMobIndexes = temp;
        mobSpawner.SpawnBoss(1);
    }
    public void Gameover()
    {
        Debug.Log("디짐");
    }
}
