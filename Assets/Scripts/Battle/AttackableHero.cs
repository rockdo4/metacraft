using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class AttackableHero : AttackableUnit
{
    protected BattleHero heroUI;

    [SerializeField]
    private Transform returnPos;
    public void SetReturnPos(Transform tr) => returnPos = returnPos = tr; 
    public float skillDuration; // 임시 변수

    [SerializeField]
    protected List<AttackableEnemy> targetList;
    public void SetTargetList(List<AttackableEnemy> list) => targetList = list;

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

    public override UnitState UnitState {
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
                case UnitState.ReturnPosition: // 재배치
                    pathFind.isStopped = false;
                    pathFind.SetDestination(returnPos.position); //재배치 위치 설정
                    pathFind.stoppingDistance = 0; //가까이 가기
                    nowUpdate = ReturnPosUpdate;
                    break;
                case UnitState.MoveNext:
                    pathFind.isStopped = false;
                    nowUpdate = MoveNextUpdate;
                    break;
                case UnitState.Battle:
                    pathFind.stoppingDistance = heroData.normalAttack.distance; //가까이 가기
                    battleManager.GetEnemyList(ref targetList);
                    pathFind.speed = heroData.stats.moveSpeed;
                    pathFind.isStopped = false;
                    HeroBattleState = UnitBattleState.NormalAttack;
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

    UnitBattleState heroBattleState;
    public UnitBattleState HeroBattleState {
        get {
            return heroBattleState;
        }
        set {
            if (value == heroBattleState)
                return;
            heroBattleState = value;
        }
    }

    protected override void Awake()
    {
        //charactorData = LoadTestData(); //임시 데이터 로드
        pathFind = transform.GetComponent<NavMeshAgent>();
        SetData();

        unitState = UnitState.Idle;
        base.Awake();
    }

    // Ui와 연결, Ui에 스킬 쿨타임 연결
    public virtual void SetUi(BattleHero _heroUI)
    {
        heroUI = _heroUI;
        heroUI.heroSkill.Set(heroData.activeSkill.cooldown, NormalSkill); //궁극기 쿨타임과 궁극기 함수 등록
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
        switch (HeroBattleState)
        {
            case UnitBattleState.NormalAttack:
                //타겟이 없으면 타겟 추척
                if (target == null)
                {
                    SetTarget();
                    return;
                }

                //타겟으로 이동
                transform.LookAt(target.transform);
                pathFind.SetDestination(target.transform.position);

                //타겟과 일정 범위 안에 있으며, 일반스킬 상태이고, 쿨타임 조건이 충족될때
                if (IsAttack && CanNormalAttack)
                {
                    lastNormalAttackTime = Time.time;
                    NormalAttackAction();
                }
                else
                    //타겟으로 이동
                    pathFind.SetDestination(target.transform.position);

                break;
            case UnitBattleState.ActiveSkill:
                if (Time.time - activeStartTime > skillDuration)
                {
                    HeroBattleState = UnitBattleState.NormalAttack;
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

    public override void SetTestBattle()
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