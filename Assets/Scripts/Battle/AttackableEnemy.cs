using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class AttackableEnemy : AttackableUnit
{
    [SerializeField]
    protected List<AttackableHero> targetList;
    public void SetTargetList(List<AttackableHero> list) => targetList = list;

    //BattleManager���� targetList �� null�̸� ���� �ൿ ����
    protected virtual void SetTarget()
    {
        if (targetList.Count == 0)
        {
            target = null;
            return;
        }

        target = targetList.OrderBy(t => Vector3.Distance(t.transform.position, transform.position))
                          .FirstOrDefault();
    }

    public float skillDuration; // �ӽ� ����
    public override UnitState UnitState {
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
                    battleManager.GetHeroList(ref targetList);
                    pathFind.speed = heroData.stats.moveSpeed;
                    pathFind.isStopped = false;
                    EnemyBattleState = UnitBattleState.NormalAttack;
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

    UnitBattleState enemyBattleState;
    public UnitBattleState EnemyBattleState {
        get {
            return enemyBattleState;
        }
        set {
            if (value == enemyBattleState)
                return;
            enemyBattleState = value;
        }
    }

    protected override void Awake()
    {
        //charactorData = LoadTestData(); //�ӽ� ������ �ε�
        pathFind = transform.GetComponent<NavMeshAgent>();
        SetData();
        base.Awake();
    }

    public override void NormalAttack()
    {
    }
    public override void NormalSkill()
    {

    }
    public override void ActiveAttack()
    {
        activeStartTime = Time.time;
        BattleState = UnitBattleState.ActiveSkill;
        Logger.Debug("Skill");
    }

    protected override void IdleUpdate()
    {

    }
    protected override void BattleUpdate()
    {
        switch (EnemyBattleState)
        {
            case UnitBattleState.NormalAttack:
                //Ÿ���� ������ Ÿ�� ��ô
                if (target == null)
                {
                    SetTarget();
                    return;
                }

                //Ÿ������ �̵�
                pathFind.SetDestination(target.transform.position);

                //Ÿ�ٰ� ���� ���� �ȿ� ������, �Ϲݽ�ų �����̰�, ��Ÿ�� ������ �����ɶ�
                if (IsAttack && CanNormalAttack)
                {
                    lastNormalAttackTime = Time.time;
                    NormalAttackAction();
                }
                break;
            case UnitBattleState.ActiveSkill:
                if (Time.time - activeStartTime > skillDuration)
                {
                    EnemyBattleState = UnitBattleState.NormalAttack;
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


    [ContextMenu("Battle")]
    public override void SetTestBattle()
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