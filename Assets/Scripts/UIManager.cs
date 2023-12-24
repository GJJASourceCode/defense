using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text resourceText;
    public Text waveText;
    public List<Text> towerPriceTextList;

    public List<Button> towerSelectButtonList;
    public List<Image> chekingList;
    public Image pauseImage;
    private GameManager gameManager;
    private MobSpawner mobSpawner = null;
    private TowerSpawner towerSpawner;

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
            towerSelectButtonList[j].onClick.AddListener(
                delegate
                {
                    SelectTower(j);
                }
            );
        }
    }

    private void SelectTower(int index)
    {
        // Debug.Log(index);
        // if (index != 3)
        towerSpawner.towerIndex = index;
        for (int i = 0; i < chekingList.Count; i++)
        {
            chekingList[i].gameObject.SetActive(false);
        }
        chekingList[index].gameObject.SetActive(true);
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
            + " / "
            + gameManager.maxWaveMobCount;
    }

    public void UpdatePrice(int index)
    {
        towerPriceTextList[index].text = towerSpawner.towerPrefab[index]
            .GetComponent<Tower>()
            .price.ToString();
    }

    public void PauseImage(bool isPaused)
    {
        pauseImage.gameObject.SetActive(isPaused);
    }

    IEnumerator LazyStart()
    {
        yield return new WaitForSeconds(0.5f);
        mobSpawner = FindObjectOfType<MobSpawner>();
        UpdateWave();
    }
}
