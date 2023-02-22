using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AttackableEnemy : AttackableUnit
{
    protected new UnitState unitState;
    public new UnitState UnitState {
        get {
            return unitState;
        }
        set {
            if (unitState == value)
                return;

            unitState = value;
            switch (unitState)
            {
                case UnitState.Idle:
                    pathFind.isStopped = true;
                    nowUpdate = IdleUpdate;
                    break;
                case UnitState.Battle:
                    pathFind.speed = charactorData.speed;
                    pathFind.isStopped = false;
                    EnemyBattleState = UnitBattleState.Common;
                    nowUpdate = BattleUpdate;
                    break;
                case UnitState.Die:
                    pathFind.isStopped = true;
                    nowUpdate = DieUpdate;
                    Destroy(gameObject, 1);
                    break;
                default:
                    Logger.Debug("Error");
                    break;
            }
        }
    }

    UnitBattleState enemyBattleState;
    public UnitBattleState EnemyBattleState {
        get {
            return enemyBattleState;
        }
        set {
            if (value == enemyBattleState)
                return;
            enemyBattleState = value;
        }
    }

    protected override void Awake()
    {
        charactorData = LoadTestData(); //임시 데이터 로드
        pathFind = transform.GetComponent<NavMeshAgent>();
        SetData();
    }

    public override void CommonAttack()
    {
    }
    public override void AutoAttack()
    {

    }
    public override void ActiveAttack()
    {
        activeStartTime = Time.time;
        BattleState = UnitBattleState.Action;
        Logger.Debug("Skill");
    }

    protected override void IdleUpdate()
    {

    }
    protected override void BattleUpdate()
    {
        switch (EnemyBattleState)
        {
            case UnitBattleState.Common:
                //타겟이 없으면 타겟 추척
                if (target == null)
                {
                    SetTarget();
                    return;
                }

                //타겟으로 이동
                pathFind.SetDestination(target.transform.position);

                //타겟과 일정 범위 안에 있으며, 일반스킬 상태이고, 쿨타임 조건이 충족될때
                if (IsAttack && IsBasicCoolDown)
                {
                    lastCommonTime = Time.time;
                    Common();
                }
                break;
            case UnitBattleState.Action:
                if (Time.time - activeStartTime > charactorData.skillDuration)
                {
                    EnemyBattleState = UnitBattleState.Common;
                }
                break;
            case UnitBattleState.Stun:
                break;
        }

    }

    protected override void DieUpdate()
    {

    }

    protected override void MoveNextUpdate()
    {

    }
    protected override void ReturnPosUpdate()
    {

    }


    [ContextMenu("Battle")]
    protected override void SetTestBattle()
    {
        UnitState = UnitState.Battle;
    }

    public override void OnDamage(int dmg)
    {
        hp = Mathf.Max(hp - dmg, 0);
        if (hp <= 0)
            UnitState = UnitState.Die;
    }
}