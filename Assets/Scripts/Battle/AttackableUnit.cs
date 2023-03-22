using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
{
    public AnimationClipOverrides(int capacity) : base(capacity) { }

    public AnimationClip this[string name]
    {
        get { return Find(x => x.Key.name.Equals(name)).Value; }
        set
        {
            int index = FindIndex(x => x.Key.name.Equals(name));
            if (index != -1)
                this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
        }
    }
}

public abstract class AttackableUnit : MonoBehaviour
{
    protected BattleManager battleManager;

    [SerializeField, Header("ĳ���� ������")]
    protected CharacterDataBundle characterData;
    public CharacterDataBundle GetUnitData() => characterData;
    public void SetUnitOriginData(CharacterData data) => characterData.originData = data;
    [SerializeField, Header("���� ��ų")]
    protected CharacterSkill nowAttack;
    protected CharacterSkill GetNowAttack() => nowAttack;
    protected float minAttackDis = float.MaxValue;
    public AnimationClip[] skillClips;


    [Header("ĳ���� Ÿ��")]
    public UnitType unitType;
    //[Header("Ai Ÿ��")]
    //public CharacterJob aiType;

    [Space, Header("�Ϲݰ��� Ÿ��")]
    public UnitType normalAttackTargetType;

    [Space, Header("�нú� Ÿ��")]
    public UnitType PassiveTargetType;

    [Space, Header("�ñر� Ÿ��")]
    public UnitType activeAttackTargetType;

    public Transform attackPos;
    public FireBallTest attackPref; //�׽�Ʈ��

    protected float searchDelay = 1f;
    protected float lastSearchTime;

    public NavMeshAgent pathFind;

    [SerializeField, Header("���� Ÿ��")] protected AttackableUnit target;
    [SerializeField, Header("�ñر� Ÿ��")] protected AttackableUnit activeTarget;
    [SerializeField, Header("����� ����Ʈ")] protected List<AttackableUnit> heroList;
    [SerializeField, Header("���ʹ� ����Ʈ")] protected List<AttackableUnit> enemyList;
    [SerializeField, Header("�ù� ����Ʈ")] protected List<AttackableUnit> citizenList;

    public float MaxHp => ((bufferState.maxHealthIncrease * characterData.data.healthPoint));
    public float UnitHpScale => characterData.data.currentHp / MaxHp;
    public virtual float UnitHp
    {
        get { return characterData.data.currentHp; }
        set
        {
            characterData.data.currentHp = Mathf.Clamp(value, 0, MaxHp);
        }
    }

    protected Dictionary<CharacterSkill, float> lastNormalAttackTime = new();
    protected float lastActiveSkillTime;

    protected float lastNavTime;
    protected float navDelay = 0.2f;

    protected Action nowUpdate;

    protected Action ActiveSkillAction;
    //private Dictionary<CharacterJob, Action> unitSearchAi = new();    //�Ϲݰ��� Ÿ��
    protected Action SearchAi;

    protected bool moveTarget;

    protected Animator animator;
    protected AnimatorStateInfo stateInfo;
    protected AnimatorOverrideController animatorOverrideController;
    protected AnimationClipOverrides clipOverrides;

    [SerializeField, Header("���� ��������")]
    protected UnitState unitState;
    protected virtual UnitState UnitState { get; set; }

    [SerializeField, Header("���� ��������")]
    protected UnitBattleState battleState;
    protected virtual UnitBattleState BattleState { get; set; }

    public bool IsAlive(AttackableUnit unit) => (unit != null) && (unit.gameObject.activeSelf) && (unit.UnitHp > 0);
    protected bool CanNormalAttackTime(CharacterSkill skill) => (Time.time - lastNormalAttackTime[skill]) * bufferState.attackSpeed > skill.cooldown;
    protected bool InRangeNormalAttack(CharacterSkill skill) => Vector3.Distance(target.transform.position, transform.position) < skill.distance;
    protected bool InRangeMinNormalAttack => Vector3.Distance(target.transform.position, transform.position) < minAttackDis;
    protected bool InRangeActiveAttack => Vector3.Distance(activeTarget.transform.position, transform.position) < characterData.activeSkill.distance;
    protected bool NonActiveSkill => battleState != UnitBattleState.ActiveSkill && battleState != UnitBattleState.Stun;
    //���߿��� NonActiveSkill �����Ͻÿ� ��ų��ư�� ��Ȱ��ȭ �ϱ�

    [SerializeField]
    protected List<Buff> buffList = new();
    [SerializeField]
    protected BufferState bufferState = new();
    public BufferState GetBuffState => bufferState;

    protected bool isAuto = false;
    public virtual bool IsAuto
    {
        get { return isAuto; }
        set { isAuto = value; }
    }

    [SerializeField]
    protected bool isThereDamageUI = false;
    [SerializeField]
    protected bool usingFloatingHpBar = false;

    protected AttackedDamageUI floatingDamageText;
    protected HpBarManager hpBarManager;

    public bool isAlive = false;

    public Transform effectCreateTransform;
    public Transform hitEffectTransform;

    private void Start()
    {
        var manager = FindObjectOfType<BattleManager>();
        if (manager != null)
            battleManager = manager;
    }

    protected void InitData()
    {
        animator = GetComponentInChildren<Animator>();
        //unitSearchAi[CharacterJob.assult] = AssultSearch;
        //unitSearchAi[CharacterJob.defence] = AssultSearch;
        //unitSearchAi[CharacterJob.shooter] = ShooterSearch;
        //unitSearchAi[CharacterJob.assassin] = AssassinSearch;
        //unitSearchAi[CharacterJob.assist] = AssistSearch;
        //unitSearchAi[CharacterJob.villain] = AssultSearch;

        if (isThereDamageUI)
        {
            floatingDamageText = GetComponent<AttackedDamageUI>();

            if (usingFloatingHpBar)
            {
                hpBarManager = GetComponent<HpBarManager>();
                hpBarManager.SetHp(UnitHp, characterData.data.healthPoint);
            }
        }

        nowAttack = characterData.attacks[0];
        foreach (CharacterSkill skill in characterData.attacks)
        {
            lastNormalAttackTime[skill] = Time.time;
            if (skill.distance < minAttackDis)
            {
                minAttackDis = skill.distance;
            }
        }

        animatorOverrideController = Instantiate(animator.runtimeAnimatorController as AnimatorOverrideController);
        animator.runtimeAnimatorController = animatorOverrideController;

        //animatorOverrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
        //animator.runtimeAnimatorController = animatorOverrideController;

        clipOverrides = new AnimationClipOverrides(animatorOverrideController.overridesCount);
        animatorOverrideController.GetOverrides(clipOverrides);
    }

    protected void SetData()
    {
        pathFind.stoppingDistance = minAttackDis;
        ActiveSkillAction = ReadyActiveSkill;

        SearchAi = (CharacterJob)GetUnitData().data.job switch
        {
            CharacterJob.shooter => ShooterSearch,
            CharacterJob.assassin => AssassinSearch,
            CharacterJob.assist => AssistSearch,
            _ => AssultSearch,
        };
        //SearchAi = unitSearchAi[(CharacterJob)GetUnitData().data.job];
    }

    public void SetLevelExp(int newLevel, int newExp)
    {
        LiveData data = GetUnitData().data;
        data.level = newLevel;
        data.exp = newExp;
    }

    public void LevelupStats(int level = 1)
    {
        LiveData data = GetUnitData().data;
        data.baseDamage += characterData.originData.damageLevelCoefficient * level;
        data.baseDefense += characterData.originData.defenseLevelCoefficient * level;
        data.healthPoint += characterData.originData.healthPointLevelCoefficient * level;
        data.currentHp = data.healthPoint;
    }

    //public void LevelUpMultiplication(float multipleDamage, float multipleDefense, float multipleHealthPoint)
    //{
    //    LiveData data = GetUnitData().data;
    //    LevelUpAdditional(
    //        (int)(data.baseDamage * multipleDamage),
    //        (int)(data.baseDefense * multipleDefense),
    //        (int)(data.healthPoint * multipleHealthPoint));
    //}

    protected void Update()
    {
        if (Time.timeScale == 0)
            return;
        nowUpdate?.Invoke();
        for (int i = buffList.Count - 1; i >= 0; i--)
        {
            buffList[i].TimerUpdate();
        }
    }

    public abstract void ChangeUnitState(UnitState state);

    public abstract void ChangeBattleState(UnitBattleState status);

    public abstract void ReadyActiveSkill();

    public virtual void OnActiveSkill()
    {
        characterData.activeSkill.OnActiveSkill(this, enemyList, heroList);

        //var units = characterData.activeSkill.SkillEffectedUnits;
        //for (int i = 0; i < units.Count; i++)
        //{
        //    foreach (var buff in attackkbuffs)
        //    {
        //        bool isCritical = false;
        //        var value = CalculDamage(characterData.activeSkill, ref isCritical);
        //        units[i].AddValueBuff(buff, value, null);          
        //    }
        //}
    }

    public virtual void NormalAttackOnDamage()
    {
        if (target == null)
            return;

        if (BattleState == UnitBattleState.ActiveSkill)
            return;

        nowAttack.NormalAttackOnDamage();

        if (nowAttack.targetNumLimit == 1)
        {
            target.OnDamage(this, nowAttack);
        }
        else
        {
            List<AttackableUnit> attackTargetList = new();

            var targetList = (normalAttackTargetType == UnitType.Hero) ? heroList : enemyList;
            foreach (var now_target in targetList)
            {
                Vector3 interV = now_target.transform.position - transform.position;
                if (interV.magnitude <= nowAttack.distance)
                {
                    float angle = Vector3.Angle(transform.forward, interV);

                    if (Mathf.Abs(angle) < nowAttack.angle / 2f)
                    {
                        attackTargetList.Add(now_target);
                    }
                }
            }

            attackTargetList = GetNearestUnitList(attackTargetList, nowAttack.targetNumLimit);

            for (int i = 0; i < attackTargetList.Count; i++)
            {
                attackTargetList[i].OnDamage(this, nowAttack);
            }
        }

        //if (nowAttack.searchType == SkillSearchType.Healer)
        //{
        //    BuffInfo heal = null;
        //    foreach (var buff in nowAttack.buffInfos)
        //    {
        //        if (buff[nowAttack.skillLevel - 1].type == BuffType.Heal)
        //        {
        //            heal = buff[nowAttack.skillLevel - 1];
        //            break;
        //        }
        //    }
        //    var levelCorrection = 1 + Mathf.Clamp((characterData.data.level - characterData.data.level) / 100f, -0.4f, 0);
        //    bool isCritical = false;
        //    var dmg = (int)(CalculDamage(nowAttack, ref isCritical) * levelCorrection);
        //    target.AddValueBuff(heal, dmg);

        //}
    }
    protected void AssultSearch()
    {
        if (bufferState.provoke)
            return;
        SearchNearbyTarget((normalAttackTargetType == UnitType.Hero) ? heroList : enemyList); //�ٰŸ� Ÿ�� ����
    }

    protected void ShooterSearch()
    {
        if (bufferState.provoke)
            return;

        lastSearchTime = Time.time;
        var targetList = (normalAttackTargetType == UnitType.Hero) ? heroList : enemyList;
        var minTarget = GetSearchTargetInAround(targetList, 3);

        if (IsAlive(minTarget))
            target = minTarget;
        else
            SearchMaxHealthTarget(targetList); //ü���� ���� ���� Ÿ�� ����
    }

    protected void AssassinSearch()
    {
        if (bufferState.provoke)
            return;
        var targetList = (normalAttackTargetType == UnitType.Hero) ? heroList : enemyList;
        if (targetList.Count == 1)
            SearchNearbyTarget(targetList); //�ٰŸ� Ÿ�� ����
        else
            SearchMinHealthTarget(targetList); //ü���� ���� ���� Ÿ�� ����
    }

    protected void AssistSearch()
    {
        if (bufferState.provoke)
            return;
        target = GetSearchMinHealthScaleTarget((normalAttackTargetType == UnitType.Hero) ? heroList : enemyList); //�ٰŸ� Ÿ�� ����
    }

    protected void SearchActiveTarget()
    {
        if (bufferState.provoke)
            return;
        var targetList = (activeAttackTargetType == UnitType.Hero) ? heroList : enemyList;
        var teamList = (unitType == UnitType.Hero) ? heroList : enemyList;
        switch (characterData.activeSkill.searchType)
        {
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
            default:
                break;
        }
    }

    public virtual void NormalAttackEnd() => target = (!IsAlive(target)) ? null : target;

    public virtual void PassiveSkillEnd() { }

    public virtual void ActiveSkillEnd() => target = (!IsAlive(target)) ? null : target;

    public virtual void StunEnd()
    {
        animator.SetTrigger("StunEnd");
        target = null;
        pathFind.isStopped = false;
    }

    public virtual void ProvokeEnd()
    {
        target = null;
        //Logger.Debug("Provoke End");
    }
    public void OnPassiveSkill(List<AttackableUnit> enemies, List<AttackableUnit> heros)
    {
        Logger.Debug($"{transform.name} ���� ����");
        characterData.passiveSkill?.OnActiveSkill(this, enemies, heros);
    }
    public virtual void ResetData()
    {
        ResetBuffTimer();
        nowAttack = characterData.attacks[0];
        animator.SetFloat("Speed", 0);

        unitState = UnitState.None;
        battleState = UnitBattleState.None;
        nowUpdate = null;
    }
    public void RemoveAllBuff()
    {
        for (int i = buffList.Count - 1; i >= 0; i--)
        {
            var buff = buffList[i];
            buff.removeBuff(buff);
        }
        buffList.Clear();
    }
    public void ResetBuffTimer()
    {
        foreach (var buff in buffList)
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
    public virtual void OnDamage(AttackableUnit attackableUnit, CharacterSkill skill)
    {
        if (UnitState != UnitState.Battle)
            return;

        if (skill.searchType == SkillSearchType.Buffer)
        {
            return;
        }
        var defense = 100f / (100 + characterData.data.baseDefense * bufferState.defense);
        var levelCorrection = 1 + Mathf.Clamp((attackableUnit.characterData.data.level - characterData.data.level) / 100f, -0.4f, 0);

        bool isCritical = false;
        var dmg = (int)(attackableUnit.CalculDamage(skill, ref isCritical) * defense * levelCorrection);

        if (bufferState.isShield)
        {
            var shield = (int)(dmg * bufferState.shield);

            dmg -= shield;
        }

        //�������� �Լ�
        var isFridendly = skill.targetType.Equals(SkillTargetType.Friendly);

        if (isFridendly)
            dmg = -(int)attackableUnit.CalculDamage(skill, ref isCritical);

        //if ((unitType == UnitType.Hero && skill.targetType.Equals(SkillTargetType.Friendly))
        //    || (unitType == UnitType.Enemy && skill.targetType.Equals(SkillTargetType.Enemy)))
        //    dmg = -(int)attackableUnit.CalculDamage(skill, ref isCritical);

        UnitHp = Mathf.Max(UnitHp - dmg, 0);
        if (UnitHp <= 0)
        {
            UnitState = UnitState.Die;
        }

        if (!skill.hitEffect.Equals(EffectEnum.None))
        {
            if (isFridendly)
            {
                EffectManager.Instance.Get(skill.hitEffect, transform);
            }
            else
                EffectManager.Instance.Get(skill.hitEffect, hitEffectTransform != null ? hitEffectTransform : transform);
        }
        ShowHpBarAndDamageText(dmg, isCritical);
    }

    public void ShowHpBarAndDamageText(int dmg, bool isCritical = false)
    {
        if (!isThereDamageUI)
            return;

        var type = isCritical ? DamageType.Critical : DamageType.Normal;

        if (dmg < 0)
        {
            dmg *= -1;
            type = DamageType.Heal;
        }

        floatingDamageText.OnAttack(dmg, isCritical, transform.position, type);

        if (!usingFloatingHpBar)
            return;

        hpBarManager.OnDamage(dmg);
        if (UnitHp <= 0)
        {
            hpBarManager.Die();
        }
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
            if (unitDis <= minDis)
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
        else if (Vector3.Distance(minTarget.transform.position, transform.position) < dis)
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

            if (maxHp == list[i].UnitHp)
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
            if (nowCount <= 0)
                break;
            if (IsAlive(list[i]))
            {
                nowCount--;
                tempList.Add(list[i]);
            }
        }

        return tempList;
    }

    // Test
    public abstract void OnDead(AttackableUnit unit);
    public virtual void DestroyUnit()
    {
        //Utils.CopyPositionAndRotation(gameObject, gameObject.transform.parent);
        //pathFind.enabled = false;
        //Logger.Debug("DestroyUnit");
        gameObject.SetActive(false);
        isAlive = false;
    }

    public void SetBattleManager(BattleManager manager) => battleManager = manager;
    public void SetEnabledPathFind(bool set) => pathFind.enabled = set;
    public void SetPathFind()
    {
        pathFind = transform.GetComponent<NavMeshAgent>();
        pathFind.enabled = true;
    }

    // ���⿡ State �ʱ�ȭ�� Ʈ���� ��� �����ϴ� �ڵ� �ۼ�

    public virtual void AddValueBuff(BuffInfo info, int anotherValue = 0, BuffIcon icon = null)
    {
        if (UnitState == UnitState.Die)
        {
            return;
        }
        else
        {
            if (info.fraction == 0)
            {
                switch (info.type)
                {
                    case BuffType.Heal:
                        {
                            UnitHp += anotherValue;
                            if (isThereDamageUI)
                            {
                                floatingDamageText.OnAttack(anotherValue, false, transform.position, DamageType.Heal);
                            }
                        }
                        break;
                }
            }
            else
            {
                Action endEvent = null;

                switch (info.type)
                {
                    case BuffType.Resistance:
                        break;
                    case BuffType.Blind:
                        break;
                    case BuffType.Burns:
                        break;
                    case BuffType.Freeze:
                        break;
                    case BuffType.Shield:
                        break;
                    case BuffType.Bleed:
                        break;
                    case BuffType.LifeSteal:
                        break;
                    case BuffType.energyCharging:
                        //Logger.Debug("energyCharging");
                        break;
                    case BuffType.Count:
                        break;
                    default:
                        break;
                }
                Buff buff = new(info, this, RemoveBuff, icon, endEvent);
                buffList.Add(buff);
                bufferState.Buffer(info.type, info.buffValue);

                if (buff.buffInfo.type == BuffType.MaxHealthIncrease)
                {
                    SetMaxHp();
                }
            }
        }
    }

    public virtual void AddStateBuff(BuffInfo info, AttackableUnit attackableUnit = null, BuffIcon icon = null)
    {
        if (UnitState == UnitState.Die)
        {
            return;
        }

        else
        {
            Action endEvent = null;

            switch (info.type)
            {
                case BuffType.Provoke:
                    //Logger.Debug("Provoke");
                    target = attackableUnit;
                    endEvent = ProvokeEnd;
                    break;
                case BuffType.Stun:
                    //Logger.Debug("Stun");
                    endEvent = StunEnd;
                    BattleState = UnitBattleState.Stun;
                    break;
                case BuffType.Silence:
                    //Logger.Debug("Stun");
                    break;

            }
            Buff buff = new Buff(info, this, RemoveBuff, icon, endEvent);
            buffList.Add(buff);
            bufferState.Buffer(info.type, info.buffValue);
        }
    }
    public void BuffDurationUpdate(int id, float dur) => buffList.Find(t => t.buffInfo.id == id).timer = dur;

    public virtual void RemoveBuff(Buff buff)
    {
        buffList.Remove(buff);
        bufferState.RemoveBuffer(buff.buffInfo.type, buff.buffInfo.buffValue);
        UnitHp = UnitHp;
    }

    public void SetMaxHp()
    {
        UnitHp = MaxHp;
    }

    public int CalculDamage(CharacterSkill skill, ref bool isCritical)
    {
        var buffDamage = skill.CreateDamageResult(characterData.data, bufferState);
        isCritical = UnityEngine.Random.Range(0f, 1f) < characterData.data.critical + (bufferState.criticalProbability);
        if (isCritical)
        {
            buffDamage = (int)(buffDamage * (characterData.data.criticalDmg * bufferState.criticalDamage));
        }
        else
            buffDamage = (int)(buffDamage * bufferState.damageDecrease);


        return buffDamage;
    }

    public void MoveNext(Vector3 movePos)
    {
        SetNoneState();
        pathFind.stoppingDistance = 0f;
        pathFind.SetDestination(movePos);
    }

    public void SetNoneState()
    {
        unitState = UnitState.None;
        battleState = UnitBattleState.None;
    }

    public bool FindNowAttack()
    {
        int idx = 0;
        foreach (var attack in characterData.attacks)
        {
            if (InRangeNormalAttack(attack) && CanNormalAttackTime(attack))
            {
                if (skillClips.Length != 0)
                {
                    //Logger.Debug(name + " " + skillClips[idx].name);

                    clipOverrides["NormalAttack"] = skillClips[idx];
                    animatorOverrideController.ApplyOverrides(clipOverrides);

                }
                nowAttack = attack;
                return true;
            }
            idx++;

        }

        nowAttack = null;
        return false;
    }

    public UnitState GetUnitState()
    {
        return UnitState;
    }

    public void ResetCoolDown()
    {
        foreach (CharacterSkill skill in characterData.attacks)
        {
            lastNormalAttackTime[skill] = Time.time + skill.preCooldown;
        }
    }

    public void SetMoveSpeed(float speed)
    {
        pathFind.speed = speed;
    }
}