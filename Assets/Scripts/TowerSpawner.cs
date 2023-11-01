using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;

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
