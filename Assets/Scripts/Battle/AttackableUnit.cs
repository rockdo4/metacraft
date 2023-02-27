using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public abstract class AttackableUnit : MonoBehaviour
{
    protected TestBattleManager battleManager;

    [SerializeField]
    protected CharacterDataBundle characterData;
    public CharacterDataBundle GetUnitData() => characterData;

    protected NavMeshAgent pathFind;

    protected AttackableUnit target;
    [SerializeField]
    protected List<AttackableHero> heroList;
    [SerializeField]
    protected List<AttackableEnemy> enemyList;

    [SerializeField]
    protected int hp;
    public int GetHp() => hp;

    protected float lastNormalAttackTime;
    protected float lastPassiveSkillTime;
    protected float lastActiveSkillTime;

    protected float lastNavTime;
    protected float navDelay = 0.2f;

    protected Action nowUpdate;

    protected Action NormalAttackAction;
    protected Action PassiveSkillAction;
    protected Action ActiveSkillAction;

    protected bool moveTarget;

    protected Animator animator;

    [SerializeField]
    protected UnitState unitState;
    protected virtual UnitState UnitState { get; set; }

    [SerializeField]
    protected UnitBattleState battleState;
    protected virtual UnitBattleState BattleState { get; set; }

    protected bool CanNormalAttackTime {
        get {
            return (Time.time - lastNormalAttackTime) > characterData.attack.cooldown;
        }
    }
    protected bool CanPassiveSkillTime {
        get {
            return (Time.time - lastPassiveSkillTime) > characterData.passiveSkill.cooldown;
        }
    }

    protected bool InRangeNormalAttack => Vector3.Distance(target.transform.position, transform.position) < characterData.attack.distance;
    protected bool InRangePassiveSkill => Vector3.Distance(target.transform.position, transform.position) < characterData.passiveSkill.distance;
    protected bool NonActiveSkill => battleState != UnitBattleState.ActiveSkill && battleState != UnitBattleState.Stun;

    protected virtual void Awake()
    {
        battleManager = FindObjectOfType<TestBattleManager>();
        animator = GetComponentInChildren<Animator>();
    }
    protected void SetData()
    {
        pathFind.stoppingDistance = characterData.attack.distance * 0.9f;

        NormalAttackAction = NormalAttack;
        PassiveSkillAction = PassiveSkill;
        ActiveSkillAction = ActiveSkill;

        hp = characterData.data.healthPoint;

        //animator.SetFloat("CharID", 0);
    }
    protected void FixedUpdate()
    {
        nowUpdate?.Invoke();
    }

    public abstract void ChangeUnitState(UnitState state);
    public abstract void ChangeBattleState(UnitBattleState status);

    public abstract void NormalAttack();
    public abstract void PassiveSkill();
    public abstract void ActiveSkill();

    public abstract void NormalAttackEnd();
    public abstract void PassiveSkillEnd();
    public abstract void ActiveSkillEnd();

    protected abstract void IdleUpdate();
    protected abstract void BattleUpdate();
    protected abstract void DieUpdate();
    protected abstract void MoveNextUpdate();
    protected abstract void ReturnPosUpdate();

    public abstract void OnDamage(int dmg, bool isCritical = false);

    public void SearchNearbyTarget<T>(List<T> list) where T : AttackableUnit
    {
        if (list.Count == 0)
        {
            target = null;
            return;
        }
        //가장 가까운 적 탐색
        target = list.Where(t => t.GetHp() > 0).OrderBy(t => Vector3.Distance(t.transform.position, transform.position))
                          .FirstOrDefault();
    }

    public AttackableUnit GetSearchTargetInAround<T>(List<T> list, float dis) where T : AttackableUnit
    {
        AttackableUnit minTarget = null;
        if (list.Count == 0)
        {
            minTarget = null;
            return null;
        }
        //가장 가까운 적 탐색
        minTarget = list.Where(t => (t.GetHp() > 0) && Vector3.Distance(transform.position, t.transform.position) <=  dis)
            .OrderBy(t => Vector3.Distance(t.transform.position, transform.position))
                          .FirstOrDefault();

        return minTarget;
    }

    protected void SearchMaxHealthTarget<T>(List<T> list) where T : AttackableUnit
    {
        if (list.Count == 0)
        {
            target = null;
            return;
        }
        //가장 가까운 적 탐색
        var maxHp = list.Max(t => t.GetHp());
        target = list.Where(t => t.GetHp() == maxHp && (t.GetHp() > 0)).FirstOrDefault().GetComponent<AttackableUnit>();
    }

    protected void SearchMinHealthTarget<T>(List<T> list) where T : AttackableUnit
    {
        if (list.Count == 0)
        {
            target = null;
            return;
        }
        //가장 가까운 적 탐색
        var minHp = list.Min(t => t.GetHp());
        target = list.Where(t => (t.GetHp() == minHp) && (t.GetHp() > 0)).FirstOrDefault().GetComponent<AttackableUnit>();
    }

    // Test
    public abstract void OnDead(AttackableUnit unit);
    public void DestroyUnit()
    {
        Destroy(gameObject, 1f);
    }
}