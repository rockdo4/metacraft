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
    public float    cooldown;       // ���� ���� ���� ��ٿ�
    public float    distance;       // ���� ���� (= �׺���̼� ���� �Ÿ�)
    public int      targetNumLimit;          // ��ų �ִ� ���� ��ü��
    public float    angle;          // ���� ���� ��. gameObject.transform.forward�� �������� �¿�� angle/2(��) ��ŭ ����

    // ���� �̿� �����, ������ ���� �� �ֱ� ������ �������� �����
    // �Ϲ� ��ų(��Ÿ)�� �ش� Ŭ���� ���ؼ� �ۼ�
    public SkillCoefficientType coefficientType = SkillCoefficientType.Attack; // ��ų ��� Ÿ��. ���ݷ�, ����, ü�� ���   
    public float coefficient = 1; //���

    public SkillTargetType targetType;
    public SkillSearchType searchType;
    public EffectEnum readyEffect;
    public EffectEnum activeEffect;
    public EffectEnum hitEffect;

    public bool isCriticalPossible;
    public bool isAuto;
    public Vector3 targetPos;
    public string skillDescription;

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
    //����� = (������ ���ݷ�*��ų���) * (100/100+����) * (1 + ��������)									
    public virtual int CreateDamageResult(LiveData data, BufferState status)
    {        
        var result = 0;
        switch (coefficientType)
        {
            case SkillCoefficientType.Attack:
                result = (int)((data.baseDamage * status.Damage) * coefficient);
                break;
            case SkillCoefficientType.Defense:
                result = (int)((data.baseDefense * status.defense) * coefficient);
                break;
            case SkillCoefficientType.MaxHealth:
                result = (int)((data.healthPoint * status.maxHealthIncrease) * coefficient);
                break;
            case SkillCoefficientType.Health:
                result = (int)(data.currentHp * coefficient);
                break;
        }
        //if (targetType == SkillTargetType.Friendly)
        //    result *= -1;

        return result;
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
    public virtual void OnActiveSkill(AttackableUnit unit)
    {
        switch (buffTargetType)
        {
            case BufferTargetType.None:
                break;
            case BufferTargetType.Self:
                foreach (var buff in buffInfos)
                {
                    if (buff[skillLevel - 1].type == BuffType.Provoke
                        || buff[skillLevel - 1].type == BuffType.Stun
                        || buff[skillLevel - 1].type == BuffType.Silence)
                        unit.AddStateBuff(buff[skillLevel - 1]);
                    else
                        unit.AddValueBuff(buff[skillLevel - 1]);
                }
                break;
            case BufferTargetType.Friendly:
            case BufferTargetType.Enemy:
                {
                    var finalTargets = FindTargetInArea(unit);
                    for (int i = 0; i < finalTargets.Count; i++)
                    {
                        foreach (var buff in buffInfos)
                        {
                            if (buff[skillLevel - 1].type == BuffType.Provoke
                                || buff[skillLevel - 1].type == BuffType.Stun
                                || buff[skillLevel - 1].type == BuffType.Silence)
                                finalTargets[i].AddStateBuff(buff[skillLevel - 1], unit);
                            else
                                finalTargets[i].AddValueBuff(buff[skillLevel - 1]);
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

    public List<AttackableUnit> FindTargetInArea(AttackableUnit unit)
    {
        List<AttackableUnit> closeTargets = new List<AttackableUnit>();

        var searchTarget = targetType == SkillTargetType.Friendly ? UnitType.Hero : UnitType.Enemy; 
        List<AttackableUnit> targets = Physics.OverlapSphere(unit.transform.position, buffRadius).Select(t=>t.GetComponent<AttackableUnit>()).ToList();

        foreach (var target in targets)
        {
            if (target == null)
                continue;
            if (target.unitType == searchTarget &&
                Vector3.Distance(unit.transform.position, target.transform.position) <= buffRadius)
            {
                closeTargets.Add(target);
            }
        }

        // ���õ� ������ �Ÿ��� ����� ������ �����մϴ�.
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

        // �ִ� maxTargets ���������� ��� �����մϴ�.
        List<AttackableUnit> finalTargets = new List<AttackableUnit>();
        var finalTargetCount = Mathf.Min(closeTargets.Count, buffTargetCnt);
        for (int i = 0; i < finalTargetCount; i++)
        {
            finalTargets.Add(closeTargets[i]);
        }


        return finalTargets;
    }
}