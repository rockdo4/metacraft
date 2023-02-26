using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public abstract class AttackableHero : AttackableUnit
{
    protected BattleHero heroUI;

    private Transform returnPos;
    public void SetReturnPos(Transform tr) => returnPos = returnPos = tr;

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
                case UnitState.Idle:
                    animator.SetTrigger("Idle");
                    pathFind.isStopped = true;
                    nowUpdate = IdleUpdate;
                    break;
                case UnitState.ReturnPosition: // ���ġ
                    animator.ResetTrigger("Run");

                    animator.SetFloat("IsBattle", 0);
                    animator.SetTrigger("Run");

                    pathFind.isStopped = false;
                    pathFind.SetDestination(returnPos.position); //���ġ ��ġ ����
                    pathFind.stoppingDistance = 0; //������ ����
                    nowUpdate = ReturnPosUpdate;
                    break;
                case UnitState.MoveNext:
                    animator.SetTrigger("Run");
                    BattleState = UnitBattleState.None;
                    pathFind.isStopped = false;
                    nowUpdate = MoveNextUpdate;
                    break;
                case UnitState.Battle:
                    animator.SetFloat("IsBattle", 1);
                    BattleState = UnitBattleState.MoveToTarget;
                    battleManager.GetEnemyList(ref enemyList);
                    pathFind.stoppingDistance = characterData.attack.distance * 0.9f;
                    pathFind.speed = characterData.data.moveSpeed;
                    pathFind.isStopped = false;
                    nowUpdate = BattleUpdate;
                    break;
                case UnitState.Die:
                    animator.SetTrigger("Die");
                    pathFind.isStopped = true;
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
            {
               // return;
            }
            battleState = value;

            //���°� �ٲ𶧸��� �ִϸ��̼� ȣ��
            switch (battleState)
            {
                case UnitBattleState.MoveToTarget:
                    animator.SetTrigger("Run");
                    break;
                case UnitBattleState.BattleIdle:
                    animator.SetTrigger("Idle");
                    break;
                case UnitBattleState.NormalAttack:
                    animator.SetTrigger("IsAttack");
                    animator.SetFloat("SkillType", 0);
                    //NormalAttackAction();
                    break;
                case UnitBattleState.PassiveSkill:
                    animator.SetTrigger("IsAttack");
                    animator.SetFloat("SkillType", 1);
                    //PassiveSkillAction();
                    break;
                case UnitBattleState.ActiveSkill:
                    animator.SetTrigger("IsAttack");
                    animator.SetFloat("SkillType", 2);
                    //ActiveSkillAction();
                    break;
                case UnitBattleState.Stun:
                    break;
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();

        characterData.InitSetting();
        //charactorData = LoadTestData(); //�ӽ� ������ �ε�
        pathFind = transform.GetComponent<NavMeshAgent>();
        SetData();

        unitState = UnitState.Idle;

        lastNormalAttackTime = lastPassiveSkillTime = Time.time;

    }

    // Ui�� ����, Ui�� ��ų ��Ÿ�� ����
    public virtual void SetUi(BattleHero _heroUI)
    {
        heroUI = _heroUI;
        heroUI.heroSkill.Set(characterData.activeSkill.cooldown, () => { BattleState = UnitBattleState.ActiveSkill; }); //�ñر� ��Ÿ�Ӱ� �ñر� �Լ� ���
    }

    protected abstract void SearchTarget(); //������ ĳ���Ͱ� Ž�� ������ �ٸ�.

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
        //Ÿ���� ������ Ÿ���� ã���� Ÿ������ ����
        switch (BattleState)
        {
            //Ÿ�ٿ��� �̵����̰ų�, ���� ����߿� Ÿ���� ������ ��Ž��
            case UnitBattleState.MoveToTarget:
            case UnitBattleState.BattleIdle:
                if (target == null)
                {
                    SearchTarget();
                    if (target != null)
                    {
                        if (InRangePassiveSkill && CanPassiveSkillTime)
                            BattleState = UnitBattleState.PassiveSkill;
                        else if (InRangeNormalAttack &&  CanNormalAttackTime)
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
            case UnitBattleState.MoveToTarget: //Ÿ�ٿ��� �̵��� Ÿ�� �Ÿ� ���.
                if (InRangePassiveSkill && CanPassiveSkillTime)
                    BattleState = UnitBattleState.PassiveSkill;
                else if (InRangeNormalAttack && CanNormalAttackTime)
                    BattleState = UnitBattleState.NormalAttack;
                else if (Time.time - lastNavTime > navDelay) //�Ϲݰ���, �нú� ��� �Ұ� �Ÿ��Ͻ� �̵�
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
                else if (InRangePassiveSkill && CanPassiveSkillTime) // �Ÿ�, ��Ÿ�� üũ
                {
                    BattleState = UnitBattleState.PassiveSkill;
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
        switch(pathFind.isStopped)
        {
            case true:
                transform.rotation = Quaternion.Lerp(transform.rotation, returnPos.rotation, Time.deltaTime * 5);
                float angle = Quaternion.Angle(transform.rotation, returnPos.rotation);

                if (angle <= 0)
                {
                    UnitState = UnitState.Idle;
                    battleManager.OnReady();
                }
                break;
            case false:
                if (Vector3.Distance(returnPos.position, transform.position) < 1f)
                {
                    pathFind.isStopped = true;
                    transform.position = returnPos.position;
                }
                break;
        }
    }

    public override void ChangeUnitState(UnitState state)
    {
        UnitState = state;
    }
    public override void ChangeBattleState(UnitBattleState state)
    {
        BattleState = state;
    }

    public override void OnDamage(int dmg, bool isCritical = false)
    {
        hp = Mathf.Max(hp - dmg, 0);

        heroUI.SetHp(hp);

        if (hp <= 0)
            UnitState = UnitState.Die;
    }
    
    public void DeadHero()
    {
        battleManager.OnDeadHero(this);
    }

    public void DestroyHero()
    {
        Destroy(gameObject,1);
    }

    //Ÿ���� ������ Idle�� ����, ��Ÿ�� ����ؼ� �ٷ� ��ų �����ϸ� ���, �ƴ϶�� ���
    public override void NormalAttackEnd()
    {
        lastNormalAttackTime = Time.time;
        if (target == null)
        {
            BattleState = UnitBattleState.BattleIdle;
        }
        else if (InRangePassiveSkill && CanPassiveSkillTime)
            BattleState = UnitBattleState.PassiveSkill;
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
        if (target == null)
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
        if (target == null)
        {
            BattleState = UnitBattleState.BattleIdle;
        }
        else if (InRangePassiveSkill && CanPassiveSkillTime)
        {
            BattleState = UnitBattleState.PassiveSkill;
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