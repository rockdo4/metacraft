using System.Collections.Generic;
using UnityEngine;

public class ShortAttack : AttackableHero
{
    public float angleRange = 30f;

    public override void NormalAttack()
    {
        base.NormalAttack();
        Logger.Debug("Attack1111");
        Logger.Debug(heroData.stats.baseDamage);

        if (heroData.normalAttack.count == 1)
        {
            target.GetComponent<AttackableUnit>().OnDamage(heroData.stats.baseDamage);
            return;
        }

        List<GameObject> attackEenmys = new();

        foreach (var enemy in targetList)
        {
            Vector3 interV = enemy.transform.position - transform.position;
            float dot = Vector3.Dot(interV.normalized, transform.forward);
            float theta = Mathf.Acos(dot);
            float degree = Mathf.Rad2Deg * theta;

            if (degree <= angleRange / 2f)
                attackEenmys.Add(enemy.transform.gameObject);
        }

        foreach (var enemy in attackEenmys)
        {
            enemy.GetComponent<AttackableEnemy>().OnDamage(heroData.stats.baseDamage);
        }
    }
    public override void NormalSkill()
    {
        base.NormalSkill();
        Logger.Debug("Skill111");
    }

    protected override void SetTarget()
    {
        base.SetTarget(); //근거리 타겟 추적
    }

}
