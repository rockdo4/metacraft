using System.Collections.Generic;
using UnityEngine;

public class ShortAttack : AttackableHero
{
    public float angleRange = 30f;

    public override void CommonAttack()
    {
        base.CommonAttack();
        Logger.Debug("Attack1111");
        Logger.Debug(characterData.data.baseDamage);

        if (characterData.attack.count == 1)
        {
            target.GetComponent<AttackableUnit>().OnDamage(characterData.data.baseDamage);
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
            enemy.GetComponent<AttackableEnemy>().OnDamage(characterData.data.baseDamage);
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
