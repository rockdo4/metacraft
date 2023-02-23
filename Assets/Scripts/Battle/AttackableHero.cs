using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class AttackableHero : AttackableUnit
{
    protected BattleHero heroUI;

    private Transform returnPos;
    public void SetReturnPos(Transform tr) => returnPos = returnPos = tr;

    //��Ʋ�� ���۵ɶ����� BattleManager���� ������ ����.
    protected List<AttackableEnemy> targetList;

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
                    pathFind.isStopped = true;
                    animator.SetTrigger("Idle");
                    nowUpdate = IdleUpdate;
                    break;
                case UnitState.ReturnPosition: // ���ġ
                    pathFind.isStopped = false;
                    animator.SetTrigger("Run");
                    pathFind.SetDestination(returnPos.position); //���ġ ��ġ ����
                    pathFind.stoppingDistance = 0; //������ ����
                    nowUpdate = ReturnPosUpdate;
                    break;
                case UnitState.MoveNext:
                    pathFind.isStopped = false;
                    animator.SetTrigger("Run");
                    nowUpdate = MoveNextUpdate;
                    break;
                case UnitState.Battle:
                    battleManager.GetEnemyList(ref targetList);
                    pathFind.stoppingDistance = characterData.attack.distance * 0.9f;
                    pathFind.speed = characterData.data.moveSpeed;
                    pathFind.isStopped = false;
                    BattleState = UnitBattleState.NormalAttack;
                    nowUpdate = BattleUpdate;
                    break;
                case UnitState.Die:
                    pathFind.isStopped = true;
                    animator.SetTrigger("Die");
                    float destroyDelay = animator.GetCurrentAnimatorStateInfo(0).length;
                    nowUpdate = DieUpdate;
                    Destroy(gameObject, destroyDelay + 1);
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
        }
    }

    protected override void Awake()
    {
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
        if (targetList.Count == 0)
        {
            target = null;
            return;
        }
        //���� ����� �� Ž��
        target = targetList.Where(t=>t.GetHp() > 0).OrderBy(t => Vector3.Distance(t.transform.position, transform.position))
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
    protected void SearchMaxHealthTarget()
    {
        if (targetList.Count == 0)
        {
            target = null;
            return;
        }
        //���� ����� �� Ž��
        var maxHp = targetList.Max(t => t.GetHp());
        target = targetList.Where(t => t.GetHp() == maxHp && (t.GetHp() > 0)).FirstOrDefault().GetComponent<AttackableUnit>();
    }
    protected void SearchMinHealthTarget()
    {
        if (targetList.Count == 0)
        {
            target = null;
            return;
        }
        //���� ����� �� Ž��
        var minHp = targetList.Min(t => t.GetHp());
        target = targetList.Where(t => (t.GetHp() == minHp) && (t.GetHp() > 0)).FirstOrDefault().GetComponent<AttackableUnit>();
    }
    protected virtual void SearchTarget()
    {
        if (target != null)
            animator.SetTrigger("Run");
    }


    public override void NormalAttack()
    {
    }
    public override void PassiveSkill()
    {
        Logger.Debug("Passive!");
        Invoke("TestPassiveEnd", 2);
        BattleState = UnitBattleState.PassiveSkill;
    }
    public override void ActiveAttack()
    {

        Invoke("TestActiveEnd", 2);
        BattleState = UnitBattleState.ActiveSkill;
    }
    public override void TestPassiveEnd()
    {
        lastNormalAttackTime = lastPassiveSkillTime = Time.time;
        BattleState = UnitBattleState.NormalAttack;
    }
    public override void TestActiveEnd()
    {
        lastNormalAttackTime = lastPassiveSkillTime = Time.time;
        BattleState = UnitBattleState.NormalAttack;
    }

    protected override void IdleUpdate()
    {

    }
    protected override void BattleUpdate()
    {
        switch (BattleState)
        {
            case UnitBattleState.NormalAttack:
                //Ÿ���� ������ Ÿ�� ��ô
                if (target == null)
                {
                    SearchTarget();
                    return;
                }

                //Ÿ������ �ٶ󺸱�
                Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 120);

                //�нú� ��ų �����̸� �нú� ���, �ƴ϶�� ��Ÿ
                if (IsPassiveAttack && CanPassiveSkillTime)
                {
                    PassiveSkillAction();
                }
                else if (IsNormalAttack && CanNormalAttackTime)
                {
                    animator.SetTrigger("Attack");
                    lastNormalAttackTime = Time.time;
                    NormalAttackAction();
                }
                 if(Time.time - lastNavTime  > navDelay) //SetDestination �� 0.2���� ������ ����
                {
                    lastNavTime = Time.time;
                    pathFind.SetDestination(target.transform.position);
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
        lastNormalAttackTime = Time.time;
    }
    public virtual void SetMoveNext()
    {
        UnitState = UnitState.MoveNext;
    }
    public virtual void SetReturn()
    {
        UnitState = UnitState.ReturnPosition;
    }

    public override void OnDamage(int dmg)
    {
        hp = Mathf.Max(hp - dmg, 0);

        heroUI.SetHp(hp);

        if (hp <= 0)
            UnitState = UnitState.Die;
    }
    private void OnDestroy()
    {
       battleManager.OnDeadHero(this);
    }
}