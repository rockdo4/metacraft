using System.Collections.Generic;
using UnityEngine;

public class ShortAttack : AttackableHero
{
    public float angleRange = 30f;

    public override void Attack()
    {
        base.Attack();
        Logger.Debug("Attack1111");
        Logger.Debug(charactorData.damage);

        if (charactorData.attackCount == 1)
        {
            Logger.Debug("Attack : " + target.name);
            return;
        }

        List<GameObject> attackEenmys = new();

        var Enemys = Physics.SphereCastAll(transform.position, charactorData.attackDistance, Vector3.up, 0f);
        foreach (var enemy in Enemys)
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
            Logger.Debug("Attack : " + enemy.name);
        }
    }

    public override void Skill()
    {
        base.Skill();
        Logger.Debug("Skill111");
    }
}
