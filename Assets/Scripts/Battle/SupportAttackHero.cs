using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SupportAttackHero : AttackableHero
{
    protected float searchDelay = 1f;
    protected float lastSearchTime;

    bool moveToTeam = false;
    Vector3 teamPos;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void SearchTarget()
    {
        if (moveToTeam)
            return;
        if (Time.time - lastSearchTime >= searchDelay)
        {
            lastSearchTime = Time.time;
            var minTarget = GetSearchTargetInAround(enemyList, characterData.attack.distance / 2);

            if (minTarget != null)
            {
                if(target == null  || !target.gameObject.activeSelf)
                    SearchNearbyTarget(heroList);
                return;
            }
            else
            {
                teamPos = GetSearchNearbyTarget(heroList).transform.position;
                pathFind.SetDestination(teamPos);
                moveToTeam = true;
                animator.ResetTrigger("Run");
                animator.SetTrigger("Run");
            }

        }
        SearchNearbyTarget(enemyList); //가장 가까운 적
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
        if(moveToTeam )
        {
            if (Vector3.Distance(teamPos, transform.position) < characterData.attack.distance/2)
            {
                moveToTeam = false;
                target = null;
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
                    var minTarget = GetSearchTargetInAround(enemyList, characterData.attack.distance / 2);

                    if (minTarget != null)
                        target = minTarget;

                    lastSearchTime = Time.time;
                }
                break;
        }

        base.BattleUpdate();
    }
}
