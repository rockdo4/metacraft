using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public abstract class AttackableUnit : MonoBehaviour
{
    protected CharacterData charactorData;
    protected NavMeshAgent pathFind;

    protected AttackableUnit target;
    protected LayerMask targetType;

    [SerializeField]
    protected int hp;

    protected float lastCommonTime;
    protected float lastAutoTime;
    protected float lastActiveTime;

    protected float activeStartTime; //궁극기 유지 시간동안 평타, 일반스킬X

    protected Action nowUpdate;

    protected Action Common;
    protected Action Auto;
    protected Action Active;


    protected UnitState unitState;
    protected UnitState UnitState { get { return unitState; } set { unitState = value; } }

    protected UnitBattleState battleState;
    protected UnitBattleState BattleState { get { return battleState; }  set { battleState = value; } }

    protected bool IsBasicCoolDown {
        get {
            return Time.time - lastCommonTime > charactorData.cooldown;
        }
    }

    protected bool InRange => Vector3.Distance(target.transform.position, transform.position) < charactorData.attackDistance;
    protected bool IsNormal => battleState != UnitBattleState.Action && battleState != UnitBattleState.Stun;

    protected bool IsAttack {
        get {
            return IsNormal && InRange;
        }
    }

    [SerializeField]
    protected List<AttackableUnit> targetList;
    protected void SetTargetList(List<AttackableUnit> list) => targetList = list;

    //BattleManager에서 targetList 가 null이면 다음 행동 지시
    protected virtual void SetTarget()
    {
        if (targetList.Count == 0)
        {
            target = null;
            return;
        }

        target = targetList.OrderBy(t => Vector3.Distance(t.transform.position, transform.position))
                          .FirstOrDefault();
    }
    protected void SetData()
    {
        lastCommonTime = Time.time;
        pathFind.stoppingDistance = charactorData.attackDistance;
        Common = CommonAttack;
        Auto = AutoAttack;
        Active = ActiveAttack;

        hp = charactorData.hp;
    }
    protected void FixedUpdate()
    {
        BasicCoolDownUpdate();
        if (nowUpdate != null)
            nowUpdate();
    }

    protected void BasicCoolDownUpdate()
    {
        if (Time.time - lastCommonTime >= charactorData.cooldown)
        {
            lastCommonTime = Time.deltaTime;
        }
    }

    protected abstract void SetTestBattle();
    public abstract void CommonAttack();
    public abstract void AutoAttack();
    public abstract void ActiveAttack();

    protected abstract void IdleUpdate();
    protected abstract void BattleUpdate();
    protected abstract void DieUpdate();
    protected abstract void MoveNextUpdate();
    protected abstract void ReturnPosUpdate();

    public CharacterData LoadTestData()
    {
        CharacterData testData = new();

        testData.damage = 3;
        testData.def = 20;
        testData.hp = 100;
        testData.speed = 3;
        testData.critical = 50;
        testData.criticalDmg = 2;
        testData.evasion = 20;
        testData.accuracy = 80;

        testData.attackDistance = 3f;
        testData.attackCount = 2;

        testData.grade = "A";
        testData.level = 1;
        testData.job = "TEST";
        testData.cooldown = 1;
        testData.skillCooldown = 0.3f;
        testData.skillDuration = 3f;
        testData.exp = 0;

        return testData;
    }
    public abstract void OnDamage(int dmg);
}
