using System.Collections.Generic;
using UnityEngine;

public class SupportAttackHero : AttackableHero
{
    protected float searchDelay = 1f;
    protected float lastSearchTime;
    bool moveToTeam = false;

    protected override void Awake()
    {
        lastSearchTime = Time.time;
        base.Awake();
    }
    protected override void SearchTarget()
    {
        if (moveToTeam)
            return;

        if (Time.time - lastSearchTime >= searchDelay)
        {
            lastSearchTime = Time.time;

            if (heroList.Count != 1)
            {
                var nearyTeam = GetSearchTargetInAround(heroList, characterData.attack.distance / 2);
                if (!IsAlive(nearyTeam))
                {
                    moveToTeam = true;
                    target = GetSearchNearbyTarget(heroList);
                    pathFind.SetDestination(target.transform.position);
                }
                else
                    SearchNearbyTarget(enemyList); //���� ����� ��
            }
            else
                SearchNearbyTarget(enemyList); //���� ����� ��
        }
    }

    public override void NormalAttack()
    {
        if (BattleState == UnitBattleState.ActiveSkill)
            return;

        base.NormalAttack();

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

    protected override void BattleUpdate()
    {
        if(moveToTeam)
        {
            if (Vector3.Distance(target.transform.position, transform.position) < characterData.attack.distance/2)
            {
                moveToTeam = false;
                target = null;
                SearchTarget();
            }
        }

        switch (BattleState)
        {
            //Ÿ�ٿ��� �̵����̰ų�, ���� ����߿� ���� �ȿ� ���� ���Դ��� Ȯ��
            case UnitBattleState.MoveToTarget:
            case UnitBattleState.BattleIdle:
                if (Time.time - lastSearchTime >= searchDelay)
                {
                    SearchTarget();
                }
                break;
        }

        base.BattleUpdate();
    }
}
