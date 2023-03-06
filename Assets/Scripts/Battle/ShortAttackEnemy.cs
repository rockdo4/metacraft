using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ShortAttackEnemy : AttackableEnemy
{
    public override void NormalAttack()
    {
        if (BattleState == UnitBattleState.ActiveSkill)
            return;

        base.NormalAttack();

        if (characterData.attack.targetNumLimit == 1)
        {
            target.OnDamage(characterData.data.baseDamage);
            return;
        }

        List<AttackableUnit> attackHeroes = new();

        foreach (var hero in heroList)
        {
            Vector3 interV = hero.transform.position - transform.position;
            if (interV.magnitude <= characterData.attack.distance)
            {
                float angle = Vector3.Angle(transform.forward, interV);

                if (Mathf.Abs(angle) < characterData.attack.angle / 2f)
                {
                    attackHeroes.Add(hero);
                }
            }
        }

        attackHeroes = GetNearestUnitList(attackHeroes, characterData.attack.targetNumLimit);

        for (int i = 0; i < attackHeroes.Count; i++)
        {
            attackHeroes[i].OnDamage(characterData.data.baseDamage);
        }
    }

    public override void PassiveSkill()
    {
        base.PassiveSkill();
    }

    protected override void SearchTarget()
    {
        SearchNearbyTarget(heroList); //근거리 타겟 추적
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, characterData.attack.angle / 2, characterData.attack.distance);
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -characterData.attack.angle / 2, characterData.attack.distance);
    }
#endif
}
