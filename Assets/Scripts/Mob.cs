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

    void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(float damage)
    {
        if (isDie == true)
            return;

        currentHP -= damage;

        Debug.Log("currentHP: " + currentHP);

        if (currentHP <= 0)
        {
            isDie = true;
            FindObjectOfType<MobSpawner>().MobDie(this);
            Destroy(gameObject);
        }
    }
}
