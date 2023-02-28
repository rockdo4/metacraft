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
        //Logger.Debug("Hero_NormalAttack");

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

        attackEnemies = attackEnemies.OrderBy(t => Vector3.Distance(transform.position, t.transform.position)).ToList();

        var cnt = Mathf.Min(attackEnemies.Count, characterData.attack.count);
        for (int i = 0; i < cnt; i++)
        {
            attackEnemies[i].OnDamage(characterData.data.baseDamage, false);
        }
    }

    public override void PassiveSkill()
    {
        base.PassiveSkill();
        //Logger.Debug("Hero_NormalAttack");

        target.OnDamage(characterData.data.baseDamage * 2, true);
        return;

        //Logger.Debug("Hero_PassiveSkill");
    }
    public override void ActiveSkill()
    {
        base.ActiveSkill();
        

        //target.OnDamage(characterData.data.baseDamage * 3, true);
    }

    protected override void SearchTarget()
    {
        SearchNearbyTarget(enemyList); //�ٰŸ� Ÿ�� ����
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, characterData.attack.angle / 2, characterData.attack.distance);
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -characterData.attack.angle / 2, characterData.attack.distance);
    }
#endif
}
