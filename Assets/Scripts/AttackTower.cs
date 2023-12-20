using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackTower : MonoBehaviour
{
    [SerializeField]
    private float attackDamage;

    [SerializeField]
    private float attackSpeed;

    [SerializeField]
    private float attackRange;

    public GameObject arrowPrefab;

    private MobSpawner mobSpawner;
    private Tower tower;
    private float time;

    void Start()
    {
        mobSpawner = FindObjectOfType<MobSpawner>();
        tower = GetComponent<Tower>();
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time >= attackSpeed)
        {
            time = 0;
            float minDistance = Mathf.Infinity;
            Mob minMob = null;
            for (int i = 0; i < mobSpawner.mobList.Count; i++)
            {
                var distance =
                    Mathf.Pow(mobSpawner.mobList[i].tilePosition.x - tower.tilePosition.x, 2)
                    + Mathf.Pow(mobSpawner.mobList[i].tilePosition.y - tower.tilePosition.y, 2);
                if (distance <= attackRange * attackRange && distance < minDistance)
                {
                    minDistance = distance;
                    minMob = mobSpawner.mobList[i];
                }
            }
            if (minMob != null)
            {
                var arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity)
                    .GetComponent<Arrow>();
                arrow.MoveTo(minMob.transform);
                arrow.damage = attackDamage;
            }
        }
    }
}
