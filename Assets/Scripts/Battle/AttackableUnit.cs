using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public abstract class AttackableUnit : MonoBehaviour
{
    protected BeltScrollBattleManager battleManager;

    [SerializeField]
    protected HeroData heroData;

    protected NavMeshAgent pathFind;

    protected AttackableUnit target;
    protected LayerMask targetType;

    [SerializeField] //hp를 인스펙터에서 보려고 작성. 
    protected int hp;

    protected float lastNormalAttackTime;
    protected float lastNormalSkillTime;
    protected float lastActiveSkillTime;

    protected float activeStartTime; //궁극기 유지 시간동안 평타, 일반스킬X

    protected Action nowUpdate;

    protected Action NormalAttackAction;
    protected Action NormalSkillAction;
    protected Action ActiveSkillAction;

    protected virtual void Awake()
    {
        battleManager = GameObject.FindObjectOfType<BeltScrollBattleManager>();
    }

    protected UnitState unitState;
    public virtual UnitState UnitState { get { return unitState; } set { unitState = value; } }

    protected UnitBattleState battleState;
    public virtual UnitBattleState BattleState { get { return battleState; }  set { battleState = value; } }

    protected bool CanNormalAttack {
        get {
            return (Time.time - lastNormalAttackTime) > heroData.normalAttack.cooldown;
        }
    }
    protected bool CanNormalSkill {
        get {
            return (Time.time - lastNormalSkillTime) > heroData.normalAttack.cooldown;
        }
    }

    protected bool InRange => Vector3.Distance(target.transform.position, transform.position) < heroData.normalAttack.distance;
    protected bool IsNormal => battleState != UnitBattleState.ActiveSkill && battleState != UnitBattleState.Stun;

    protected bool IsAttack {
        get {
            return IsNormal && InRange;
        }
    }

    [SerializeField]
    protected List<AttackableUnit> targetList;
    public void SetTargetList(List<AttackableUnit> list) => targetList = list;

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
    protected void SetData()
    {
        lastNormalAttackTime = Time.time;
        pathFind.stoppingDistance = heroData.normalAttack.distance;
        NormalAttackAction = NormalAttack;
        NormalSkillAction = NormalSkill;
        ActiveSkillAction = ActiveAttack;

        hp = heroData.stats.healthPoint;
    }
    protected void FixedUpdate()
    {
        NormalAttackUpdate();
        nowUpdate?.Invoke();
    }

    protected void NormalAttackUpdate()
    {
        if (Time.time - lastNormalAttackTime >= heroData.normalAttack.cooldown)
        {
            lastNormalAttackTime = Time.deltaTime;
        }
    }

    public abstract void SetTestBattle();

    public abstract void NormalAttack();
    public abstract void NormalSkill();
    public abstract void ActiveAttack();

    protected abstract void IdleUpdate();
    protected abstract void BattleUpdate();
    protected abstract void DieUpdate();
    protected abstract void MoveNextUpdate();
    protected abstract void ReturnPosUpdate();

    public abstract void OnDamage(int dmg);

}
