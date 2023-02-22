using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public abstract class AttackableUnit : MonoBehaviour
{
    //protected CharacterData charactorData;
    protected HeroData heroData;

    protected NavMeshAgent pathFind;

    protected AttackableUnit target;
    protected LayerMask targetType;

    [SerializeField] // Why ?
    protected int hp;

    protected float lastNormalAttackTime;
    protected float lastAutoTime;
    protected float lastActiveTime;

    protected float activeStartTime; //�ñر� ���� �ð����� ��Ÿ, �Ϲݽ�ųX

    protected Action nowUpdate;

    protected Action Common;
    protected Action Auto;
    protected Action Active;


    protected UnitState unitState;
    protected UnitState UnitState { get { return unitState; } set { unitState = value; } }

    protected UnitBattleState battleState;
    protected UnitBattleState BattleState { get { return battleState; }  set { battleState = value; } }

    protected bool CanNormalAttack {
        get {
            return (Time.time - lastNormalAttackTime) > heroData.normalAttack.cooldown;
        }
    }

    protected bool InRange => Vector3.Distance(target.transform.position, transform.position) < heroData.normalAttack.distance;
    protected bool IsNormal => battleState != UnitBattleState.Action && battleState != UnitBattleState.Stun;

    protected bool IsAttack {
        get {
            return IsNormal && InRange;
        }
    }

    [SerializeField]
    protected List<AttackableUnit> targetList;
    protected void SetTargetList(List<AttackableUnit> list) => targetList = list;

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
    protected void SetData()
    {
        lastNormalAttackTime = Time.time;
        pathFind.stoppingDistance = heroData.normalAttack.distance;
        Common = CommonAttack;
        Auto = AutoAttack;
        Active = ActiveAttack;

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

    protected abstract void SetTestBattle();
    public abstract void CommonAttack();
    public abstract void AutoAttack();
    public abstract void ActiveAttack();

    protected abstract void IdleUpdate();
    protected abstract void BattleUpdate();
    protected abstract void DieUpdate();
    protected abstract void MoveNextUpdate();
    protected abstract void ReturnPosUpdate();

    public abstract void OnDamage(int dmg);

    //public CharacterData LoadTestData()
    //{
    //    CharacterData testData = new();

    //    testData.damage = 3;
    //    testData.def = 20;
    //    testData.hp = 100;
    //    testData.speed = 3;
    //    testData.critical = 50;
    //    testData.criticalDmg = 2;
    //    testData.evasion = 20;
    //    testData.accuracy = 80;

    //    testData.attackDistance = 3f;
    //    testData.attackCount = 2;

    //    testData.grade = "A";
    //    testData.level = 1;
    //    testData.job = "TEST";
    //    testData.cooldown = 1;
    //    testData.skillCooldown = 0.3f;
    //    testData.skillDuration = 3f;
    //    testData.exp = 0;

    //    return testData;
    //}
}
