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
                case UnitState.ReturnPosition: // 재배치
                    animator.SetTrigger("ReturnPosition");
                    pathFind.isStopped = false;
                    pathFind.SetDestination(returnPos.position); //재배치 위치 설정
                    pathFind.stoppingDistance = 0; //가까이 가기
                    nowUpdate = ReturnPosUpdate;
                    break;
                case UnitState.MoveNext:
                    animator.SetTrigger("MoveNext");
                    BattleState = UnitBattleState.None;
                    pathFind.isStopped = false;
                    nowUpdate = MoveNextUpdate;
                    break;
                case UnitState.Battle:
                    animator.SetTrigger("Battle");
                    animator.SetTrigger("MoveToTarget");

                    BattleState = UnitBattleState.MoveToTarget;
                    battleManager.GetEnemyList(ref enemyList);

                    lastNormalAttackTime = lastPassiveSkillTime = Time.time;

                    pathFind.stoppingDistance = characterData.attack.distance * 0.9f;
                    pathFind.speed = characterData.data.moveSpeed;
                    pathFind.isStopped = false;
                    nowUpdate = BattleUpdate;
                    break;
                case UnitState.Die:
                    pathFind.isStopped = true;
                    animator.SetTrigger("Die");
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

            //상태가 바뀔때마다 애니메이션 호출
            switch(battleState)
            {
                case UnitBattleState.MoveToTarget:
                    animator.SetTrigger("MoveToTarget");
                    break;
                case UnitBattleState.BattleIdle:
                    animator.SetTrigger("BattleIdle");
                    break;
                case UnitBattleState.NormalAttack:
                    lastNormalAttackTime = Time.time;
                    NormalAttackAction();

                    animator.SetTrigger("NormalAttack");
                    break;
                case UnitBattleState.PassiveSkill:
                    lastPassiveSkillTime = Time.time;
                    PassiveSkillAction();

                    animator.SetTrigger("PassiveSkill");
                    break;
                case UnitBattleState.ActiveSkill:
                    ActiveSkillAction();

                    animator.SetTrigger("NormalAttack");
                    break;
                case UnitBattleState.Stun:
                    break;
            }
        }
    }

    protected override void Awake()
    {
        characterData.InitSetting();
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
        heroUI.heroSkill.Set(characterData.activeSkill.cooldown, () => { BattleState = UnitBattleState.ActiveSkill; }); //궁극기 쿨타임과 궁극기 함수 등록
    }

    //BattleManager에서 targetList 가 null이면 SetReturnPos 를 실행해줌
    protected void SearchNearbyTarget()
    {
        if (enemyList.Count == 0)
        {
            target = null;
            return;
        }
        //가장 가까운 적 탐색
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
        //가장 가까운 적 탐색
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
        //가장 가까운 적 탐색
        var minHp = enemyList.Min(t => t.GetHp());
        target = enemyList.Where(t => (t.GetHp() == minHp) && (t.GetHp() > 0)).FirstOrDefault().GetComponent<AttackableUnit>();
    }

    protected abstract void SearchTarget(); //각각의 캐릭터가 탐색 조건이 다름.

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
        //타겟이 없을때 타겟을 찾으면 타겟으로 가기
        switch (BattleState)
        {
            //타겟에게 이동중이거나, 공격 대기중에 타겟이 죽으면 재탐색
            case UnitBattleState.MoveToTarget:
            case UnitBattleState.BattleIdle:
                if (target == null)
                {
                    SearchTarget();
                    if (target != null)
                    {
                        if (InRangePassiveSkill)
                            BattleState = UnitBattleState.PassiveSkill;
                        else if (InRangeNormalAttack)
                            BattleState = UnitBattleState.NormalAttack;
                        else
                            BattleState = UnitBattleState.MoveToTarget;
                    }
                    return;
                }
                Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 120);
                break;
        }

        switch (BattleState)
        {
            case UnitBattleState.MoveToTarget: //타겟에게 이동중 타겟 거리 계산.
                if (InRangePassiveSkill && CanPassiveSkillTime)
                    BattleState = UnitBattleState.PassiveSkill;
                else if (InRangeNormalAttack && CanNormalAttackTime)
                    BattleState = UnitBattleState.NormalAttack;
                else if (Time.time - lastNavTime > navDelay) //일반공격, 패시브 사용 불가 거리일시 이동
                {
                    lastNavTime = Time.time;
                    pathFind.SetDestination(target.transform.position);
                }
                break;
            case UnitBattleState.BattleIdle:
                if (InRangePassiveSkill && CanPassiveSkillTime) // 거리, 쿨타임 체크
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
        Destroy(gameObject,1);
    }

    //타겟이 없으면 Idle로 가고, 쿨타임 계산해서 바로 스킬 가능하면 사용, 아니라면 대기
    public override void NormalAttackEnd()
    {
        if (target == null)
            BattleState = UnitBattleState.BattleIdle;
        else if (InRangePassiveSkill)
            BattleState = UnitBattleState.PassiveSkill;
        else if (InRangeNormalAttack)
        {
            BattleState = UnitBattleState.None;
            animator.ResetTrigger("NormalAttack");
            BattleState = UnitBattleState.NormalAttack;
        }
        else
            BattleState = UnitBattleState.BattleIdle;
       // BattleState = UnitBattleState.BattleIdle;
    }
    public override void PassiveSkillEnd()
    {
        if (target == null)
            BattleState = UnitBattleState.BattleIdle;
        else if (InRangePassiveSkill)
        {
            BattleState = UnitBattleState.None;
            animator.ResetTrigger("PassiveSkill");
            BattleState = UnitBattleState.PassiveSkill;
        }
        else if (InRangeNormalAttack)
            BattleState = UnitBattleState.NormalAttack;
        else
            BattleState = UnitBattleState.BattleIdle;
        //BattleState = UnitBattleState.BattleIdle;
    }
    public override void ActiveSkillEnd()
    {
        if (target == null)
            BattleState = UnitBattleState.BattleIdle;
        else if (InRangePassiveSkill)
            BattleState = UnitBattleState.PassiveSkill;
        else if (InRangeNormalAttack)
            BattleState = UnitBattleState.NormalAttack;
        else
            BattleState = UnitBattleState.BattleIdle;
        //BattleState = UnitBattleState.BattleIdle;
    }
}