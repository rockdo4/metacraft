using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public abstract class AttackableUnit : MonoBehaviour
{
    protected TestBattleManager testBattleManager;
    [SerializeField]
    protected CharacterDataBundle characterData;

    protected NavMeshAgent pathFind;

    protected AttackableUnit target;
    protected LayerMask targetType;

    [SerializeField]
    protected int hp;

    protected float lastNormalAttackTime;
    protected float lastAutoTime;
    protected float lastActiveTime;

    protected float activeStartTime; //궁극기 유지 시간동안 평타, 일반스킬X

    protected Action nowUpdate;

    protected Action Common;
    protected Action Auto;
    protected Action Active;

    protected virtual void Awake()
    {
        testBattleManager = FindObjectOfType<TestBattleManager>();
    }

    protected UnitState unitState;

    protected UnitBattleState battleState;
    protected UnitBattleState BattleState { get { return battleState; }  set { battleState = value; } }

    protected bool CanNormalAttack {
        get {
            return (Time.time - lastNormalAttackTime) > characterData.attack.cooldown;
        }
    }

    protected bool InRange => Vector3.Distance(target.transform.position, transform.position) < characterData.attack.distance;
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
        lastNormalAttackTime = Time.time;
        pathFind.stoppingDistance = characterData.attack.distance;
        Common = CommonAttack;
        Auto = AutoAttack;
        Active = ActiveAttack;

        hp = characterData.data.healthPoint;
    }
    protected void FixedUpdate()
    {
        NormalAttackUpdate();
        nowUpdate?.Invoke();
    }

    protected void NormalAttackUpdate()
    {
        if (Time.time - lastNormalAttackTime >= characterData.attack.cooldown)
        {
            lastNormalAttackTime = Time.deltaTime;
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

    public abstract void OnDamage(int dmg);
}