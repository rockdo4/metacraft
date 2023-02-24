using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RangeAttackHero : AttackableHero
{
    protected AttackableUnit warningTarget;

    protected override void SearchTarget()
    {
        SearchNearbyTarget(); //체력이 가장 많은 타겟 추적
        base.SearchTarget();
    }

    public override void NormalAttack()
    {
        base.NormalAttack();
        //Logger.Debug("Hero_NormalAttack");

        if (characterData.attack.count == 1)
        {
            target.GetComponent<AttackableEnemy>().OnDamage(characterData.data.baseDamage, false);
            return;
        }

        List<GameObject> attackEnemies = new();

        foreach (var enemy in targetList)
        {
            Vector3 interV = enemy.transform.position - transform.position;
            if (interV.magnitude <= characterData.attack.distance)
            {
                float angle = Vector3.Angle(transform.forward, interV);

                if (Mathf.Abs(angle) < characterData.attack.angle / 2f)
                {
                    attackEnemies.Add(enemy.transform.gameObject);
                }
            }
        }

        attackEnemies.OrderBy(t => Vector3.Distance(transform.position, t.transform.position));

        var cnt = Mathf.Min(attackEnemies.Count, characterData.attack.count);
        for (int i = 0; i < cnt; i++)
        {
            attackEnemies[i].GetComponent<AttackableEnemy>().OnDamage(characterData.data.baseDamage, false);
        }
    }
    public override void PassiveSkill()
    {
        base.PassiveSkill();
    }

    protected override void BattleUpdate()
    {
        switch (BattleState)
        {
            case UnitBattleState.NormalAttack:
                //if(ContainTarget(targetList, target, characterData.attack.distance))
                //{

                //}
                break;
            case UnitBattleState.PassiveSkill:
                break;
            case UnitBattleState.ActiveSkill:
                break;
            case UnitBattleState.Stun:
                break;
        }
        base.BattleUpdate();
    }
}
