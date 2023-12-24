using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    public Vector3Int tilePosition;
    public int price;
    public int level;
    public float attackDamage;
    public float attackDelay;
    public float attackRange;
    public float detectDelay = 0.1f;
    protected MobSpawner mobSpawner;
    protected float time;
    protected GameManager gameManager;

    private bool canAttack = true;

    public virtual void Start()
    {
        mobSpawner = FindObjectOfType<MobSpawner>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (gameManager.isPaused)
            return;
        time += Time.deltaTime;
        if ((time >= detectDelay) && canAttack)
        {
            time = 0;
            List<Mob> attackableMob = new List<Mob>();
            for (int i = 0; i < mobSpawner.mobList.Count; i++)
            {
                var distance =
                    Mathf.Pow(mobSpawner.mobList[i].tilePosition.x - tilePosition.x, 2)
                    + Mathf.Pow(mobSpawner.mobList[i].tilePosition.y - tilePosition.y, 2);
                if (distance <= attackRange * attackRange)
                {
                    attackableMob.Add(mobSpawner.mobList[i]);
                }
            }
            if (attackableMob.Count != 0)
            {
                Attack(attackableMob);
                canAttack = false;
                StartCoroutine(AttackDelay());
            }
        }
    }

    public abstract void Attack(List<Mob> attackableMob);

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(attackDelay);
        canAttack = true;
    }
}
