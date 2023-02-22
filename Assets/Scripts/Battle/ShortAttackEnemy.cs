using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortAttackEnemy : AttackableEnemy
{
    public float angleRange = 30f;

    public override void CommonAttack()
    {
        base.CommonAttack();
        Logger.Debug("Enemy_Attack1111");
        Logger.Debug(heroData.stats.baseDamage);

        if (heroData.normalAttack.count == 1)
        {
            target.GetComponent<AttackableHero>().OnDamage(heroData.stats.baseDamage);
            return;
        }

        List<GameObject> attackHeros = new();

        foreach (var hero in targetList)
        {
            Vector3 interV = hero.transform.position - transform.position;
            float dot = Vector3.Dot(interV.normalized, transform.forward);
            float theta = Mathf.Acos(dot);
            float degree = Mathf.Rad2Deg * theta;

            if (degree <= angleRange / 2f)
                attackHeros.Add(hero.transform.gameObject);
        }

        foreach (var hero in attackHeros)
        {
            hero.GetComponent<AttackableHero>().OnDamage(heroData.stats.baseDamage);
        }
    }
    public override void AutoAttack()
    {
        base.AutoAttack();
        Logger.Debug("Skill111");
    }

    protected override void SetTarget()
    {
        base.SetTarget(); //근거리 타겟 추적
    }
}
