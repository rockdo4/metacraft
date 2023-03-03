using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CovertAttackHero : AttackableHero
{
    protected float searchDelay = 1f;
    protected float lastSearchTime;

    public override void NormalAttack()
    {
        if (BattleState == UnitBattleState.ActiveSkill)
            return;

        base.NormalAttack();


        if (characterData.attack.count == 1)
        {
            target.OnDamage(characterData.data.baseDamage, false);
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

        attackEnemies = GetNearestUnitList(attackEnemies, characterData.attack.count);

        for (int i = 0; i < attackEnemies.Count; i++)
        {
            attackEnemies[i].OnDamage(characterData.data.baseDamage, false);
        }
    }

    public override void PassiveSkill()
    {
        base.PassiveSkill();
        return;
    }
    public override void ActiveSkill()
    {
        base.ActiveSkill();

    }

    protected override void SearchTarget()
    {
        if(heroList.Count == 1)
            SearchNearbyTarget(heroList); //근거리 타겟 추적
        else
            SearchMinHealthTarget(enemyList); //체력이 가장 적은 타겟 추적
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, characterData.attack.angle / 2, characterData.attack.distance);
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -characterData.attack.angle / 2, characterData.attack.distance);
    }
#endif
}
