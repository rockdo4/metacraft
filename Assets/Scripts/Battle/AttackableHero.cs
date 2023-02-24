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
                    Logger.Debug("Idle");
                    break;
                case UnitState.ReturnPosition: // ���ġ
                    animator.SetTrigger("Run");
                    pathFind.isStopped = false;
                    pathFind.SetDestination(returnPos.position); //���ġ ��ġ ����
                    pathFind.stoppingDistance = 0; //������ ����
                    nowUpdate = ReturnPosUpdate;
                    Logger.Debug("Run");
                    break;
                case UnitState.MoveNext:
                    animator.SetTrigger("Run");
                    BattleState = UnitBattleState.None;
                    pathFind.isStopped = false;
                    nowUpdate = MoveNextUpdate;
                    Logger.Debug("Run");
                    break;
                case UnitState.Battle:
                    BattleState = UnitBattleState.MoveToTarget;
                    battleManager.GetEnemyList(ref enemyList);
                    pathFind.stoppingDistance = characterData.attack.distance * 0.9f;
                    pathFind.speed = characterData.data.moveSpeed;
                    pathFind.isStopped = false;
                    nowUpdate = BattleUpdate;
                    break;
                case UnitState.Die:
                    pathFind.isStopped = true;
                    animator.SetTrigger("Die");
                    Logger.Debug("Die");
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

            switch(battleState)
            {
                case UnitBattleState.MoveToTarget:
                    animator.SetTrigger("MoveToTarget");
                    Logger.Debug("MoveToTarget");
                    break;
                case UnitBattleState.NormalAttack:
                    break;
                case UnitBattleState.PassiveSkill:
                    break;
                case UnitBattleState.ActiveSkill:
                    Logger.Debug("ActiveSkill");
                    break;
                case UnitBattleState.Stun:
                    break;
            }
        }
    }

    protected override void Awake()
    {
        characterData.InitSetting();
        //charactorData = LoadTestData(); //�ӽ� ������ �ε�
        pathFind = transform.GetComponent<NavMeshAgent>();
        SetData();

        unitState = UnitState.Idle;
        base.Awake();
    }

    // Ui�� ����, Ui�� ��ų ��Ÿ�� ����
    public virtual void SetUi(BattleHero _heroUI)
    {
        heroUI = _heroUI;
        heroUI.heroSkill.Set(characterData.activeSkill.cooldown, ActiveAttack); //�ñر� ��Ÿ�Ӱ� �ñر� �Լ� ���
    }

    //BattleManager���� targetList �� null�̸� SetReturnPos �� ��������
    protected void SearchNearbyTarget()
    {
        if (enemyList.Count == 0)
        {
            target = null;
            return;
        }
        //���� ����� �� Ž��
        target = enemyList.Where(t=>t.GetHp() > 0).OrderBy(t => Vector3.Distance(t.transform.position, transform.position))
                          .FirstOrDefault();
    }
    protected bool ContainTarget(List<AttackableUnit> targetList,ref AttackableUnit target,float distance)
    {
        float minDist = float.MaxValue;
        bool targetChanged = false;

        foreach (var unit in targetList)
        {
            float dist = Vector3.Distance(unit.transform.position, transform.position);

            if (dist <= distance && dist < minDist)
            {
                target = unit;
                minDist = dist;
                targetChanged = true;
            }
        }

        return targetChanged;
    }
    protected bool ContainTarget(AttackableUnit target, float distance)
    {
        return Vector3.Distance(target.transform.position, transform.position) <= distance;
    }
    protected void SearchMaxHealthTarget()
    {
        if (enemyList.Count == 0)
        {
            target = null;
            return;
        }
        //���� ����� �� Ž��
        var maxHp = enemyList.Max(t => t.GetHp());
        target = enemyList.Where(t => t.GetHp() == maxHp && (t.GetHp() > 0)).FirstOrDefault().GetComponent<AttackableUnit>();
    }
    protected void SearchMinHealthTarget()
    {
        if (enemyList.Count == 0)
        {
            target = null;
            return;
        }
        //���� ����� �� Ž��
        var minHp = enemyList.Min(t => t.GetHp());
        target = enemyList.Where(t => (t.GetHp() == minHp) && (t.GetHp() > 0)).FirstOrDefault().GetComponent<AttackableUnit>();
    }
    protected virtual void SearchTarget()
    {
        if(target != null)
            BattleState = UnitBattleState.MoveToTarget;

    }


    public override void NormalAttack()
    {
        Logger.Debug("NormalAttack!"); 
        animator.SetTrigger("Attack");
    }

    public void HeroPassiveSkill()
    {
        Logger.Debug("PassiveSkill!");
        BattleState = UnitBattleState.PassiveSkill;
    }
    public void HeroActiveSkill()
    {
        Logger.Debug("Active!");
        Invoke("TestActiveEnd", 1);
        BattleState = UnitBattleState.ActiveSkill;
    }
    public void TestSkillEnd()
    {
        lastPassiveSkillTime = Time.time;
        BattleState = UnitBattleState.NormalAttack;
    }

    public override void PassiveSkill()
    {
    }
    public override void ActiveAttack()
    {
        Logger.Debug("ActiveAttack!");
        Invoke("TestActiveEnd",1);
        BattleState = UnitBattleState.ActiveSkill;
    }
    protected override void IdleUpdate()
    {

    }
    protected override void BattleUpdate()
    {
        switch (BattleState)
        {
            case UnitBattleState.MoveToTarget:
                if (target == null)
                {
                    SearchTarget();
                    return;
                }
                if (ContainTarget(target, characterData.attack.distance))
                {
                    BattleState = UnitBattleState.NormalAttack;
                }
                if (Time.time - lastNavTime > navDelay) //SetDestination �� 0.2���� ������ ����
                {
                    lastNavTime = Time.time;
                    pathFind.SetDestination(target.transform.position);
                }
                break;
            case UnitBattleState.NormalAttack:
                //Ÿ���� ������ Ÿ�� ��ô
                if (target == null)
                {
                    SearchTarget();
                    return;
                }
                if (!ContainTarget(target, characterData.attack.distance))
                {
                    BattleState = UnitBattleState.MoveToTarget;
                }

                //Ÿ������ �ٶ󺸱�
                Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 120);

                //�нú� ��ų �����̸� �нú� ���, �ƴ϶�� ��Ÿ
                if (IsPassiveAttack && CanPassiveSkillTime && CanNormalAttackTime)
                {
                    lastPassiveSkillTime = Time.time;
                    animator.SetTrigger("Passive");
                    PassiveSkillAction();
                }
                else if (IsNormalAttack && CanNormalAttackTime)
                {
                    lastNormalAttackTime = Time.time;
                    NormalAttackAction();
                }
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

    public override void SetBattle()
    {
        UnitState = UnitState.Battle;
    }
    public virtual void SetMoveNext()
    {
        UnitState = UnitState.MoveNext;
    }
    public virtual void SetReturn()
    {
        UnitState = UnitState.ReturnPosition;
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
        Destroy(gameObject);
    }

    //???? ????????? ??????, ???��? NormalAttack?? ????? ??? ???
    public override void TestPassiveEnd() { }
    public override void TestActiveEnd()
    {
        BattleState = UnitBattleState.NormalAttack;
    }
}