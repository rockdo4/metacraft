using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public abstract class AttackableEnemy : AttackableUnit
{
    [SerializeField]
    public void SetTargetList(List<AttackableHero> list) => heroList = list;

    private AttackedDamageUI floatingDamageText;
    private HpBarManager hpBarManager;

    protected override UnitState UnitState {
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
                    animator.SetTrigger("Idle");
                    nowUpdate = IdleUpdate;
                    break;
                case UnitState.Battle:
                    animator.SetBool("IsBattle", true);
                    BattleState = UnitBattleState.MoveToTarget;
                    battleManager.GetHeroList(ref heroList);
                    pathFind.stoppingDistance = characterData.attack.distance ;
                    pathFind.speed = characterData.data.moveSpeed;
                    pathFind.isStopped = false;
                    nowUpdate = BattleUpdate;
                    break;
                case UnitState.Die:
                    animator.SetTrigger("Die");
                    pathFind.isStopped = true;
                    nowUpdate = DieUpdate;
                    break;
                default:
                    Logger.Debug("Error");
                    break;
            }
        }
    }

    protected override UnitBattleState BattleState {
        get {
            return battleState;
        }
        set {
            if (value == battleState)
            {
                // Attack 에서 Attack로 갈수도 있어서 일단은 returnX
                // return;
            }
            battleState = value;

            switch (battleState)
            {
                case UnitBattleState.MoveToTarget:
                    animator.SetTrigger("Run");
                    animator.ResetTrigger("Idle");
                    break;
                case UnitBattleState.BattleIdle:
                    animator.ResetTrigger("Run");
                    animator.SetTrigger("Idle");
                    break;
                case UnitBattleState.NormalAttack:
                    animator.ResetTrigger("Run"); //문제가 생겨서 임시. 
                    animator.ResetTrigger("Idle");
                    animator.SetTrigger("IsAttack");
                    //NormalAttackAction();
                    break;
                case UnitBattleState.PassiveSkill:
                    break;
                case UnitBattleState.ActiveSkill:
                    break;
                case UnitBattleState.Stun:
                    break;
            }
        }
    }

    public bool isMapTriggerEnter = false;

    protected override void Awake()
    {
        base.Awake();
        pathFind = transform.GetComponent<NavMeshAgent>();
        characterData.InitSetting();
        SetData();

        unitState = UnitState.Idle;
        lastNormalAttackTime = lastPassiveSkillTime = Time.time;

        floatingDamageText = GetComponent<AttackedDamageUI>();
        hpBarManager = GetComponent<HpBarManager>();
        hpBarManager.SetHp(UnitHp, characterData.data.healthPoint);
    }
    protected abstract void SearchTarget(); //각각의 캐릭터가 탐색 조건이 다름.

    public override void NormalAttack()
    {
    }
    public override void PassiveSkill()
    {
    }
    public override void ActiveSkill()
    {
    }

    protected override void IdleUpdate()
    {

    }
    protected override void BattleUpdate()
    {
        //타겟이 없을때 타겟을 찾으면 타겟으로 가기
        switch (BattleState)
        {
            //타겟에게 이동중이거나, 공격 대기중에 타겟이 죽으면 재탐색
            case UnitBattleState.MoveToTarget:
            case UnitBattleState.BattleIdle:
                if (IsAlive(target))
                {
                    Vector3 targetDirection = target.transform.position - transform.position;
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10);
                }
                else
                {
                    SearchTarget();
                    if (IsAlive(target))
                    {
                        //if (InRangePassiveSkill && CanPassiveSkillTime)   
                        //    BattleState = UnitBattleState.PassiveSkill;
                        //else
                        if (InRangeNormalAttack && CanNormalAttackTime)
                            BattleState = UnitBattleState.NormalAttack;
                        else
                            BattleState = UnitBattleState.MoveToTarget;
                    }
                    else
                        return;
                }
                break;
        }

        switch (BattleState)
        {
            case UnitBattleState.MoveToTarget: //타겟에게 이동중 타겟 거리 계산.
                if (InRangeNormalAttack)
                    BattleState = CanNormalAttackTime ? UnitBattleState.NormalAttack : UnitBattleState.BattleIdle;
                else if (Time.time - lastNavTime > navDelay) //일반공격, 패시브 사용 불가 거리일시 이동
                {
                    lastNavTime = Time.time;
                    pathFind.SetDestination(target.transform.position);
                }
                break;
            case UnitBattleState.BattleIdle:
                if (!InRangeNormalAttack)
                {
                    BattleState = UnitBattleState.MoveToTarget;
                }
                else if (InRangeNormalAttack && CanNormalAttackTime)
                {
                    BattleState = UnitBattleState.NormalAttack;
                }
                break;
            case UnitBattleState.NormalAttack:
                break;
            case UnitBattleState.PassiveSkill:
                break;
            case UnitBattleState.ActiveSkill:
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

    public override void ChangeUnitState(UnitState state)
    {
        UnitState = state;
    }
    public override void ChangeBattleState(UnitBattleState state)
    {
        BattleState = state;
    }

    public override void OnDamage(int dmg, bool isCritical)
    {
        UnitHp = Mathf.Max(UnitHp - dmg, 0);
        if (UnitHp <= 0)
            UnitState = UnitState.Die;

        TempShowHpBarAndDamageText(dmg, isCritical);
    }
    public void TempShowHpBarAndDamageText(int dmg, bool isCritical = false)
    {
        floatingDamageText.OnAttack(dmg, isCritical, transform.position, DamageType.Normal);
        hpBarManager.TestCode(dmg);
        if (UnitHp <= 0)
            hpBarManager.Die();
    }

    public override void OnDead(AttackableUnit unit)
    {
        battleManager.OnDeadEnemy((AttackableEnemy)unit);
    }

    //타겟이 없으면 Idle로 가고, 쿨타임 계산해서 바로 스킬 가능하면 사용, 아니라면 대기
    public override void NormalAttackEnd()
    {
        base.NormalAttackEnd();
        lastNormalAttackTime = Time.time;
        if (target == null  || !target.gameObject.activeSelf)
        {
            BattleState = UnitBattleState.BattleIdle;
        }
        else if (InRangeNormalAttack && CanNormalAttackTime)
            BattleState = UnitBattleState.NormalAttack;
        else
        {
            BattleState = UnitBattleState.BattleIdle;
        }
    }
    public override void PassiveSkillEnd()
    {
    }
    public override void ActiveSkillEnd()
    {
        base.ActiveSkillEnd();
    }

    public AttackableEnemy TestGetIsBattle()
    {
        if (UnitState == UnitState.Battle)
            return this;
        else
            return null;
    }
    public void SetEnabledPathFind(bool set)
    {
        pathFind.enabled = set;
    }
}