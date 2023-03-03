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
    }
    public override void ActiveSkill()
    {
        base.ActiveSkill();

        if (IsAlive(target)) //�ӽ��ڵ�� Ÿ�ٿ� ���� �������� ����ؼ� nullüũ.������� �̰͵� ��ӹ޴� �Լ��� ������ �־�� ��.
            target.OnDamage(177);
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
