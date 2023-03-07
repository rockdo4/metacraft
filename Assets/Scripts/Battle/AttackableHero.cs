using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public abstract class AttackableHero : AttackableUnit
{
    protected HeroUi heroUI;

    private Transform returnPos;
    public void SetReturnPos(Transform tr) => returnPos = returnPos = tr;

    private Coroutine coOnIndicator;

    bool lateReturn = false;
    protected override UnitState UnitState {
        get {
            return unitState;
        }
        set {
            if (unitState == value)
                return;

            unitState = value;
            heroUI.heroState = unitState;
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
                case UnitState.ReturnPosition: // 재배치
                    pathFind.isStopped = false;
                    pathFind.stoppingDistance = 0; //가까이 가기
                    pathFind.SetDestination(returnPos.position); //재배치 위치 설정

                    animator.ResetTrigger("Attack");

                    BattleState = UnitBattleState.None;
                    nowUpdate = ReturnPosUpdate;

                    lastNormalAttackTime = Time.time;
                    heroUI.heroSkill.CancleSkill();                    

                    target = null;
                    lateReturn = false;
                    testRot = false;
                    break;
                case UnitState.MoveNext:
                    pathFind.isStopped = false;
                    pathFind.SetDestination(transform.position);
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
                    pathFind.enabled = false;
                    gameObject.GetComponent<Collider>().enabled = false;
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
                    animator.ResetTrigger("Attack");
                    animator.ResetTrigger("AttackEnd");
                    break;
                case UnitBattleState.Stun:
                    break;
            }
        }
    }

    bool testRot = false;

    protected override void Awake()
    {
        base.Awake();
        pathFind = transform.GetComponent<NavMeshAgent>();
        characterData.InitSetting();
        SetData();

        unitState = UnitState.Idle;

        lastNormalAttackTime = lastPassiveSkillTime = Time.time;
    }
    private void Start()
    {
        var activeSkill = characterData.activeSkill as ActiveSkillAOE;
        activeSkill.ActorTransform = transform;
    }

    // Ui와 연결, Ui에 스킬 쿨타임 연결
    public virtual void SetUi(HeroUi _heroUI)
    {
        heroUI = _heroUI;
        //BattleState = UnitBattleState.ActiveSkill;
        heroUI.heroSkill.Set(characterData.activeSkill.cooldown); //궁극기 쿨타임 등록
        heroUI.heroSkill.
            SetActions(
            ReadyActiveSkill,
            PlayActiveSkillAnimation,
            OffSkillAreaIndicator,
            SkillCancle);
    }

    public override void ResetData()
    {
        testRot = false;
        UnitState = UnitState.Idle;
        battleState = UnitBattleState.None;
        
        lateReturn = false;
        lastActiveSkillTime = lastNormalAttackTime = lastNavTime = Time.time;
        target = null;
        animator.Rebind();
        //UnitHp = characterData.data.healthPoint;
        
        // Test
        UnitHp = characterData.data.currentHp;
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
        coOnIndicator = StartCoroutine(characterData.activeSkill.SkillCoroutine());
    }
    public void OffSkillAreaIndicator()
    {
        if (coOnIndicator == null)
            return;

        GetActiveSkillAOE().ActiveOffSkillAreaIndicator();                
    }
    public void SkillCancle()
    {
        if (coOnIndicator == null)
            return;

        GetActiveSkillAOE().SetActiveIndicators(false);
        StopAOESkillCoroutine();        
    }
    public void PlayActiveSkillAnimation()
    {        
        pathFind.isStopped = true;
        BattleState = UnitBattleState.ActiveSkill;
        if (coOnIndicator != null)
        {
            GetActiveSkillAOE().OffIndicatorsForOnActiveSkill();
            StopAOESkillCoroutine();
        }
    }
    public override void OnActiveSkill()
    {
        characterData.activeSkill.OnActiveSkill(characterData.data);
    }
    private void StopAOESkillCoroutine()
    {
        StopCoroutine(coOnIndicator);
        coOnIndicator = null;
    }
    private ActiveSkillAOE GetActiveSkillAOE()
    {   
        return characterData.activeSkill as ActiveSkillAOE;
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
        switch (testRot)
        {
            case true:
                transform.rotation = Quaternion.Lerp(transform.rotation, returnPos.rotation, Time.deltaTime * 5);
                float angle = Quaternion.Angle(transform.rotation, returnPos.rotation);

                var nowSpeed = animator.GetFloat("Speed");
                var downSpeed = Mathf.Lerp(0, 1, Time.deltaTime * 10f);
                animator.SetFloat("Speed", nowSpeed - downSpeed);

                if (angle <= 0)
                {
                    testRot = false;
                    UnitState = UnitState.Idle;
                    battleManager.OnReady();

                }
                break;
            case false:
                animator.SetFloat("Speed", pathFind.velocity.magnitude / characterData.data.moveSpeed);
                if (Vector3.Distance(returnPos.position, transform.position) <= 0.5f)
                {
                    testRot = true;
                    pathFind.isStopped = true;
                    transform.position = returnPos.position;
                }
                break;
        }
    }

    public override void ChangeUnitState(UnitState state)
    {
        if (BattleState == UnitBattleState.ActiveSkill || BattleState == UnitBattleState.NormalAttack)
        {
            lateReturn = (state == UnitState.ReturnPosition);
            return;
        }
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

        SkillCancle();      
    }

    //타겟이 없으면 Idle로 가고, 쿨타임 계산해서 바로 스킬 가능하면 사용, 아니라면 대기
    public override void NormalAttackEnd()
    {
        pathFind.isStopped = false;
        animator.SetTrigger("AttackEnd");
        base.NormalAttackEnd();

        lastNormalAttackTime = Time.time;

        if (lateReturn)
        {
            UnitState = UnitState.ReturnPosition;
        }
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
        pathFind.isStopped = false;
        animator.SetTrigger("ActiveEnd");
        lastNormalAttackTime = Time.time;
        base.ActiveSkillEnd();

        if (lateReturn)
        {
            UnitState = UnitState.ReturnPosition;
        }
        else
        {
            BattleState = UnitBattleState.BattleIdle;
        }
    }
    public override void AddBuff(BuffType type, float scale, float duration)
    {
        int idx = 0;
        for (int i = buffList.Count - 1; i >= 0; i--)
        {
            if (buffList[i].duration > duration)
            {
                idx = i;
                break;
            }
        }
        var icon = heroUI.AddIcon(type, duration, idx);
        Buff buff = new()
        {
            type = type,
            buffScale = scale,
            duration = duration,
            icon = icon
        };

        buff.removeBuff += RemoveBuff;
        buffList.Add(buff);

        bufferState.Buffer(type, scale);
    }
    public override void RemoveBuff(Buff buff)
    {
        heroUI.RemoveBuff(buff.icon);
        buffList.Remove(buff);

        bufferState.Buffer(buff.type, -buff.buffScale);
    }
}