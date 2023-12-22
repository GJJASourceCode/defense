using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text resourceText;
    public Text waveText;
    // public Text towerPriceText;
    public List<Button> towerSelectButtonList;
    private GameManager gameManager;
    private MobSpawner mobSpawner = null;
    private TowerSpawner towerSpawner;
    private Tower tower;

    public List<GameObject> heart;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        towerSpawner = FindObjectOfType<TowerSpawner>();
        UpdateHP();
        UpdateMoney();
        StartCoroutine(LazyStart());
        for (int i = 0; i < towerSelectButtonList.Count; i++)
        {
            int j = i;
            towerSelectButtonList[j].onClick.AddListener(delegate{SelectTower(j);});
        }
    }

    private void SelectTower(int index)
    {
        Debug.Log(index);
        towerSpawner.towerIndex = index;
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
            "Wave "
            + gameManager.wave
            + "\nNext Wave "
            + (gameManager.maxWaveMobCount - mobSpawner.waveMobCount)
            +" / "
            + gameManager.maxWaveMobCount;
    }
    
    public void UpdatePrice(int price)
    {
        // towerPriceText.text = price.ToString();
        //TODO 가격 업데이트 만들기
    }

    IEnumerator LazyStart()
    {
        yield return new WaitForSeconds(0.5f);
        mobSpawner = FindObjectOfType<MobSpawner>();
        UpdateWave();
    }
}
