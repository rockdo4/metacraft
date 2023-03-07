using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ShortAttackHero : AttackableHero
{
    public override void NormalAttack()
    {
        if (BattleState == UnitBattleState.ActiveSkill)
            return;

        base.NormalAttack();

        AddBuff(BuffType.AttackIncrease, 0.5f, 2f);

        if (characterData.attack.targetNumLimit == 1)
        {
            target.OnDamage(GetFixedDamage, false);
            return;
        }

        List<AttackableUnit> attackEnemies = new();

        foreach (var enemy in enemyList)
        {
            Vector3 interV = enemy.transform.position - transform.position;
            if (interV.magnitude <= characterData.attack.distance)
            {
                float angle = Vector3.Angle(transform.forward, interV);

                if (Mathf.Abs(angle) < characterData.attack.angle / 2f)
                {
                    attackEnemies.Add(enemy);
                }
            }
        }

        attackEnemies = GetNearestUnitList(attackEnemies, characterData.attack.targetNumLimit);

        for (int i = 0; i < attackEnemies.Count; i++)
        {
            attackEnemies[i].OnDamage(GetFixedDamage, false);
        }
    }

    public override void PassiveSkill()
    {
        base.PassiveSkill();
    }
    public override void ReadyActiveSkill()
    {
        base.ReadyActiveSkill();
    }

    protected override void SearchTarget()
    {
        SearchNearbyTarget(enemyList); //근거리 타겟 추적
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, characterData.attack.angle / 2, characterData.attack.distance);
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -characterData.attack.angle / 2, characterData.attack.distance);
    }
#endif
}
