using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    public GameObject towerPrefab;
    private float TowerNumber;
    bool keypad;

    public void Update()
    {
        keypad = Input.GetKeyDown(KeyCode.Alpha1);
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            towerPrefab = GameObject.Find("Tower01");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            towerPrefab = GameObject.Find("wizard01");
        }
    }

    public void SpawnTower(Transform tileTransform)
    {
        Tile tile = tileTransform.GetComponent<Tile>();

        if (tile.IsBuildTower == true) //건설됨이면 건설x
        {
            return;
        }

        tile.IsBuildTower = true; //건설됨 설정
        Instantiate(towerPrefab, tileTransform.position + Vector3.up * 0.4f, Quaternion.identity); //타워 건설
    }
}
