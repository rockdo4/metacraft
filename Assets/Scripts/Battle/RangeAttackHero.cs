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
            SearchMaxHealthTarget(enemyList); //체력이 가장 많은 타겟 추적
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
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, characterData.attack.angle / 2, characterData.attack.distance);
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -characterData.attack.angle / 2, characterData.attack.distance);
    }
#endif
}
