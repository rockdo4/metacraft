using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AttackableUnit : MonoBehaviour
{
    protected TestBattleManager battleManager;  

    [SerializeField, Header("캐릭터 데이터")]
    protected CharacterDataBundle characterData;
    public CharacterDataBundle GetUnitData() => characterData;

    [Header("캐릭터 타입")]
    public UnitType unitType;
    [Header("Ai 타입")]
    public UnitAiType aiType;

    [Space, Header("일반공격 - 타겟, 시간, 증가량")]
    public UnitType normalAttackTargetType;
    public Transform attackPos;
    public FireBallTest attackPref; //테스트용

    [Space, Header("궁극기 - 타겟, 버프or디버프, 시간, 증가량")]
    public UnitType activeAttackTargetType;

    protected float searchDelay = 1f;
    protected float lastSearchTime;

    protected NavMeshAgent pathFind;

    [SerializeField, Header("현재 타겟")]     protected AttackableUnit target;
    [SerializeField, Header("궁극기 타겟")]   protected AttackableUnit activeTarget;
    [SerializeField, Header("히어로 리스트")] protected List<AttackableUnit> heroList;
    [SerializeField, Header("에너미 리스트")] protected List<AttackableUnit> enemyList;
    [SerializeField, Header("시민 리스트")]   protected List<AttackableUnit> citizenList;

    public int UnitHp {
        get { return characterData.data.currentHp;  }
        set { characterData.data.currentHp = Mathf.Max(value, 0); }
    }
    public float UnitHpScale => (float)characterData.data.currentHp / (float)characterData.data.healthPoint;

    protected float lastNormalAttackTime;
    protected float lastPassiveSkillTime;
    protected float lastActiveSkillTime;

    protected float lastNavTime;
    protected float navDelay = 0.2f;

    protected Action nowUpdate;

    protected Action PassiveSkillAction;
    protected Action ActiveSkillAction;
    private Dictionary<UnitAiType, Action> unitSearchAi = new();    //일반공격 타겟
    protected Action SearchAi;

    protected bool moveTarget;

    protected Animator animator;
    protected AnimatorStateInfo stateInfo;

    [SerializeField, Header("메인 상태패턴")]
    protected UnitState unitState;
    protected virtual UnitState UnitState { get; set; }

    [SerializeField, Header("전투 상태패턴")]
    protected UnitBattleState battleState;
    protected virtual UnitBattleState BattleState { get; set; }

    public bool IsAlive(AttackableUnit unit) => (unit != null) && (unit.gameObject.activeSelf) && (unit.UnitHp > 0);
    protected bool CanNormalAttackTime => (Time.time - lastNormalAttackTime) > characterData.attack.cooldown;
    protected bool InRangeNormalAttack => Vector3.Distance(target.transform.position, transform.position) < characterData.attack.distance;
    protected bool InRangeActiveAttack => Vector3.Distance(activeTarget.transform.position, transform.position) < characterData.activeSkill.distance;
    protected bool NonActiveSkill => battleState != UnitBattleState.ActiveSkill && battleState != UnitBattleState.Stun;
    //나중에는 NonActiveSkill 상태일시에 스킬버튼을 비활성화 하기

    protected List<Buff> buffList = new();
    protected BufferState bufferState = new();
    protected int GetFixedDamage => (int)(characterData.data.baseDamage * bufferState.attackIncrease);
    protected int GetFixedActiveDamage => (int)(characterData.data.baseDamage * bufferState.attackIncrease);

    protected bool isAuto = true;
    public virtual bool IsAuto {
        get { return isAuto; }
        set {  isAuto = value; }
    }

    protected virtual void Awake()
    {
        var manager = FindObjectOfType<TestBattleManager>();
        if (manager != null)
            battleManager = manager;

        animator = GetComponentInChildren<Animator>();
        unitSearchAi[UnitAiType.Rush] = RushSearch;
        unitSearchAi[UnitAiType.Range] = RangeSearch;
        unitSearchAi[UnitAiType.Assassin] = AssassinSearch;
        unitSearchAi[UnitAiType.Supprot] = SupportSearch;
    }

    protected void SetData()
    {
        pathFind.stoppingDistance = characterData.attack.distance;
        PassiveSkillAction = PassiveSkillEvent;
        ActiveSkillAction = ReadyActiveSkill;
        SearchAi = unitSearchAi[aiType];
    }

    public void SetLevelExp(int newLevel, int newExp)
    {
        LiveData data = GetUnitData().data;
        data.level = newLevel;
        data.exp = newExp;
    }

    public void LevelUpAdditional(int incDamage, int incDefense, int incHealthPoint)
    {
        LiveData data = GetUnitData().data;
        data.baseDamage += incDamage;
        data.baseDefense += incDefense;
        data.healthPoint += incHealthPoint;
        data.currentHp = data.healthPoint;
    }
    
    public void LevelUpMultiplication(float multipleDamage, float multipleDefense, float multipleHealthPoint)
    {
        LiveData data = GetUnitData().data;
        LevelUpAdditional(
            (int) (data.baseDamage * (1 + multipleDamage)),
            (int) (data.baseDefense * (1 + multipleDefense)),
            (int) (data.healthPoint * (1 + multipleHealthPoint)));
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

    public abstract void PassiveSkillEvent();
    public abstract void ReadyActiveSkill();
    public virtual void OnActiveSkill() => characterData.activeSkill.OnActiveSkill(characterData.data);

    public virtual void NormalAttackOnDamage()
    {
        if (BattleState == UnitBattleState.ActiveSkill)
            return;

        var isHeal = characterData.attack.searchType == SkillSearchType.Healer ? -1 : 1;
        if (characterData.attack.targetNumLimit == 1)
        {
            target.OnDamage(GetFixedDamage * isHeal, false);
            return;
        }

        List<AttackableUnit> attackTargetList = new();

        var targetList = (normalAttackTargetType == UnitType.Hero) ? heroList : enemyList;
        foreach (var now_target in targetList)
        {
            Vector3 interV = now_target.transform.position - transform.position;
            if (interV.magnitude <= characterData.attack.distance)
            {
                float angle = Vector3.Angle(transform.forward, interV);

                if (Mathf.Abs(angle) < characterData.attack.angle / 2f)
                {
                    attackTargetList.Add(now_target);
                }
            }
        }

        attackTargetList = GetNearestUnitList(attackTargetList, characterData.attack.targetNumLimit);

        for (int i = 0; i < attackTargetList.Count; i++)
        {
            attackTargetList[i].OnDamage(GetFixedDamage * isHeal, false);
        }
    }

    protected void RushSearch()
    {
        SearchNearbyTarget((normalAttackTargetType == UnitType.Hero) ? heroList : enemyList); //근거리 타겟 추적
    }
    protected void RangeSearch()
    {
        lastSearchTime = Time.time;
        var targetList = (normalAttackTargetType == UnitType.Hero) ? heroList : enemyList;
        var minTarget = GetSearchTargetInAround(targetList, characterData.attack.distance / 2);

        if (IsAlive(minTarget))
            target = minTarget;
        else
            SearchMaxHealthTarget(targetList); //체력이 가장 많은 타겟 추적
    }
    protected void AssassinSearch()
    {
        var targetList = (normalAttackTargetType == UnitType.Hero) ? heroList : enemyList;
        if (targetList.Count == 1)
            SearchNearbyTarget(targetList); //근거리 타겟 추적
        else
            SearchMinHealthTarget(targetList); //체력이 가장 적은 타겟 추적
    }

    protected void SupportSearch()
    {
        target = GetSearchMinHealthScaleTarget((normalAttackTargetType == UnitType.Hero) ? heroList : enemyList); //근거리 타겟 추적
    }

    protected void SearchActiveTarget()
    {
        var targetList = (activeAttackTargetType == UnitType.Hero) ? heroList : enemyList;
        var teamList = (unitType == UnitType.Hero) ? heroList : enemyList;
        switch (characterData.activeSkill.searchType)
        {
            case SkillSearchType.None:
                break;
            case SkillSearchType.AOE:
                activeTarget = GetSearchNearbyTarget(targetList); //가장 가까운 타겟
                break;
            case SkillSearchType.Targeting:
                activeTarget = target;
                break;
            case SkillSearchType.Buffer:
                activeTarget = GetSearchNearbyTarget(teamList); //가장 가까운 우리팀이 타겟
                break;
            case SkillSearchType.Healer:
                activeTarget = GetSearchMinHealthScaleTarget(teamList); //가장 체력비율 적은 우리팀이 타겟
                break;
            case SkillSearchType.Another:
                break;
            case SkillSearchType.Count:
                break;
            default:
                break;
        }
    }

    public virtual void NormalAttackEnd() => target = (IsAlive(target)) ? null : target;
    public virtual void PassiveSkillEnd() { }
    public virtual void ActiveSkillEnd() => target = (IsAlive(target)) ? null : target;

    public virtual void ResetData()
    {
        foreach (var buff in buffList)
        {
            buff.removeBuff(buff);
        }
        buffList.Clear();
    }

    protected abstract void IdleUpdate();
    protected abstract void BattleUpdate();
    protected abstract void DieUpdate();
    protected abstract void MoveNextUpdate();
    protected abstract void ReturnPosUpdate();

    public abstract void OnDamage(int dmg, bool isCritical = false);

    public void SearchNearbyTarget(List<AttackableUnit> list) 
    {
        AttackableUnit tempTarget = null;
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
                tempTarget = list[i];
            }
        }
        target = tempTarget;
    }

    public AttackableUnit GetSearchNearbyTarget(List<AttackableUnit> list) 
    {
        AttackableUnit minTarget = null;
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

    public AttackableUnit GetSearchTargetInAround(List<AttackableUnit> list, float dis) 
    {
        AttackableUnit minTarget = GetSearchNearbyTarget(list);
        if (minTarget == null)
            return null;
        else if(Vector3.Distance(minTarget.transform.position, transform.position) < dis)
                return minTarget;
        else
            return null;
    }

    protected void SearchMaxHealthTarget(List<AttackableUnit> list) 
    {
        AttackableUnit tempTarget = null;
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
                tempTarget = Vector3.Distance(transform.position, list[i].transform.position) < minDis ?
                    list[i] : tempTarget;
            }
            if (maxHp < list[i].UnitHp)
            {
                minDis = Vector3.Distance(transform.position, list[i].transform.position);
                maxHp = list[i].UnitHp;
                tempTarget = list[i];
            }
        }
        target = tempTarget;
    }

    protected void SearchMinHealthTarget(List<AttackableUnit> list) 
    {
        AttackableUnit tempTarget = null;
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
                tempTarget = Vector3.Distance(transform.position, list[i].transform.position) < minDis ?
                    list[i] : tempTarget;
            }
            if (list[i].UnitHp < minHp)
            {
                minDis = Vector3.Distance(transform.position, list[i].transform.position);
                minHp = list[i].UnitHp;
                tempTarget = list[i];
            }
        }

        target = tempTarget;
    }

    protected AttackableUnit GetSearchMinHealthScaleTarget(List<AttackableUnit> list) 
    {
        AttackableUnit nowTarget = null;
        if (list.Count == 0)
        {
            nowTarget = null;
            return nowTarget;
        }
        float minHp = int.MaxValue;
        float minDis = int.MaxValue;

        for (int i = 0; i < list.Count; i++)
        {
            if (!IsAlive(list[i]))
                continue;

            if (minHp == list[i].UnitHpScale)
            {
                nowTarget = Vector3.Distance(transform.position, list[i].transform.position) < minDis ?
                    list[i] : nowTarget;
            }
            if (list[i].UnitHpScale < minHp)
            {
                minDis = Vector3.Distance(transform.position, list[i].transform.position);
                minHp = list[i].UnitHpScale;
                nowTarget = list[i];
            }
        }

        return nowTarget;
    }

    public List<AttackableUnit> GetNearestUnitList(List<AttackableUnit> list, int count)
    {
        List<AttackableUnit> tempList = new();
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
                    AttackableUnit temp = list[i];
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

    public void SetMaxHp()
    {
        UnitHp = characterData.data.healthPoint;
    }
}