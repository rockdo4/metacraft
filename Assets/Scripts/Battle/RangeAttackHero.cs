using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RangeAttackHero : AttackableHero
{
    protected AttackableUnit warningTarget;
    float searchDelay = 1f;
    float lastSearchTime = 1f;

    protected override void Awake()
    {
        lastSearchTime = Time.time;
        base.Awake();
    }
    protected override void SearchTarget()
    {
        if (Time.time - lastSearchTime >= searchDelay)
        {
            lastSearchTime = Time.time;
            var minTarget = GetSearchTargetInAround(enemyList, characterData.attack.distance / 2);

            if (minTarget != null)
            {
                target = minTarget;
                return;
            }

        }
        SearchMaxHealthTarget(enemyList); //ü���� ���� ���� Ÿ�� ����
    }

    public override void NormalAttack()
    {
        base.NormalAttack();

        if (characterData.attack.count == 1)
        {
            target.GetComponent<AttackableEnemy>().OnDamage(characterData.data.baseDamage, false);
            return;
        }

        List<GameObject> attackEnemies = new();

        foreach (var enemy in enemyList)
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
            //Ÿ�ٿ��� �̵����̰ų�, ���� ����߿� ���� �ȿ� ���� ���Դ��� Ȯ��
            case UnitBattleState.MoveToTarget:
            case UnitBattleState.BattleIdle:
                if (Time.time - lastSearchTime >= searchDelay)
                {
                    var minTarget = GetSearchTargetInAround(enemyList, characterData.attack.distance/2);

                    if (minTarget != null)
                        target = minTarget;

                    lastSearchTime = Time.time;
                }
                break;
        }

        base.BattleUpdate();
    }
}
