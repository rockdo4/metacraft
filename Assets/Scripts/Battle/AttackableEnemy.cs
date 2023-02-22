using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public abstract class AttackableEnemy : AttackableUnit
{
    [SerializeField]
    protected List<AttackableHero> targetList;
    public void SetTargetList(List<AttackableHero> list) => targetList = list;

    public float skillDuration; // �ӽ� ����
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
                case UnitState.Idle:
                    pathFind.isStopped = true;
                    nowUpdate = IdleUpdate;
                    break;
                case UnitState.Battle:
                    pathFind.stoppingDistance = heroData.normalAttack.distance; //������ ����
                    battleManager.GetHeroList(ref targetList);
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
                default:
                    Logger.Debug("Error");
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
        base.Awake();
    }

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
        Invoke("TestPassiveEnd", 2);
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

                //Ÿ������ �̵�
                transform.LookAt(target.transform);
                pathFind.SetDestination(target.transform.position);

                //Ÿ�ٰ� ���� ���� �ȿ� ������, �Ϲݽ�ų �����̰�, ��Ÿ�� ������ �����ɶ�
                if (IsNormalAttack && CanNormalAttackTime)
                {
                    lastNormalAttackTime = Time.time;
                    NormalAttackAction();
                }
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

    }


    [ContextMenu("Battle")]
    public override void SetBattle()
    {
        UnitState = UnitState.Battle;
    }

    public override void OnDamage(int dmg)
    {
        hp = Mathf.Max(hp - dmg, 0);
        if (hp <= 0)
            UnitState = UnitState.Die;
    }

    private void OnDestroy()
    {
        battleManager.OnDeadEnemy(this);
    }
}