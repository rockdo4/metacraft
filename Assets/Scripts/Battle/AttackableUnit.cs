using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AttackableUnit : MonoBehaviour
{
    protected TestBattleManager battleManager;

    [SerializeField]
    protected CharacterDataBundle characterData;
    public CharacterDataBundle GetUnitData() => characterData;

    protected NavMeshAgent pathFind;

    [SerializeField]
    protected AttackableUnit target;
    [SerializeField]
    protected List<AttackableHero> heroList;
    [SerializeField]
    protected List<AttackableEnemy> enemyList;
    [SerializeField]
    protected List<AttackableEnemy> citizenList;

    public int UnitHp {
        get {
            return characterData.data.currentHp;
        }
        set {
            characterData.data.currentHp = Mathf.Max(value, 0);
        }
    }

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
    protected AnimatorStateInfo stateInfo;

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
    public bool IsAlive(AttackableUnit unit) => (unit != null) && (unit.gameObject.activeSelf) && (unit.UnitHp > 0);
    protected bool InRangeNormalAttack => Vector3.Distance(target.transform.position, transform.position) < characterData.attack.distance;
    protected bool NonActiveSkill => battleState != UnitBattleState.ActiveSkill && battleState != UnitBattleState.Stun;

    protected List<Buff> buffList = new();
    protected BufferState bufferState = new();
    protected int GetFixedDamage {
        get {
            return (int)(characterData.data.baseDamage * bufferState.attackIncrease);
        }
    }
    protected int GetFixedActiveDamage {
        get {
            return (int)(characterData.data.baseDamage * bufferState.attackIncrease);
        }
    }

    protected virtual void Awake()
    {
        var manager = FindObjectOfType<TestBattleManager>();
        if (manager != null)
            battleManager = manager;

        animator = GetComponentInChildren<Animator>();
    }
    protected void SetData()
    {
        pathFind.stoppingDistance = characterData.attack.distance;

        NormalAttackAction = NormalAttack;
        PassiveSkillAction = PassiveSkill;
        ActiveSkillAction = ReadyActiveSkill;
        ResetAttackableUnit();
    }

    public void ResetAttackableUnit()
    {
        UnitHp = characterData.data.healthPoint;
    }

    protected void Update()
    {
        nowUpdate?.Invoke();
        for (int i = buffList.Count - 1; i >= 0; i--)
        {
            buffList[i].Update();
        }
    }

    public abstract void ChangeUnitState(UnitState state);
    public abstract void ChangeBattleState(UnitBattleState status);

    public abstract void NormalAttack();
    public abstract void PassiveSkill();
    public abstract void ReadyActiveSkill();
    public virtual void OnActiveSkill() { }

    public virtual void NormalAttackEnd()
    {
        if (IsAlive(target))
            if (target.UnitHp <= 0)
            {
                target = null;
            }
    }
    public virtual void PassiveSkillEnd()
    {
    }
    public virtual void ActiveSkillEnd()
    {
        if (target != null)
            if (target.UnitHp <= 0)
            {
                target = null;
            }
    }

    public abstract void ResetData();

    protected abstract void IdleUpdate();
    protected abstract void BattleUpdate();
    protected abstract void DieUpdate();
    protected abstract void MoveNextUpdate();
    protected abstract void ReturnPosUpdate();

    public abstract void OnDamage(int dmg, bool isCritical = false);

    protected abstract void SearchTarget();
    public void SearchNearbyTarget<T>(List<T> list) where T : AttackableUnit
    {
        var tempList = list;
        if (list.Count == 0)
        {
            target = null;
            return;
        }

        var position = transform.position;
        float minDis = int.MaxValue;

        for (int i = 0; i < list.Count; i++)
        {
            if (!IsAlive(list[i]))
                continue;

            var unitDis = Vector3.Distance(position, list[i].transform.position);
            if(unitDis <= minDis)
            {
                minDis = unitDis;
                target = list[i];
            }
        }
    }

    public T GetSearchNearbyTarget<T>(List<T> list) where T : AttackableUnit
    {
        T minTarget = null;
        if (list.Count == 0)
        {
            return minTarget;
        }

        var position = transform.position;
        float minDis = int.MaxValue;

        for (int i = 0; i < list.Count; i++)
        {
            if (!IsAlive(list[i]))
                continue;

            var unitDis = Vector3.Distance(position, list[i].transform.position);
            if (unitDis <= minDis)
            {
                minDis = unitDis;
                minTarget = list[i];
            }
        }
        return minTarget;
    }

    public T GetSearchTargetInAround<T>(List<T> list, float dis) where T : AttackableUnit
    {
        T minTarget = GetSearchNearbyTarget(list);
        if (minTarget == null)
            return null;
        else if(Vector3.Distance(minTarget.transform.position, transform.position) < dis)
                return minTarget;
        else
            return null;
    }

    protected void SearchMaxHealthTarget<T>(List<T> list) where T : AttackableUnit
    {
        if (list.Count == 0)
        {
            target = null;
            return;
        }

        float maxHp = 0;
        float minDis = int.MaxValue;

        for (int i = 0; i < list.Count; i++)
        {
            if (!IsAlive(list[i]))
                continue;

            if(maxHp == list[i].UnitHp)
            {
                target = Vector3.Distance(transform.position, list[i].transform.position) < minDis ?
                    list[i] : target;
            }
            if (maxHp < list[i].UnitHp)
            {
                minDis = Vector3.Distance(transform.position, list[i].transform.position);
                maxHp = list[i].UnitHp;
                target = list[i];
            }
        }
    }

    protected void SearchMinHealthTarget<T>(List<T> list) where T : AttackableUnit
    {
        if (list.Count == 0)
        {
            target = null;
            return;
        }
        float minHp = int.MaxValue;
        float minDis = int.MaxValue;

        for (int i = 0; i < list.Count; i++)
        {
            if (!IsAlive(list[i]))
                continue;

            if (minHp == list[i].UnitHp)
            {
                target = Vector3.Distance(transform.position, list[i].transform.position) < minDis ?
                    list[i] : target;
            }
            if (list[i].UnitHp < minHp)
            {
                minDis = Vector3.Distance(transform.position, list[i].transform.position);
                minHp = list[i].UnitHp;
                target = list[i];
            }
        }
    }

    public List<T> GetNearestUnitList<T>(List<T> list, int count) where T : AttackableUnit
    {
        List<T> tempList = new();
        if (list.Count == 0)
        {
            return tempList;
        }

        Vector3 position = transform.position;

        for (int i = 0; i < list.Count; i++)
        {
            float dist = Vector3.Distance(position, list[i].transform.position);

            for (int j = i + 1; j < list.Count; j++)
            {
                float distOther = Vector3.Distance(position, list[j].transform.position);

                if (distOther < dist)
                {
                    T temp = list[i];
                    list[i] = list[j];
                    list[j] = temp;

                    dist = distOther;
                }
            }
        }

        var nowCount = count;
        for (int i = 0; i < list.Count; i++)
        {
            if (IsAlive(list[i]))
            {
                nowCount--;
                tempList.Add(list[i]);
            }
            if (nowCount <= 0)
                break;
        }

        return tempList;
    }

    // Test
    public abstract void OnDead(AttackableUnit unit);
    public void DestroyUnit()
    {
        // 이 부분 로테이션 이상할 시 바꿔야함
        Utils.CopyPositionAndRotation(gameObject, gameObject.transform.parent);
        pathFind.enabled = false;
        gameObject.SetActive(false);
    }

    public void SetBattleManager(TestBattleManager manager)
    {
        battleManager = manager;
    }
    public void SetEnabledPathFind(bool set)
    {
        pathFind.enabled = set;
    }

    // 여기에 State 초기화랑 트리거 모두 해제하는 코드 작성

    public abstract void AddBuff(BuffType type, float scale, float duration);
    public abstract void RemoveBuff(Buff buff);

}