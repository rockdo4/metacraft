using System.Net.Sockets;
using UnityEngine;
using UnityEngine.AI;

public class AttackableHero : AttackableUnit
{
    protected HeroUi heroUI;

    private Transform returnPos;
    public void SetReturnPos(Transform tr) => returnPos = returnPos = tr;

    private Coroutine coOnIndicator;
    private Coroutine coOnAutoSkill;

    bool lateReturn = false;
    public override float UnitHp {
        get { return base.UnitHp; }
        set {
            base.UnitHp = value;
            heroUI.SetHp(UnitHp, MaxHp);
        }
    }

    protected override UnitState UnitState {
        get {
            return unitState;
        }
        set {

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
                case UnitState.ReturnPosition: // ���ġ
                    pathFind.isStopped = false;
                    pathFind.stoppingDistance = 0; //������ ����
                    pathFind.SetDestination(returnPos.position); //���ġ ��ġ ����
                    animator.Rebind();

                    BattleState = UnitBattleState.None;
                    nowUpdate = ReturnPosUpdate;


                    target = null;
                    isRotate = false;
                    heroUI.heroSkill.CancleSkill();
                    if (coOnAutoSkill != null)
                    {
                        StopCoroutine(coOnAutoSkill);
                        coOnAutoSkill = null;
                    }
                    break;
                case UnitState.MoveNext:
                    pathFind.isStopped = false;
                    lateReturn = false;
                    pathFind.SetDestination(transform.position);
                    nowUpdate = MoveNextUpdate;
                    break;
                case UnitState.Battle:
                    coOnAutoSkill = null;
                    pathFind.isStopped = false;
                    pathFind.speed = characterData.data.moveSpeed;
                    pathFind.stoppingDistance = minAttackDis;

                    battleManager.GetHeroList(ref heroList);
                    battleManager.GetCurrBtMgr().GetEnemyList(ref enemyList);

                    animator.SetFloat("Speed", 1);
                    BattleState = UnitBattleState.MoveToTarget;
                    nowUpdate = BattleUpdate;
                    OnPassiveSkill(enemyList, heroList);

                    break;
                case UnitState.Die:
                    pathFind.enabled = false;
                    OnDead(this);
                    gameObject.GetComponent<Collider>().enabled = false;
                    animator.SetTrigger("Die");

                    if (coOnAutoSkill != null)
                    {
                        StopCoroutine(coOnAutoSkill);
                        coOnAutoSkill = null;
                        heroUI.heroSkill.CancleSkill();
                    }
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
            if (unitState == UnitState.Die && value != UnitBattleState.None)
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

    bool isRotate = false;
    public override bool IsAuto {
        get {
            return isAuto;
        }
        set {
            isAuto = value;
            characterData.activeSkill.isAuto = isAuto;
            if (heroUI != null)
                heroUI.heroSkill.isAuto = isAuto;

            if (!isAuto)
            {
                SkillCancle();
                if (coOnAutoSkill != null)
                {
                    StopCoroutine(coOnAutoSkill);
                    heroUI.heroSkill.CancleSkill();
                    coOnAutoSkill = null;
                }
            }
        }
    }
    public bool isTutorial;

    private Transform[] tutorialPos = new Transform[2];
    public void SetTutorialPos(Transform[] tutorialPos) => this.tutorialPos = tutorialPos;
    public bool tutorialActive;


    public void Test1()
    {
        Time.timeScale = 0;
        characterData.activeSkill.targetPos = tutorialPos[0].localPosition;

        heroUI.heroSkill.isTutorialPos = true;
        heroUI.heroSkill.tutorialPos = tutorialPos[0];
        characterData.activeSkill.isTutorial = true;
        tutorialPos[0].gameObject.SetActive(true);
    }
    public void Test2()
    {
        Time.timeScale = 0;
        characterData.activeSkill.targetPos = tutorialPos[1].localPosition;

        heroUI.heroSkill.isTutorialPos = true;
        heroUI.heroSkill.tutorialPos = tutorialPos[1];
        characterData.activeSkill.isTutorial = true;
        tutorialPos[1].gameObject.SetActive(true);
    }

    //test
    public void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Keypad0))
        {
            Test1();
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            Test2();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        // �����ũ �������� �ӽ÷� �߰���
        InitData();
        pathFind = transform.GetComponent<NavMeshAgent>();
        characterData.InitSetting();
        SetData();

        unitState = UnitState.Idle;

        ResetCoolDown();
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
        characterData.activeSkill.ActorTransform = transform;

        var manager = FindObjectOfType<BattleManager>();
        if (manager != null)
            battleManager = manager;

        for (int i = 0; i < characterData.activeSkill.buffInfos.Count; i++)
        {
            characterData.activeSkill.buffInfos[i].buffLevel = characterData.activeSkill.skillLevel;
            characterData.activeSkill.buffInfos[i].addBuffState = characterData.activeSkill.addBuffState[i];
        }
        for (int i = 0; i < characterData.attacks[0].buffInfos.Count; i++)
        {
            characterData.attacks[0].buffInfos[i].buffLevel = characterData.attacks[0].skillLevel;
            characterData.attacks[0].buffInfos[i].addBuffState = characterData.attacks[0].addBuffState[i];
        }
        for (int i = 0; i < characterData.passiveSkill.buffInfos.Count; i++)
        {
            characterData.passiveSkill.buffInfos[i].buffLevel = characterData.passiveSkill.skillLevel;
            characterData.passiveSkill.buffInfos[i].addBuffState = characterData.passiveSkill.addBuffState[i];
        }
    }

    // Ui�� ����, Ui�� ��ų ��Ÿ�� ����
    public virtual void SetUi(HeroUi _heroUI)
    {
        heroUI = _heroUI;
        heroUI.heroSkill.
            Set(
            characterData.activeSkill.cooldown,
            characterData.activeSkill.skillDescription); //�ñر� ��Ÿ�� ���
        heroUI.heroSkill.
            SetActions(
            ReadyActiveSkill,
            PlayActiveSkillAnimation,
            OffSkillAreaIndicator,
            SkillCancle);
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
        isTutorial = GameManager.Instance.playerData.isTutorial;

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

        if (coOnAutoSkill != null)
        {
            StopCoroutine(coOnAutoSkill);
            coOnAutoSkill = null;
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
        if (!isTutorial)
        {
            if (IsAuto && BattleState != UnitBattleState.None && heroUI.heroSkill.IsCoolDown && !bufferState.silence)
            {
                SearchActiveTarget();
                if (activeTarget != null && InRangeActiveAttack)
                {
                    characterData.activeSkill.targetPos = activeTarget.transform.position;
                    if (coOnAutoSkill == null)
                    {
                        coOnAutoSkill = StartCoroutine(heroUI.heroSkill.OnAutoSkillActive(characterData.activeSkill));
                    }
                }
            }
        }
        //Ÿ���� ������ Ÿ���� ã���� Ÿ������ ����
        switch (BattleState)
        {
            //Ÿ�ٿ��� �̵����̰ų�, ���� ����߿� Ÿ���� ������ ��Ž��
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
                        {
                            BattleState = UnitBattleState.NormalAttack;
                        }
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
            case UnitBattleState.MoveToTarget: //Ÿ�ٿ��� �̵��� Ÿ�� �Ÿ� ���.
                if (FindNowAttack())
                {
                    BattleState = UnitBattleState.NormalAttack;
                }
                else if (InRangeMinNormalAttack)
                {
                    BattleState = UnitBattleState.BattleIdle;
                }
                else if (Time.time - lastNavTime > navDelay) //�Ϲݰ���, �нú� ��� �Ұ� �Ÿ��Ͻ� �̵�
                {
                    lastNavTime = Time.time;
                    pathFind.SetDestination(target.transform.position);
                }
                break;
            case UnitBattleState.BattleIdle:
                if (FindNowAttack())
                {
                    BattleState = UnitBattleState.NormalAttack;
                }
                else if (!InRangeMinNormalAttack)
                    BattleState = UnitBattleState.MoveToTarget;
                break;
            case UnitBattleState.NormalAttack:
                stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("NormalAttack") && stateInfo.normalizedTime >= .75f)
                {
                    NormalAttackEnd();
                }
                break;
            case UnitBattleState.ActiveSkill:
                stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("ActiveSkill") && stateInfo.normalizedTime >= 0.75f)
                {
                    ActiveSkillEnd();
                }
                break;
            case UnitBattleState.Stun:
                break;
        }
    }

    protected override void DieUpdate() { }

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
                break;
            case false:
                if (pathFind.isStopped)
                    pathFind.isStopped = false;
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
        if (state == UnitState.ReturnPosition)
        {
            if (BattleState == UnitBattleState.ActiveSkill || BattleState == UnitBattleState.NormalAttack || BattleState == UnitBattleState.Stun)
            {
                lateReturn = true;
                return;
            }
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

    //Ÿ���� ������ Idle�� ����, ��Ÿ�� ����ؼ� �ٷ� ��ų �����ϸ� ���, �ƴ϶�� ���
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
        pathFind.speed = characterData.data.moveSpeed;
        animator.SetTrigger("ActiveEnd");
        characterData.activeSkill.isTutorial = false;
        //if (coOnAutoSkill != null)
        //    StopCoroutine(coOnAutoSkill);
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
    public override void AddValueBuff(BuffInfo info, BuffIcon icon = null)
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

        bool isOverlap = false;
        foreach (var buff in buffList)
        {
            if (buff.buffInfo.id == info.id)
            {
                isOverlap = true;
                break;
            }
        }

        if (!isOverlap)
        {
            if (info.fraction != 0)
            {
                icon = heroUI.AddIcon(info.type, info.duration, idx, GameManager.Instance.GetSpriteByAddress($"state{info.sort}"));
            }
            base.AddValueBuff(info, icon);
        }
        else
        {
            BuffDurationUpdate(info.id, info.duration);
        }

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
        bool isOverlap = false;
        foreach (var buff in buffList)
        {
            if (buff.buffInfo.id == info.id)
            {
                isOverlap = true;
                break;
            }
        }

        if (!isOverlap)
        {
            if (info.fraction != 0)
            {
                icon = heroUI.AddIcon(info.type, info.duration, idx, GameManager.Instance.GetSpriteByAddress($"state{info.sort}"));
                base.AddStateBuff(info, attackableUnit, icon);
            }
        }
        else
            BuffDurationUpdate(info.id, info.duration);

        if (info.type == BuffType.Silence)
        {
            heroUI.IsSilence = true;
        }

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