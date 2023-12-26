using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text resourceText;
    public Text waveText;
    public List<Text> towerPriceTextList;

    public List<Button> towerSelectButtonList;
    public List<Image> chekingList;

    public RectTransform upgradePanel;
    public List<Sprite> upgradeAfterSpriteList;
    public Image afterUpgradeImage;
    public Text upgradePriceText;

    public Image pauseImage;

    public Image claerImage;

    public List<Image> currentWaveMobImage;
    public List<Sprite> waveMobs;

    private GameManager gameManager;
    private MobSpawner mobSpawner = null;
    private TowerSpawner towerSpawner;
    private SpawnManager spawnManager;

    public List<GameObject> heart;
    public Tilemap ground;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        towerSpawner = FindObjectOfType<TowerSpawner>();
        spawnManager = FindObjectOfType<SpawnManager>();
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
            WaveMobImage(gameManager.wave);
    }

    public void WaveMobImage(int wave)
    {
        if(mobSpawner.spawnableMobIndexes.Count == 1)
        {
            currentWaveMobImage[0].GetComponent<Image>().sprite = waveMobs[mobSpawner.spawnableMobIndexes[0]];
            currentWaveMobImage[1].gameObject.SetActive(false);
            if(gameManager.wave == 5 || gameManager.wave == 10)
            {
                currentWaveMobImage[1].gameObject.SetActive(true);
                currentWaveMobImage[0].GetComponent<Image>().sprite = waveMobs[mobSpawner.spawnableMobIndexes[0]];
                if( gameManager.wave == 5 )
                    currentWaveMobImage[1].GetComponent<Image>().sprite = waveMobs[6];
                else if( gameManager.wave == 10 )
                    currentWaveMobImage[1].GetComponent<Image>().sprite = waveMobs[7];
            }
        }
        else if (mobSpawner.spawnableMobIndexes.Count == 2)
        {
            currentWaveMobImage[1].gameObject.SetActive(true);
            currentWaveMobImage[0].GetComponent<Image>().sprite = waveMobs[mobSpawner.spawnableMobIndexes[0]];
            currentWaveMobImage[1].GetComponent<Image>().sprite = waveMobs[mobSpawner.spawnableMobIndexes[1]];
        }
        
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

    public void UpgradeImage(Vector3Int tileIntPos)
    {
        if (ground.GetTile(tileIntPos) == null)
        {
            upgradePanel.gameObject.SetActive(false);
            return;
        }
        var tileObject = spawnManager.GetObject(tileIntPos);
        if (tileObject == null)
        {
            upgradePanel.gameObject.SetActive(false);
            return;
        }
        if (tileObject.TryGetComponent(out Tower tower))
        {
            upgradePanel.gameObject.SetActive(true);
            var parentCanvas = upgradePanel.gameObject.GetComponentInParent<Canvas>();
            Vector2 movePos;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentCanvas.transform as RectTransform,
                Input.mousePosition + Vector3.right * 150f,
                parentCanvas.worldCamera,
                out movePos
            );

            upgradePanel.transform.position = parentCanvas.transform.TransformPoint(movePos);
            if (tileObject.TryGetComponent<ArcherTower>(out ArcherTower a) == true)
            {
                if (a.level == 1)
                {
                    afterUpgradeImage.GetComponent<Image>().sprite = upgradeAfterSpriteList[0];
                    upgradePriceText.text = towerSpawner.towerPrefab[3]
                        .GetComponent<Tower>()
                        .price.ToString();
                }
                else if (a.level == 2)
                {
                    afterUpgradeImage.GetComponent<Image>().sprite = upgradeAfterSpriteList[1];
                    upgradePriceText.text = towerSpawner.towerPrefab[4]
                        .GetComponent<Tower>()
                        .price.ToString();
                }
                else if (a.level == 3)
                {
                    upgradePanel.gameObject.SetActive(false);
                    return;
                }
            }
            else if (tower.TryGetComponent<WizardTower>(out WizardTower w) == true)
            {
                if (w.level == 1)
                {
                    afterUpgradeImage.GetComponent<Image>().sprite = upgradeAfterSpriteList[2];
                    upgradePriceText.text = towerSpawner.towerPrefab[5]
                        .GetComponent<Tower>()
                        .price.ToString();
                }
                if (w.level == 2)
                {
                    afterUpgradeImage.GetComponent<Image>().sprite = upgradeAfterSpriteList[3];
                    upgradePriceText.text = towerSpawner.towerPrefab[6]
                        .GetComponent<Tower>()
                        .price.ToString();
                }
                else if (w.level == 3)
                {
                    upgradePanel.gameObject.SetActive(false);
                    return;
                }
            }
            else if (tower.TryGetComponent<SlowTower>(out SlowTower s) == true)
                if (s.level == 1)
                {
                    afterUpgradeImage.GetComponent<Image>().sprite = upgradeAfterSpriteList[4];
                    upgradePriceText.text = towerSpawner.towerPrefab[7]
                        .GetComponent<Tower>()
                        .price.ToString();
                }
                else if (s.level == 2)
                {
                    upgradePanel.gameObject.SetActive(false);
                    return;
                }
        }
    }

    public IEnumerator ClearImage()
    {
        yield return new WaitForSeconds(1);
        claerImage.gameObject.SetActive(true);
    }

    IEnumerator LazyStart()
    {
        yield return new WaitForSeconds(0.5f);
        mobSpawner = FindObjectOfType<MobSpawner>();
        UpdateWave();
    }
}
