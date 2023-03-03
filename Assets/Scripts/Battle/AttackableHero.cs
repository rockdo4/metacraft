using UnityEngine;
using UnityEngine.AI;

public abstract class AttackableHero : AttackableUnit
{
    protected HeroUi heroUI;

    private Transform returnPos;
    public void SetReturnPos(Transform tr) => returnPos = returnPos = tr;

    private Coroutine coOnIndicator;

    protected override UnitState UnitState {
        get {
            return unitState;
        }
        set {
            //if (unitState == value)
            //    return;

            unitState = value;
            heroUI.heroState = unitState;
            switch (unitState)
            {
                case UnitState.Idle:
                    pathFind.isStopped = true;

                    animator.SetFloat("Speed", 0);

                    nowUpdate = IdleUpdate;
                    break;
                case UnitState.ReturnPosition: // 재배치
                    pathFind.isStopped = false;
                    pathFind.stoppingDistance = 0; //가까이 가기
                    pathFind.SetDestination(returnPos.position); //재배치 위치 설정

                    BattleState = UnitBattleState.None;
                    nowUpdate = ReturnPosUpdate;
                    break;
                case UnitState.MoveNext:
                    pathFind.isStopped = false;

                    nowUpdate = MoveNextUpdate;
                    break;
                case UnitState.Battle:
                    pathFind.isStopped = false;
                    pathFind.speed = characterData.data.moveSpeed;
                    pathFind.stoppingDistance = characterData.attack.distance;

                    battleManager.GetEnemyList(ref enemyList);
                    battleManager.GetHeroList(ref heroList);

                    animator.SetFloat("Speed", 1);

                    BattleState = UnitBattleState.MoveToTarget;
                    nowUpdate = BattleUpdate;
                    break;
                case UnitState.Die:
                    pathFind.isStopped = true;

                    animator.SetTrigger("Die");

                    nowUpdate = DieUpdate;
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

            //상태가 바뀔때마다 애니메이션 호출
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
                    break;
                case UnitBattleState.Stun:
                    break;
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        pathFind = transform.GetComponent<NavMeshAgent>();
        characterData.InitSetting();
        SetData();

        unitState = UnitState.Idle;

        lastNormalAttackTime = lastPassiveSkillTime = Time.time;
    }

    // Ui와 연결, Ui에 스킬 쿨타임 연결
    public virtual void SetUi(HeroUi _heroUI)
    {
        heroUI = _heroUI;
        //BattleState = UnitBattleState.ActiveSkill;
        heroUI.heroSkill.Set(characterData.activeSkill.cooldown, ReadyActiveSkill, PlayActiveSkillAnimation); //궁극기 쿨타임과 궁극기 함수 등록
    }

    protected override void SearchTarget()
    {

    }

    public override void NormalAttack()
    {
    }

    public override void PassiveSkill()
    {
    }
    public override void ReadyActiveSkill()
    {        
        if (coOnIndicator != null)
        {
            characterData.activeSkill.SkillCancle();
            StopCoroutine(coOnIndicator);
            coOnIndicator = null;
            return;
        }        

        coOnIndicator = StartCoroutine(characterData.activeSkill.SkillCoroutine());
    }
    public void PlayActiveSkillAnimation()
    {
        pathFind.isStopped = true;
        BattleState = UnitBattleState.ActiveSkill;
        StopCoroutine(coOnIndicator);
        coOnIndicator = null;
    }    
    public override void OnActiveSkill()
    {
        characterData.activeSkill.OnActiveSkill();
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
                animator.SetFloat("Speed", pathFind.velocity.magnitude / characterData.data.moveSpeed);
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
        var nowSpeed = animator.GetFloat("Speed");
        nowSpeed = Mathf.Lerp(nowSpeed, 1, Time.deltaTime * 5f);
        animator.SetFloat("Speed", nowSpeed);
    }

    protected override void ReturnPosUpdate()
    {
        switch (pathFind.isStopped)
        {
            case true:
                transform.rotation = Quaternion.Lerp(transform.rotation, returnPos.rotation, Time.deltaTime * 5);
                float angle = Quaternion.Angle(transform.rotation, returnPos.rotation);

                var nowSpeed = animator.GetFloat("Speed");
                var downSpeed = Mathf.Lerp(0, 1, Time.deltaTime * 10f);
                animator.SetFloat("Speed", nowSpeed - downSpeed);

                if (angle <= 0)
                {
                    UnitState = UnitState.Idle;
                    battleManager.OnReady();
                }
                break;
            case false:
                animator.SetFloat("Speed", pathFind.velocity.magnitude / characterData.data.moveSpeed);
                if (Vector3.Distance(returnPos.position, transform.position) <= 0.5f)
                {
                    pathFind.isStopped = true;
                    transform.position = returnPos.position;
                }
                break;
        }
    }

    public override void ChangeUnitState(UnitState state)
    {
        if (BattleState == UnitBattleState.ActiveSkill)
            return;
        UnitState = state;
    }
    public override void ChangeBattleState(UnitBattleState state)
    {
        BattleState = state;
    }

    public override void OnDamage(int dmg, bool isCritical = false)
    {
        UnitHp = Mathf.Max(UnitHp - dmg, 0);

        heroUI.SetHp(UnitHp);

        if (UnitHp <= 0)
            UnitState = UnitState.Die;
    }

    public override void OnDead(AttackableUnit unit)
    {
        battleManager.OnDeadHero((AttackableHero)unit);
        heroUI.SetDieImage();

        characterData.activeSkill.SkillCancle();
    }

    //타겟이 없으면 Idle로 가고, 쿨타임 계산해서 바로 스킬 가능하면 사용, 아니라면 대기
    public override void NormalAttackEnd()
    {
        base.NormalAttackEnd();
        animator.SetTrigger("AttackEnd");
        lastNormalAttackTime = Time.time;
        if (!IsAlive(target))
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
        lastPassiveSkillTime = Time.time;
        if (!IsAlive(target))
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
    public override void ActiveSkillEnd()
    {
        pathFind.isStopped = false;
        animator.SetTrigger("ActiveEnd");
        base.ActiveSkillEnd();

        if (enemyList.Count == 0)
        {
            UnitState = UnitState.ReturnPosition;
        }
        else if (!IsAlive(target))
        {
            BattleState = UnitBattleState.BattleIdle;
        }
        else if (InRangeNormalAttack && CanNormalAttackTime)
        {
            BattleState = UnitBattleState.NormalAttack;
        }
        else
        {
            BattleState = UnitBattleState.BattleIdle;
        }
    }
}