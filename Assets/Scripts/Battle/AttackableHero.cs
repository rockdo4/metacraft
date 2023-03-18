using UnityEngine;
using UnityEngine.AI;

public class AttackableHero : AttackableUnit
{
    protected HeroUi heroUI;

    private Transform returnPos;
    public void SetReturnPos(Transform tr) => returnPos = returnPos = tr;

    private Coroutine coOnIndicator;

    bool lateReturn = false;

    public override float UnitHp
    {
        get { return base.UnitHp; }
        set
        {
            base.UnitHp = value;
            heroUI.SetHp(UnitHp, MaxHp);

        }
    }

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
            heroUI.heroState = unitState;
            switch (unitState)
            {
                case UnitState.None:
                    lateReturn = false;
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
                    animator.Rebind();

                    BattleState = UnitBattleState.None;
                    nowUpdate = ReturnPosUpdate;

                    heroUI.heroSkill.CancleSkill();

                    target = null;
                    lateReturn = false;
                    isRotate = false;
                    break;
                case UnitState.MoveNext:
                    pathFind.isStopped = false;
                    pathFind.SetDestination(transform.position);
                    nowUpdate = MoveNextUpdate;
                    break;
                case UnitState.Battle:
                    pathFind.isStopped = false;
                    pathFind.speed = characterData.data.moveSpeed;
                    pathFind.stoppingDistance = minAttackDis;

                    battleManager.GetHeroList(ref heroList);
                    battleManager.GetCurrBtMgr().GetEnemyList(ref enemyList);

                    animator.SetFloat("Speed", 1);

                    BattleState = UnitBattleState.MoveToTarget;
                    nowUpdate = BattleUpdate;

                    break;
                case UnitState.Die:
                    pathFind.enabled = false;
                    OnDead(this);
                    gameObject.GetComponent<Collider>().enabled = false;
                    animator.SetTrigger("Die");

                    nowUpdate = DieUpdate;
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
                    pathFind.isStopped = true;
                    animator.SetTrigger("Stun");
                    //Logger.Debug("Stun Trigger");
                    animator.ResetTrigger("Attack");
                    animator.ResetTrigger("AttackEnd");
                    break;
            }
        }
    }

    bool isRotate = false;
    public override bool IsAuto
    {
        get
        {
            return isAuto;
        }
        set
        {
            isAuto = value;
            characterData.activeSkill.isAuto = isAuto;
        }
    }

    protected void Awake()
    {
        // 어웨이크 에러땜에 임시로 추가함
        InitData();
        pathFind = transform.GetComponent<NavMeshAgent>();
        characterData.InitSetting();
        SetData();

        unitState = UnitState.Idle;

        ResetCoolDown();
    }

    private void Start()
    {
        foreach (var attack in characterData.attacks)
        {
            attack.SkillHolderTransform = effectCreateTransform ?? transform;
            attack.ActorTransform = transform;
        }
        characterData.activeSkill.ActorTransform = transform;

        var manager = FindObjectOfType<BattleManager>();
        if (manager != null)
            battleManager = manager;
    }

    // Ui와 연결, Ui에 스킬 쿨타임 연결
    public virtual void SetUi(HeroUi _heroUI)
    {
        heroUI = _heroUI;
        //BattleState = UnitBattleState.ActiveSkill;
        heroUI.heroSkill.
            Set(
            characterData.activeSkill.cooldown,
            characterData.activeSkill.skillDescription); //궁극기 쿨타임 등록
        heroUI.heroSkill.
            SetActions(
            ReadyActiveSkill,
            PlayActiveSkillAnimation,
            OffSkillAreaIndicator,
            SkillCancle);

        //heroUI.SetAuto(ref isAuto);
    }

    public override void ResetData()
    {
        isRotate = false;
        UnitState = UnitState.None;
        battleState = UnitBattleState.None;
        pathFind.stoppingDistance = 0f;

        lateReturn = false;
        lastNavTime = Time.time;
        ResetCoolDown();
        target = null;
        animator.Rebind();
        UnitHp = characterData.data.currentHp;

        heroUI.heroSkill.SetCoolTime(characterData.activeSkill.preCooldown);
        base.ResetData();
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
            GetActiveSkillAOE().ReadyEffectUntillOnActiveSkill();
            StopAOESkillCoroutine();
        }
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
        if (isAuto && heroUI.heroSkill.IsCoolDown && !bufferState.silence)
        {
            SearchActiveTarget();
            if (activeTarget != null && InRangeActiveAttack)
            {
                heroUI.heroSkill.OnDownSkill();
                characterData.activeSkill.targetPos = activeTarget.transform.position;

                StartCoroutine(heroUI.heroSkill.OnAutoSkillActive(characterData.activeSkill));
            }
        }
        //타겟이 없을때 타겟을 찾으면 타겟으로 가기
        switch (BattleState)
        {
            //타겟에게 이동중이거나, 공격 대기중에 타겟이 죽으면 재탐색
            case UnitBattleState.MoveToTarget:
            case UnitBattleState.BattleIdle:
                animator.SetFloat("Speed", pathFind.velocity.magnitude / characterData.data.moveSpeed);
                if ((CharacterJob)GetUnitData().data.job == CharacterJob.shooter)
                {
                    if (Time.time - lastSearchTime >= searchDelay)
                    {
                        lastSearchTime = Time.time;
                        SearchAi();
                    }
                }
                if (IsAlive(target) && target != this)
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
                        if (FindNowAttack())
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
                if (FindNowAttack())
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
                if (FindNowAttack())
                    BattleState = UnitBattleState.NormalAttack;
                else if (!InRangeMinNormalAttack)
                    BattleState = UnitBattleState.MoveToTarget;
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
        switch (isRotate)
        {
            case true:
                //transform.rotation = Quaternion.Lerp(transform.rotation, returnPos.rotation, Time.deltaTime * 5);
                //float angle = Quaternion.Angle(transform.rotation, returnPos.rotation);

                //var nowSpeed = animator.GetFloat("Speed");
                //var downSpeed = Mathf.Lerp(0, 1, Time.deltaTime * 10f);
                //animator.SetFloat("Speed", nowSpeed - downSpeed);

                //if (angle <= 0)
                {
                    transform.rotation = returnPos.rotation;
                    isRotate = false;
                    UnitState = UnitState.Idle;

                    if (battleManager.tree.CurNode.type == TreeNodeTypes.Threat)
                    {
                        if (!battleManager.isMiddleBossAlive)
                        {
                            battleManager.OnReady();
                        }
                    }
                    else
                    {
                        battleManager.OnReady();
                    }
                }
                break;
            case false:
                animator.SetFloat("Speed", pathFind.velocity.magnitude / characterData.data.moveSpeed);
                if (Vector3.Distance(returnPos.position, transform.position) <= 0.5f)
                {
                    isRotate = true;
                    pathFind.isStopped = true;
                    transform.position = returnPos.position;
                }
                break;
        }
    }

    public override void ChangeUnitState(UnitState state)
    {
        //Logger.Debug(state);
        if (BattleState == UnitBattleState.ActiveSkill || BattleState == UnitBattleState.NormalAttack || BattleState == UnitBattleState.Stun)
        {
            lateReturn = true;
            return;
        }
        UnitState = state;
    }

    public override void ChangeBattleState(UnitBattleState state)
    {
        BattleState = state;
    }

    public override void OnDamage(AttackableUnit attackableUnit, CharacterSkill skill)
    {
        base.OnDamage(attackableUnit, skill);
        heroUI.SetHp(UnitHp, MaxHp);

    }

    public override void OnDead(AttackableUnit unit)
    {
        battleManager.OnDeadHero((AttackableHero)unit);
        heroUI.SetDieImage();
        //enemyList.Clear();

        SkillCancle();
    }

    //타겟이 없으면 Idle로 가고, 쿨타임 계산해서 바로 스킬 가능하면 사용, 아니라면 대기
    public override void NormalAttackEnd()
    {
        pathFind.isStopped = false;
        animator.SetTrigger("AttackEnd");
        base.NormalAttackEnd();

        lastNormalAttackTime[nowAttack] = Time.time;

        if (lateReturn)
        {
            UnitState = UnitState.ReturnPosition;
        }
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

        if (lateReturn)
        {
            UnitState = UnitState.ReturnPosition;
        }
        else
        {
            BattleState = UnitBattleState.BattleIdle;
        }
    }

    public override void AddValueBuff(BuffInfo info, int anotherValue = 0, BuffIcon icon = null)
    {
        int idx = 0;
        for (int i = buffList.Count - 1; i >= 0; i--)
        {
            if (buffList[i].buffInfo.duration > info.duration)
            {
                idx = i;
                break;
            }
        }

        if (buffList.Find(t => t.buffInfo.id == info.id) == null)
        {
            if (info.fraction != 0)
            {
                icon = heroUI.AddIcon(info.type, info.duration, idx);
            }
        }
        else
        {
            BuffDurationUpdate(info.id, info.duration);
        }


        base.AddValueBuff(info, anotherValue, icon);
        if (info.type == BuffType.MaxHealthIncrease)
        {
            heroUI.SetHp(UnitHp, MaxHp);
        }
    }

    public override void AddStateBuff(BuffInfo info, AttackableUnit attackableUnit = null, BuffIcon icon = null)
    {
        int idx = 0;
        for (int i = buffList.Count - 1; i >= 0; i--)
        {
            if (buffList[i].buffInfo.duration > info.duration)
            {
                idx = i;
                break;
            }
        }

        if (buffList.Find(t => t.buffInfo.id == info.id) == null)
        {
            if (info.fraction != 0)
            {
                icon = heroUI.AddIcon(info.type, info.duration, idx);
            }
        }
        else
            BuffDurationUpdate(info.id, info.duration);

        if (info.type == BuffType.Silence)
        {
            heroUI.IsSilence = true;
        }

        base.AddStateBuff(info, attackableUnit, icon);
    }

    public override void StunEnd()
    {
        if (unitState == UnitState.Die)
        {
            animator.SetTrigger("Die");
            return;
        }
        base.StunEnd();
        if (lateReturn)
        {
            UnitState = UnitState.ReturnPosition;
        }
        else
        {
            BattleState = UnitBattleState.BattleIdle;
        }
        //Logger.Debug("Stun End");
    }

    public override void RemoveBuff(Buff buff)
    {
        base.RemoveBuff(buff);
        if (buff != null)
            heroUI.RemoveBuff(buff.icon);

        if (buff.buffInfo.type == BuffType.MaxHealthIncrease)
        {
            heroUI.SetHp(UnitHp, MaxHp);
        }
        if (buff.buffInfo.type == BuffType.Silence)
        {
            heroUI.IsSilence = false;
        }
    }
}