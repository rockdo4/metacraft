using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AttackableUnit : MonoBehaviour
{
    protected TestBattleManager battleManager;

    [SerializeField, Header("ĳ���� ������")]
    protected CharacterDataBundle characterData;
    public CharacterDataBundle GetUnitData() => characterData;

    [Header("ĳ���� Ÿ��")]
    public UnitType unitType;
    [Header("Ai Ÿ��")]
    public UnitAiType aiType;

    [Space, Header("�Ϲݰ��� Ÿ��")]
    public UnitType normalAttackTargetType;
    public List<BuffInfo> normalbuffs;

    [Space, Header("�нú� Ÿ��")]
    public UnitType PassiveTargetType;
    public List<BuffInfo> passivekbuffs;

    [Space, Header("�ñر� Ÿ��")]
    public UnitType activeAttackTargetType;
    public List<BuffInfo> attackkbuffs;

    public Transform attackPos;
    public FireBallTest attackPref; //�׽�Ʈ��

    protected float searchDelay = 1f;
    protected float lastSearchTime;

    protected NavMeshAgent pathFind;

    [SerializeField, Header("���� Ÿ��")] protected AttackableUnit target;
    [SerializeField, Header("�ñر� Ÿ��")] protected AttackableUnit activeTarget;
    [SerializeField, Header("����� ����Ʈ")] protected List<AttackableUnit> heroList;
    [SerializeField, Header("���ʹ� ����Ʈ")] protected List<AttackableUnit> enemyList;
    [SerializeField, Header("�ù� ����Ʈ")] protected List<AttackableUnit> citizenList;

    public int UnitHp {
        get { return characterData.data.currentHp; }
        set { characterData.data.currentHp = Mathf.Max(value, 0); }
    }
    //����� = (������ ���ݷ�*��ų���) * (100/100+����) * (1 + ��������)
    public int AttackDamage {
        get {
            return characterData.attack.CreateDamageResult(characterData.data, bufferState);
        }
    }

    public int ActiveDamage {
        get {
            return characterData.activeSkill.CreateDamageResult(characterData.data, bufferState);
        }
    }
    public float UnitHpScale => (float)characterData.data.currentHp / characterData.data.healthPoint;

    protected float lastNormalAttackTime;
    protected float lastActiveSkillTime;

    protected float lastNavTime;
    protected float navDelay = 0.2f;

    protected Action nowUpdate;

    protected Action ActiveSkillAction;
    private Dictionary<UnitAiType, Action> unitSearchAi = new();    //�Ϲݰ��� Ÿ��
    protected Action SearchAi;

    protected bool moveTarget;

    protected Animator animator;
    protected AnimatorStateInfo stateInfo;

    [SerializeField, Header("���� ��������")]
    protected UnitState unitState;
    protected virtual UnitState UnitState { get; set; }

    [SerializeField, Header("���� ��������")]
    protected UnitBattleState battleState;
    protected virtual UnitBattleState BattleState { get; set; }

    public bool IsAlive(AttackableUnit unit) => (unit != null) && (unit.gameObject.activeSelf) && (unit.UnitHp > 0);
    protected bool CanNormalAttackTime => (Time.time - lastNormalAttackTime) > characterData.attack.cooldown;
    protected bool InRangeNormalAttack => Vector3.Distance(target.transform.position, transform.position) < characterData.attack.distance;
    protected bool InRangeActiveAttack => Vector3.Distance(activeTarget.transform.position, transform.position) < characterData.activeSkill.distance;
    protected bool NonActiveSkill => battleState != UnitBattleState.ActiveSkill && battleState != UnitBattleState.Stun;
    //���߿��� NonActiveSkill �����Ͻÿ� ��ų��ư�� ��Ȱ��ȭ �ϱ�

    [SerializeField]
    protected List<Buff> buffList = new();
    [SerializeField]
    protected BufferState bufferState = new();

    protected bool isAuto = true;
    public virtual bool IsAuto {
        get { return isAuto; }
        set { isAuto = value; }
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
            (int) (data.baseDamage * multipleDamage),
            (int) (data.baseDefense * multipleDefense),
            (int) (data.healthPoint * multipleHealthPoint));
    }

    protected void Update()
    {
        nowUpdate?.Invoke();
        for (int i = buffList.Count - 1; i >= 0; i--)
        {
            buffList[i].TimerUpdate();
        }
    }

    public abstract void ChangeUnitState(UnitState state);
    public abstract void ChangeBattleState(UnitBattleState status);

    public abstract void PassiveSkillEvent();
    public abstract void ReadyActiveSkill();
    public virtual void OnActiveSkill() => characterData.activeSkill.OnActiveSkill(ActiveDamage, characterData.data.level);

    public virtual void NormalAttackOnDamage()
    {
        if (BattleState == UnitBattleState.ActiveSkill)
            return;
        if ((characterData.attack.searchType != SkillSearchType.Healer
            && characterData.attack.searchType != SkillSearchType.Buffer))
        {
            if (characterData.attack.targetNumLimit == 1)
            {
                target.OnDamage(AttackDamage, characterData.data.level, false);
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
                attackTargetList[i].OnDamage(AttackDamage, characterData.data.level, false);
            }
        }

        foreach (var buff in normalbuffs)
        {
            var value = (int)(AttackDamage * (buff.buffValue / 100f));
            target.AddBuff(buff, value, null);
        }
    }

    protected void RushSearch()
    {
        SearchNearbyTarget((normalAttackTargetType == UnitType.Hero) ? heroList : enemyList); //�ٰŸ� Ÿ�� ����
    }
    protected void RangeSearch()
    {
        lastSearchTime = Time.time;
        var targetList = (normalAttackTargetType == UnitType.Hero) ? heroList : enemyList;
        var minTarget = GetSearchTargetInAround(targetList, characterData.attack.distance / 2);

        if (IsAlive(minTarget))
            target = minTarget;
        else
            SearchMaxHealthTarget(targetList); //ü���� ���� ���� Ÿ�� ����
    }
    protected void AssassinSearch()
    {
        var targetList = (normalAttackTargetType == UnitType.Hero) ? heroList : enemyList;
        if (targetList.Count == 1)
            SearchNearbyTarget(targetList); //�ٰŸ� Ÿ�� ����
        else
            SearchMinHealthTarget(targetList); //ü���� ���� ���� Ÿ�� ����
    }
    protected void SupportSearch()
    {
        target = GetSearchMinHealthScaleTarget((normalAttackTargetType == UnitType.Hero) ? heroList : enemyList); //�ٰŸ� Ÿ�� ����
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
                activeTarget = GetSearchNearbyTarget(targetList); //���� ����� Ÿ��
                break;
            case SkillSearchType.Targeting:
                activeTarget = target;
                break;
            case SkillSearchType.Buffer:
                activeTarget = GetSearchNearbyTarget(teamList); //���� ����� �츮���� Ÿ��
                break;
            case SkillSearchType.Healer:
                activeTarget = GetSearchMinHealthScaleTarget(teamList); //���� ü�º��� ���� �츮���� Ÿ��
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
    public virtual void StunEnd()
    {
        animator.SetTrigger("StunEnd");
        target = null;
        pathFind.isStopped = false;
        lastNormalAttackTime = Time.time;
    }

    public virtual void ResetData()
    {
        RemoveBuffers();
    }
    public void ResetBuffers()
    {
        for (int i = buffList.Count - 1; i >= 0; i--)
        {
            var buff = buffList[i];
            buff.removeBuff(buff);
        }
        buffList.Clear();
    }
    public void RemoveBuffers()
    {
        foreach(var buff in buffList)
        {
            buff.timer = 0;
        }
    }

    protected abstract void IdleUpdate();
    protected abstract void BattleUpdate();
    protected abstract void DieUpdate();
    protected abstract void MoveNextUpdate();
    protected abstract void ReturnPosUpdate();
    //����� = (������ ���ݷ�*��ų���) * (100/100+����) * (1 + ��������)
    public virtual void OnDamage(int dmg, int level, bool isCritical = false)
    {
        var defense = 100f / (100 + characterData.data.baseDefense + bufferState.defense);
        var levelCorrection = 1 + Mathf.Clamp((level - characterData.data.level) / 100f, -0.4f, 0);

        dmg = (int)(dmg * defense * levelCorrection);
        UnitHp = Mathf.Max(UnitHp - dmg, 0);
        if (UnitHp <= 0)
            UnitState = UnitState.Die;
    }

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
        // �� �κ� �����̼� �̻��� �� �ٲ����
        Utils.CopyPositionAndRotation(gameObject, gameObject.transform.parent);
        pathFind.enabled = false;
        gameObject.SetActive(false);
    }

    public void SetBattleManager(TestBattleManager manager) => battleManager = manager;
    public void SetEnabledPathFind(bool set) => pathFind.enabled = set;

    // ���⿡ State �ʱ�ȭ�� Ʈ���� ��� �����ϴ� �ڵ� �ۼ�

    public virtual void AddBuff(BuffInfo info, int anotherValue , BuffIcon icon = null)
    {
        var findBuff = buffList.Find(t => t.buffInfo.id == info.id);
        if (findBuff != null)
        {
            findBuff.timer = info.duration;
        }
        else
        {
            if (info.fraction == 0)
            {
                switch(info.type)
                {
                    case BuffType.Heal:
                        UnitHp += anotherValue;
                        break;
                }
            }
            else
            {
                Buff buff = new Buff(info, this, RemoveBuff, icon);
                buffList.Add(buff);
                bufferState.Buffer(info.type, info.buffValue);
            }

        }
    }   
    public void BuffDurationUpdate(int id, float dur) => buffList.Find(t => t.buffInfo.id == id).timer= dur;
    public virtual void RemoveBuff(Buff buff)
    {
        buffList.Remove(buff);
        bufferState.RemoveBuffer(buff.buffInfo.type, buff.buffInfo.buffValue);
    }

    public void SetMaxHp()
    {
        UnitHp = GetUnitData().data.healthPoint;
    }
}