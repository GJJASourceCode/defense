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
        foreach (var mob in attackableMob)
        {
            if (mob != null)
            {
                StartCoroutine(ReturnSpeedToOriginal(mob));
                if (mob.TryGetComponent<MobController>(out MobController m) == true)
                    m.speed *= slow;
                else if (mob.TryGetComponent<BossController>(out BossController b) == true)
                    b.speed *= slow;
            }
            
        }
    }

    IEnumerator ReturnSpeedToOriginal(Mob mob)
    {
        yield return new WaitForSeconds(attackDelay);
        if (mob != null)
        {
            if (mob.TryGetComponent<MobController>(out MobController m) == true)
                m.speed /= slow;
            else if (mob.TryGetComponent<BossController>(out BossController b) == true)
                b.speed /= slow;
        }
    }
}
