using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class AttackableEnemy : AttackableUnit
{
    [SerializeField]
    protected List<AttackableHero> targetList;
    public void SetTargetList(List<AttackableHero> list) => targetList = list;

    //BattleManager에서 targetList 가 null이면 다음 행동 지시
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

    public float skillDuration; // 임시 변수
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
        //charactorData = LoadTestData(); //임시 데이터 로드
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
                //타겟이 없으면 타겟 추척
                if (target == null)
                {
                    SetTarget();
                    return;
                }

                //타겟으로 이동
                pathFind.SetDestination(target.transform.position);

                //타겟과 일정 범위 안에 있으며, 일반스킬 상태이고, 쿨타임 조건이 충족될때
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