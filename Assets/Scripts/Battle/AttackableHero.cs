using System.Collections.Generic;
using System.Linq;
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
                    nowUpdate = IdleUpdate;
                    break;
                case UnitState.ReturnPosition: // ���ġ
                    pathFind.isStopped = false;
                    pathFind.SetDestination(returnPos.position); //���ġ ��ġ ����
                    pathFind.stoppingDistance = 0; //������ ����
                    nowUpdate = ReturnPosUpdate;
                    break;
                case UnitState.MoveNext:
                    pathFind.isStopped = false;
                    nowUpdate = MoveNextUpdate;
                    break;
                case UnitState.Battle:
                    battleManager.GetEnemyList(ref targetList);
                    pathFind.stoppingDistance = heroData.normalAttack.distance;
                    pathFind.speed = heroData.stats.moveSpeed;
                    pathFind.isStopped = false;
                    BattleState = UnitBattleState.NormalAttack;
                    nowUpdate = BattleUpdate;
                    break;
                case UnitState.Die:
                    pathFind.isStopped = true;
                    nowUpdate = DieUpdate;
                    Destroy(gameObject, 1);
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
        heroUI.heroSkill.Set(heroData.activeSkill.cooldown, PassiveSkill); //�ñر� ��Ÿ�Ӱ� �ñر� �Լ� ���
    }

    //BattleManager���� targetList �� null�̸� SetReturnPos �� ��������
    protected void SearchNearbyEnemy()
    {
        if (targetList.Count == 0)
        {
            target = null;
            return;
        }

        //���� ����� �� Ž��
        target = targetList.OrderBy(t => Vector3.Distance(t.transform.position, transform.position))
                          .FirstOrDefault();
    }
    protected abstract void SearchTarget();

    public override void NormalAttack()
    {
    }
    public override void PassiveSkill()
    {
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

                //transform.LookAt(target.transform);

                //�нú� ��ų �����̸� �нú� ���, �ƴ϶�� ��Ÿ
                if (IsPassiveAttack && CanPassiveSkillTime)
                {
                    PassiveSkillAction();
                }
                else if (IsNormalAttack && CanNormalAttackTime)
                {
                    lastNormalAttackTime = Time.time;
                    NormalAttackAction();
                }
                //else if(!InRangeNormalAttack) // ���� �ָ� ������ �ִٸ�
                //{
                //    target = null; //Ÿ���� �缳���ϱ� ���� null��
                //}
                else if(Time.time - lastNavTime  > navDelay) //SetDestination �� 0.2���� ������ ����
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
                    battleManager.OnReady();
                    UnitState = UnitState.Idle;
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