using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTower : Tower
{
    private ParticleSystem ice;
    public float slow;

    public override void Start()
    {
        base.Start();
        ice = GetComponentInChildren<ParticleSystem>();
    }

    public override void Attack(List<Mob> attackableMob)
    {
        ice.Play();
        foreach(var mob in attackableMob)
        {
            StartCoroutine(ReturnSpeedToOriginal(mob));
            mob.GetComponent<MobController>().speed *= slow;
        }
    }


    IEnumerator ReturnSpeedToOriginal(Mob mob)
    {
        yield return new WaitForSeconds(attackDelay);
        if (mob !=null)
        {
        mob.GetComponent<MobController>().speed /= slow;   
        }
    }
}