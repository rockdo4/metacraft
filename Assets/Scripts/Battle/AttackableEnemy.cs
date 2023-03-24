using System.Collections.Generic;
using UnityEngine;

public class AttackableEnemy : AttackableUnit
{
    [SerializeField]
    public void SetTargetList(List<AttackableUnit> list) => heroList = list;

    protected override UnitState UnitState
    {
        get
        {
            return unitState;
        }
        set
        {
            if (unitState == value)
                return;
            if (unitState == UnitState.Die && value != UnitState.None)
                return;
            unitState = value;
            switch (unitState)
            {
                case UnitState.None:
                    nowUpdate = null;
                    break;
                case UnitState.Idle:
                    //pathFind.isStopped = true;

                    animator.SetFloat("Speed", 0);

                    nowUpdate = IdleUpdate;
                    break;
                case UnitState.Battle:
                    //pathFind.isStopped = false;
                    pathFind.speed = characterData.data.moveSpeed;
                    pathFind.stoppingDistance = minAttackDis;

                    // 매니저 못 찾아서 임시로 추가
                    {
                        var manager = FindObjectOfType<BattleManager>();
                        if (manager != null)
                            battleManager = manager;
                    }
                    battleManager.GetCurrBtMgr().GetEnemyList(ref enemyList);
                    battleManager.GetHeroList(ref heroList);

                    animator.SetFloat("Speed", 1);

                    BattleState = UnitBattleState.MoveToTarget;
                    nowUpdate = BattleUpdate;
                    OnPassiveSkill(enemyList, heroList);
                    break;
                case UnitState.Die:
                    //pathFind.enabled = false;
                    OnDead(this);
                    RemoveAllBuff();
                    BattleState = UnitBattleState.None;
                    gameObject.GetComponent<Collider>().enabled = false;
                    animator.Rebind();
                    animator.SetTrigger("Die");

                    //Logger.Debug("Enemy Die");
                    nowUpdate = DieUpdate;
                    break;
                default:
                    break;
            }
        }
    }

    protected override UnitBattleState BattleState
    {
        get
        {
            return battleState;
        }
        set
        {
            if (value == battleState)
                return;
            if (unitState == UnitState.Die && value != UnitBattleState.None)
                return;

            battleState = value;
            switch (battleState)
            {
                case UnitBattleState.MoveToTarget:
                    //Logger.Debug(transform.parent.parent.parent.name);
                    pathFind.isStopped = false;
                    break;
                case UnitBattleState.BattleIdle:
                    pathFind.isStopped = true;
                    break;
                case UnitBattleState.NormalAttack:
                    pathFind.isStopped = true;
                    animator.SetTrigger("Attack");
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
    float attackDelay = 1f;
    float attackDelayTimer = 0f;

    private void Awake()
    {
        InitData();
        SetPathFind();
        characterData.InitSetting();
        SetData();

        unitState = UnitState.Idle;
        ResetCoolDown();

        //hpBarManager.SetHp(UnitHp, characterData.data.healthPoint);
        hpBarManager.SetLiveData(characterData.data);
    }

    private void Start()
    {
        if (effectCreateTransform.Equals(null))
            effectCreateTransform = transform;

        foreach (var attack in characterData.attacks)
        {
            attack.SkillHolderTransform = effectCreateTransform;
            attack.ActorTransform = transform;
        }
        if (characterData.activeSkill != null)
            characterData.activeSkill.ActorTransform = transform;

        var manager = FindObjectOfType<BattleManager>();
        if (manager != null)
            battleManager = manager;
    }

    public override void ResetData()
    {
        UnitState = UnitState.None;
        BattleState = UnitBattleState.None;
        UnitHp = characterData.data.healthPoint;
        //hpBarManager.SetHp(UnitHp, characterData.data.healthPoint);
        hpBarManager.SetLiveData(characterData.data);
        lastNavTime = Time.time;
        ResetCoolDown();
        target = null;
        animator.Rebind();
        base.ResetData();
    }

    public override void ReadyActiveSkill()
    {
    }

    protected override void IdleUpdate()
    {

    }
    protected override void BattleUpdate()
    {

        if (attackDelayTimer < attackDelay)
            attackDelayTimer += Time.deltaTime;

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
                        if (attackDelayTimer >= attackDelay && FindNowAttack())
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
                if (attackDelayTimer >= attackDelay && FindNowAttack())
                {
                    BattleState = UnitBattleState.NormalAttack;
                }
                else if (InRangeMinNormalAttack)
                {
                    BattleState = UnitBattleState.BattleIdle;
                }
                else if (Time.time - lastNavTime > navDelay) //일반공격, 패시브 사용 불가 거리일시 이동
                {
                    lastNavTime = Time.time;

                    pathFind.SetDestination(target.transform.position);
                }
                break;
            case UnitBattleState.BattleIdle:
                if (attackDelayTimer >= attackDelay && FindNowAttack())
                    BattleState = UnitBattleState.NormalAttack;
                else if (!InRangeMinNormalAttack)
                    BattleState = UnitBattleState.MoveToTarget;
                break;
            case UnitBattleState.NormalAttack:
                stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("NormalAttack") && stateInfo.normalizedTime >= 1.0f)
                {
                    //if (name.Contains("Test"))
                    //{
                    //    // Logger.Debug("anime name : "  + animator.GetCurrentAnimatorClipInfo(0)[0].clip.);
                    //    Logger.Debug(nowAttack.name + " End");
                    //}
                    NormalAttackEnd();
                    attackDelayTimer = 0f;
                    //StartCoroutine(TestAttackEnd());
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
        hpBarManager.Die();
    }

    //타겟이 없으면 Idle로 가고, 쿨타임 계산해서 바로 스킬 가능하면 사용, 아니라면 대기
    public override void NormalAttackEnd()
    {
        base.NormalAttackEnd();
        animator.SetTrigger("AttackEnd");
        lastNormalAttackTime[nowAttack] = Time.time;

        BattleState = UnitBattleState.BattleIdle;
    }

    public override void OnActiveSkill()    //테스트용
    {
        if (nowAttack.targetNumLimit == 1)
        {
            target.OnDamage(this, characterData.activeSkill);
            return;
        }

        List<AttackableUnit> attackTargetList = new();

        var targetList = (activeAttackTargetType == UnitType.Hero) ? heroList : enemyList;
        foreach (var now_target in targetList)
        {
            Vector3 interV = now_target.transform.position - transform.position;
            if (interV.magnitude <= nowAttack.distance)
            {
                float angle = Vector3.Angle(transform.forward, interV);

                if (Mathf.Abs(angle) < nowAttack.angle / 2f)
                {
                    attackTargetList.Add(now_target);
                }
            }
        }

        attackTargetList = GetNearestUnitList(attackTargetList, nowAttack.targetNumLimit);

        for (int i = 0; i < attackTargetList.Count; i++)
        {
            attackTargetList[i].OnDamage(this, characterData.activeSkill);
        }
    }

    public override void ActiveSkillEnd()
    {
        pathFind.isStopped = false;
        animator.SetTrigger("ActiveEnd");
        ResetCoolDown();
        BattleState = UnitBattleState.BattleIdle;
        base.ActiveSkillEnd();
    }

    public override void StunEnd()
    {
        if (unitState == UnitState.Die)
        {
            animator.SetTrigger("Die");
            return;
        }
        base.StunEnd();
        BattleState = UnitBattleState.BattleIdle;
    }

    public AttackableEnemy GetIsBattle()
    {
        if (UnitState == UnitState.Battle)
            return this;
        else
            return null;
    }

    public override void DestroyUnit()
    {
        Destroy(gameObject);
    }
}