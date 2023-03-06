using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class RangeAttackHero : AttackableHero
{
    protected float searchDelay = 1f;
    protected float lastSearchTime;
    public Transform attackPos;
    public FireBallTest attackPref;

    protected override void Awake()
    {
        lastSearchTime = Time.time;
        base.Awake();
    }
    protected override void SearchTarget()
    {
        lastSearchTime = Time.time;
        var minTarget = GetSearchTargetInAround(enemyList, characterData.attack.distance / 2);

        if (IsAlive(minTarget))
            target = minTarget;
        else
            SearchMaxHealthTarget(enemyList); //ü���� ���� ���� Ÿ�� ����
    }

    public override void NormalAttack()
    {
        if (BattleState == UnitBattleState.ActiveSkill)
            return;

        base.NormalAttack();

        var f = Instantiate(attackPref, attackPos.transform.position, Quaternion.identity);
        f.transform.rotation = transform.rotation;
        f.Set(target.transform, characterData);
        f.MoveStart();
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
                    SearchTarget();
                }
                break;
        }

        base.BattleUpdate();
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, characterData.attack.angle / 2, characterData.attack.distance);
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -characterData.attack.angle / 2, characterData.attack.distance);
    }
#endif
}
