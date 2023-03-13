using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackableEnemy : AttackableUnit
{
    [SerializeField]
    public void SetTargetList(List<AttackableUnit> list) => heroList = list;

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
                case UnitState.None:
                    nowUpdate = null;
                    break;
                case UnitState.Idle:
                    pathFind.isStopped = true;

                    animator.SetFloat("Speed", 0);

                    nowUpdate = IdleUpdate;
                    break;
                case UnitState.Battle:
                    pathFind.isStopped = false;
                    pathFind.speed = characterData.data.moveSpeed;
                    pathFind.stoppingDistance = characterData.attack.distance;

                    battleManager.GetCurrBtMgr().GetEnemyList(ref enemyList);
                    battleManager.GetHeroList(ref heroList);

                    animator.SetFloat("Speed", 1);

                    BattleState = UnitBattleState.MoveToTarget;
                    nowUpdate = BattleUpdate;
                    break;
                case UnitState.Die:
                    pathFind.enabled = false;
                    gameObject.GetComponent<Collider>().enabled = false;
                    animator.SetTrigger("Die");

                    nowUpdate = DieUpdate;
                    break;
                default:
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
                return;

            battleState = value;
            switch (battleState)
            {
                case UnitBattleState.MoveToTarget:
                    pathFind.isStopped = false;
                    break;
                case UnitBattleState.BattleIdle:
                    pathFind.isStopped = true;
                    break;
                case UnitBattleState.NormalAttack:
                    pathFind.isStopped = true;
                    animator.SetTrigger("Attack");
                    break;
                case UnitBattleState.PassiveSkill:
                    break;
                case UnitBattleState.ActiveSkill:
                    pathFind.isStopped = true;
                    animator.SetTrigger("Active");
                    animator.ResetTrigger("Attack");
                    animator.ResetTrigger("AttackEnd");
                    break;
                case UnitBattleState.Stun:
                    pathFind.isStopped = true;
                    animator.SetTrigger("Stun");
                    animator.ResetTrigger("Attack");
                    animator.ResetTrigger("AttackEnd");
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
        lastNormalAttackTime = Time.time;

        hpBarManager.SetHp(UnitHp, characterData.data.healthPoint);

    }
    public override void ResetData()
    {
        UnitState = UnitState.None;
        battleState = UnitBattleState.None;
        UnitHp = characterData.data.healthPoint;
        hpBarManager.SetHp(UnitHp, characterData.data.healthPoint);
        lastActiveSkillTime = lastNormalAttackTime = lastNavTime = Time.time;
        target = null;
        animator.Rebind();
    }

    public override void PassiveSkillEvent()
    {

    }
    public override void ReadyActiveSkill()
    {
    }

    protected override void IdleUpdate()
    {

    }
    protected override void BattleUpdate()
    {
        if (isAuto && target != null && Time.time - lastActiveSkillTime > characterData.activeSkill.cooldown)
        {
            if (InRangeNormalAttack)
            {
                lastActiveSkillTime = Time.time;
                BattleState = UnitBattleState.ActiveSkill;
            }
        }
        //타겟이 없을때 타겟을 찾으면 타겟으로 가기
        switch (BattleState)
        {
            //타겟에게 이동중이거나, 공격 대기중에 타겟이 죽으면 재탐색
            case UnitBattleState.MoveToTarget:
            case UnitBattleState.BattleIdle:
                animator.SetFloat("Speed", pathFind.velocity.magnitude / characterData.data.moveSpeed);
                if (IsAlive(target))
                {
                    Vector3 targetDirection = target.transform.position - transform.position;
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10);
                }
                else
                {
                    SearchAi();
                    if (IsAlive(target))
                    {
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
                    BattleState = UnitBattleState.MoveToTarget;
                else if (InRangeNormalAttack && CanNormalAttackTime)
                    BattleState = UnitBattleState.NormalAttack;
                break;
            case UnitBattleState.NormalAttack:
                stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("NormalAttack") && stateInfo.normalizedTime >= 1.0f)
                {
                    NormalAttackEnd();
                }
                break;
            case UnitBattleState.ActiveSkill:
                stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("ActiveSkill") && stateInfo.normalizedTime >= 1.0f)
                {
                    ActiveSkillEnd();
                }
                break;
            case UnitBattleState.Stun:
                stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("Stun") && stateInfo.normalizedTime >= 1.0f)
                {
                    StunEnd();
                }
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

    public override void OnDamage(AttackableUnit attackableUnit, CharacterSkill skill)
    {
        base.OnDamage(attackableUnit, skill);        
    }
    public override void OnDead(AttackableUnit unit)
    {
        battleManager.OnDeadEnemy((AttackableEnemy)unit);
    }

    //타겟이 없으면 Idle로 가고, 쿨타임 계산해서 바로 스킬 가능하면 사용, 아니라면 대기
    public override void NormalAttackEnd()
    {
        base.NormalAttackEnd();
        animator.SetTrigger("AttackEnd");
        lastNormalAttackTime = Time.time;

        BattleState = UnitBattleState.BattleIdle;
    }
    public override void OnActiveSkill()    //테스트용
    {
        if (characterData.attack.targetNumLimit == 1)
        {
            bool isCritical = false;
            target.OnDamage(this, characterData.activeSkill);
            return;
        }

        List<AttackableUnit> attackTargetList = new();

        var targetList = (activeAttackTargetType == UnitType.Hero) ? heroList : enemyList;
        foreach (var now_target in targetList)
        {
            Vector3 interV = now_target.transform.position - transform.position;
            if (interV.magnitude <= characterData.attack.distance)
            {
                float angle = Vector3.Angle(transform.forward, interV);

                if (Mathf.Abs(angle) < characterData.attack.angle / 2f)
                {
                    attackTargetList.Add(now_target);
                }
            }
        }

        attackTargetList = GetNearestUnitList(attackTargetList, characterData.attack.targetNumLimit);

        for (int i = 0; i < attackTargetList.Count; i++)
        {
            bool isCritical = false;
            attackTargetList[i].OnDamage(this, characterData.activeSkill);
        }
    }
    public override void ActiveSkillEnd()
    {
        pathFind.isStopped = false;
        animator.SetTrigger("ActiveEnd");
        lastNormalAttackTime = Time.time;
        BattleState = UnitBattleState.BattleIdle;
        base.ActiveSkillEnd();
    }
    public override void StunEnd()
    {
        Logger.Debug("StunEnd");
        base.StunEnd();
        BattleState = UnitBattleState.BattleIdle;
    }

    public AttackableEnemy TestGetIsBattle()
    {
        if (UnitState == UnitState.Battle)
            return this;
        else
            return null;
    }

}