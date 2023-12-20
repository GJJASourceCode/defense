using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    public Node tilePosition;

    [SerializeField]
    private float maxHP;
    private float currentHP;
    private bool isDie = false;
    private HealthBar healthBar;

    void Awake()
    {
        currentHP = maxHP;
    }

    void Start()
    {
        healthBar = GetComponentInChildren<HealthBar>();
    }

    public void TakeDamage(float damage)
    {
        if (isDie == true)
            return;

        currentHP -= damage;
        healthBar.ChangeHealth(currentHP, maxHP);

        if (currentHP <= 0)
        {
            isDie = true;
            FindObjectOfType<MobSpawner>().MobDie(this);
            Destroy(gameObject);
        }
    }
}
