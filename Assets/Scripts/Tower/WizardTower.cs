using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardTower : Tower
{
    private ParticleSystem fire;

    public override void Start()
    {
        base.Start();
        fire = GetComponentInChildren<ParticleSystem>();
    }

    public override void Attack(List<Mob> attackableMob)
    {
        fire.Play();
        foreach(var mob in attackableMob)
        {
            mob.TakeDamage(attackDamage);
        }
    }
}
