using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public abstract class AttackableEnemy : AttackableUnit
{
    [SerializeField]
    public void SetTargetList(List<AttackableHero> list) => heroList = list;

    private AttackedDamageUI floatingDamageText;
    private HpBarManager hpBarManager;

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
                    animator.SetTrigger("Idle");
                    nowUpdate = IdleUpdate;
                    break;
                case UnitState.Battle:
                    pathFind.stoppingDistance = characterData.attack.distance * 0.9f; //������ ����
                    //animator.SetTrigger("MoveToTarget");
                    battleManager.GetHeroList(ref heroList);
                    pathFind.speed = characterData.data.moveSpeed;
                    pathFind.isStopped = false;
                    BattleState = UnitBattleState.NormalAttack;
                    nowUpdate = BattleUpdate;
                    break;
                case UnitState.Die:
                    pathFind.isStopped = true;
                    animator.SetTrigger("Die");
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

            switch (battleState)
            {
                case UnitBattleState.MoveToTarget:
                    animator.SetTrigger("MoveToTarget");
                    Logger.Debug("MoveToTarget");
                    break;
                case UnitBattleState.NormalAttack:
                    break;
                case UnitBattleState.PassiveSkill:
                    animator.SetTrigger("PassiveSkill");
                    Logger.Debug("Attack");
                    break;
                case UnitBattleState.ActiveSkill:
                    animator.SetTrigger("NormalAttack");
                    Logger.Debug("Attack");
                    break;
                case UnitBattleState.Stun:
                    break;
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        //charactorData = LoadTestData(); //�ӽ� ������ �ε�
        pathFind = transform.GetComponent<NavMeshAgent>();
        SetData();

        floatingDamageText = GetComponent<AttackedDamageUI>();
        hpBarManager = GetComponent<HpBarManager>();
        hpBarManager.SetHp(hp, hp);
    }
    protected virtual void SearchTarget()
    {
        if (target != null)
            BattleState = UnitBattleState.MoveToTarget;

    }

    public override void NormalAttack()
    {
        animator.SetTrigger("NormalAttack");
    }
    public override void PassiveSkill()
    {
        animator.SetTrigger("PassiveSkill");
    }
    public override void ActiveSkill()
    {
        animator.SetTrigger("ActiveSkill");
    }

    public override void NormalAttackEnd() { }
    public override void PassiveSkillEnd() { }
    public override void ActiveSkillEnd() { }

    protected override void IdleUpdate()
    {

    }
    protected override void BattleUpdate()
    {
        //switch (BattleState)
        //{
        //    case UnitBattleState.MoveToTarget:
        //        //Ÿ���� ������ Ÿ�� ��ô
        //        if (target == null)
        //        {
        //            SearchTarget();
        //            return;
        //        }
        //        if (ContainTarget(target, characterData.attack.distance))
        //        {
        //            BattleState = UnitBattleState.NormalAttack;
        //        }
        //        if (Time.time - lastNavTime > navDelay) //SetDestination �� 0.2���� ������ ����
        //        {
        //            lastNavTime = Time.time;
        //            pathFind.SetDestination(target.transform.position);
        //        }
        //        break;
        //    case UnitBattleState.NormalAttack:
        //        //Ÿ���� ������ Ÿ�� ��ô
        //        if (target == null)
        //        {
        //            SearchTarget();
        //            return;
        //        }
        //        if (!ContainTarget(target, characterData.attack.distance))
        //        {
        //            BattleState = UnitBattleState.MoveToTarget;
        //        }

        //        //Ÿ������ �ٶ󺸱�
        //        Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        //        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 120);

        //        //Ÿ�ٰ� ���� ���� �ȿ� ������, �Ϲݽ�ų �����̰�, ��Ÿ�� ������ �����ɶ�

        //        if (CanDisNormalAttack && CanNormalAttackTime)
        //        {
        //            lastNormalAttackTime = Time.time;

        //            animator.SetTrigger("NormalAttack");
        //            Logger.Debug("Idle");
        //            NormalAttackAction();
        //        }
        //        break; ;
        //    case UnitBattleState.PassiveSkill:
        //        break;
        //    case UnitBattleState.ActiveSkill:
        //        break;
        //    case UnitBattleState.Stun:
        //        break;
        //}

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

    public override void ChangeUnitState(UnitState state)
    {
        UnitState = state;
    }
    public override void ChangeBattleState(UnitBattleState state)
    {
        BattleState = state;
    }

    public override void OnDamage(int dmg, bool isCritical)
    {
        hp = Mathf.Max(hp - dmg, 0);
        if (hp <= 0)
            UnitState = UnitState.Die;

        TempShowHpBarAndDamageText(dmg, isCritical);
    }
    public void TempShowHpBarAndDamageText(int dmg, bool isCritical = false)
    {
        floatingDamageText.OnAttack(dmg, isCritical, transform.position, DamageType.Normal);
        hpBarManager.TestCode(dmg);
        if (hp <= 0)
            hpBarManager.Die();
    }
    private void OnDestroy()
    {
        battleManager.OnDeadEnemy(this);
    }
}