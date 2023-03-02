using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                    animator.ResetTrigger("Run");
                    animator.SetTrigger("Run");
                }
                else
                    SearchNearbyTarget(enemyList); //가장 가까운 적
            }
            else
                SearchNearbyTarget(enemyList); //가장 가까운 적
        }
    }

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

            return;
        }

        switch (BattleState)
        {
            //타겟에게 이동중이거나, 공격 대기중에 범위 안에 적이 들어왔는지 확인
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
