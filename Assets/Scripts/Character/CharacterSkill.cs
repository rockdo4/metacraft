using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

[Serializable]
public class BuffLevel
{
    public List<BuffInfo> list;

    public BuffInfo this[int index] {
        get { return list[index]; }
        set { list[index] = value; }
    }
}

[CreateAssetMenu(fileName = "CharacterSkill", menuName = "Character/CharacterSkill")]
public class CharacterSkill : ScriptableObject
{
    public Transform ActorTransform { set { actorTransform = value; } }
    protected Transform actorTransform;    

    public Transform SkillHolderTransform { set { skillHolderTransform = value; } }
    protected Transform skillHolderTransform;

    public List<AttackableUnit> SkillEffectedUnits { get { return skillEffectedUnits; } }
    protected List<AttackableUnit> skillEffectedUnits;    

    //public List<(BuffType bufftype, float value)> buffTypeAndValue;

    public int      skillLevel = 1;

    public int      id;
    public string   skillName;
    
    public float    preCooldown;
    public float    cooldown;       // 보정 되지 않은 쿨다운
    public float    distance;       // 적용 범위 (= 네비게이션 정지 거리)
    public int      targetNumLimit;          // 스킬 최대 적용 개체수
    public float    angle;          // 적용 범위 각. gameObject.transform.forward를 기준으로 좌우로 angle/2(도) 만큼 적용

    // 공격 이외 디버프, 버프도 있을 수 있기 때문에 적용으로 명명함
    // 일반 스킬(평타)을 해당 클래스 통해서 작성
    public SkillCoefficientType coefficientType = SkillCoefficientType.Attack; // 스킬 계수 타입. 공격력, 방어력, 체력 등등   
    public float coefficient = 1; //계수
    public float addCoefficient;

    public SkillTargetType targetType;
    public SkillSearchType searchType;
    public EffectEnum readyEffect;
    public EffectEnum activeEffect;
    public EffectEnum hitEffect;

    public bool isCriticalPossible;
    public bool isAuto;
    public virtual bool IsAutoStart { get; }
    public Vector3 targetPos;
    public string skillDescription;
    public string skillIcon;

    public List<BuffLevel> buffInfos;
    public int buffRadius;
    public BufferTargetType buffTargetType;
    public int buffTargetCnt;


    public int priority;

    public virtual void OnActive()
    {
    }

    public virtual IEnumerator SkillCoroutine()
    {
        yield break;  
    }
    //대미지 = (공격자 공격력*스킬계수) * (100/100+방어력) * (1 + 레벨보정)									
    public virtual int CreateDamageResult(LiveData data, BufferState status)
    {
        var currCoefficient = coefficient + skillLevel * addCoefficient;

        switch (coefficientType)
        {
            case SkillCoefficientType.Attack:
                return (int)((data.baseDamage * status.Damage) * currCoefficient);                
            case SkillCoefficientType.Defense:
                return (int)((data.baseDefense * status.defense) * currCoefficient);                
            case SkillCoefficientType.MaxHealth:
                return (int)((data.healthPoint * status.maxHealthIncrease) * currCoefficient);
            case SkillCoefficientType.Health:
                return (int)(data.currentHp * currCoefficient);                
        }        

        return 0;
    }
    public virtual void NormalAttackOnDamage()
    {
        if (activeEffect.Equals(EffectEnum.None))
            return;
        
        EffectManager.Instance.Get(activeEffect, skillHolderTransform ?? actorTransform, actorTransform.rotation);
    }
    //public void OnActiveSkilThroughToLastChild(AttackableUnit unit)
    //{
    //    OnActiveSkill(unit);
    //}

    public virtual void OnActiveSkill(AttackableUnit unit, List<AttackableUnit> enemies, List<AttackableUnit> heros)
    {
        switch (buffTargetType)
        {
            case BufferTargetType.None:
                break;
            case BufferTargetType.Self:
                foreach (var buff in buffInfos)
                {
                    if (buff[0].type == BuffType.Provoke
                        || buff[0].type == BuffType.Stun
                        || buff[0].type == BuffType.Silence)
                        unit.AddStateBuff(buff[0]);
                    else
                        unit.AddValueBuff(buff[0]);
                }
                break;
            case BufferTargetType.Friendly:
            case BufferTargetType.Enemy:
                {
                    var finalTargets = FindTargetInArea(unit, buffTargetType, enemies, heros);
                    for (int i = 0; i < finalTargets.Count; i++)
                    {
                        foreach (var buff in buffInfos)
                        {
                            var nowBuff = buff[0];
                            if (nowBuff.type == BuffType.Provoke
                                || nowBuff.type == BuffType.Stun
                                || nowBuff.type == BuffType.Silence)
                                finalTargets[i].AddStateBuff(nowBuff, unit);
                            else
                            {
                                finalTargets[i].AddValueBuff(nowBuff);
                            }
                        }
                    }
                }
                break;
            case BufferTargetType.Both:
                break;
            case BufferTargetType.Count:
                break;
            default:
                break;
        }
    }

    public List<AttackableUnit> FindTargetInArea(AttackableUnit unit, BufferTargetType b_targetType, List<AttackableUnit> enemies, List<AttackableUnit> heros)
    {
        List<AttackableUnit> closeTargets = new List<AttackableUnit>();

        var searchTarget = b_targetType == BufferTargetType.Friendly ? UnitType.Hero : UnitType.Enemy;
        List<AttackableUnit> targets = (searchTarget == UnitType.Hero ? heros : enemies).ToList();

        foreach (var target in targets)
        {
            if (target == null)
                continue;
            if (Vector3.Distance(unit.transform.position, target.transform.position) <= buffRadius)
            {
                closeTargets.Add(target);
            }
        }

        // 선택된 대상들을 거리가 가까운 순으로 정렬합니다.
        for (int i = 0; i < closeTargets.Count - 1; i++)
        {
            for (int j = i + 1; j < closeTargets.Count; j++)
            {
                float distance1 = Vector3.Distance(unit.transform.position, closeTargets[i].transform.position);
                float distance2 = Vector3.Distance(unit.transform.position, closeTargets[j].transform.position);

                if (distance2 < distance1)
                {
                    AttackableUnit temp = closeTargets[i];
                    closeTargets[i] = closeTargets[j];
                    closeTargets[j] = temp;
                }
            }
        }

        // 최대 maxTargets 개수까지의 대상만 선택합니다.
        List<AttackableUnit> finalTargets = new List<AttackableUnit>();
        var finalTargetCount = Mathf.Min(closeTargets.Count, buffTargetCnt);
        for (int i = 0; i < finalTargetCount; i++)
        {
            finalTargets.Add(closeTargets[i]);
        }


        return finalTargets;
    }
}