using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ArcherTower : Tower
{
    public GameObject arrowPrefab;

    
    public override void Attack(List<Mob> attackableMob)
    {
        float minDistance = Mathf.Infinity;
        Mob minMob = null;
        for (int i = 0; i <attackableMob.Count; i++)
        {
            var distance =
                Mathf.Pow(attackableMob[i].tilePosition.x - tilePosition.x, 2)
                + Mathf.Pow(attackableMob[i].tilePosition.y - tilePosition.y, 2);
            if (distance <= attackRange * attackRange && distance < minDistance)
            {
                minDistance = distance;
                minMob = attackableMob[i];
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
