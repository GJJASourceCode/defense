using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text resourceText;
    public Text waveText;
    private GameManager gameManager;
    private MobSpawner mobSpawner = null;

    public List<GameObject> heart;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        UpdateHP();
        UpdateMoney();
        StartCoroutine(LazyStart());
    }

    public void UpdateMoney()
    {
        resourceText.text = "  : " + gameManager.money;
    }

    public void UpdateHP()
    {
        for (int i = 0; i < heart.Count; i++)
        {
            if (i < gameManager.houseHP)
            {
                heart[i].SetActive(true);
            }
            else
            {
                heart[i].SetActive(false);
            }
        }
    }

    public void UpdateWave()
    {
        waveText.text =
            "현재 웨이브 : "
            + gameManager.wave
            + "\n 다음 웨이브까지 남은 몹 수 : "
            + (gameManager.maxWaveMobCount - mobSpawner.waveMobCount);
    }

    IEnumerator LazyStart()
    {
        yield return new WaitForSeconds(0.5f);
        mobSpawner = FindObjectOfType<MobSpawner>();
        UpdateWave();
    }
}
