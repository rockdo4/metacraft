using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    public AnimationClip animationClip;

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

    public List<BuffInfo> buffInfos;

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

        EffectManager.Instance.Get(activeEffect, skillHolderTransform, actorTransform.rotation);
    }
    public virtual void OnActiveSkill(AttackableUnit unit) { }
}