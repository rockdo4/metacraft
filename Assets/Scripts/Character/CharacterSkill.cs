using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "CharacterSkill", menuName = "Character/CharacterSkill")]
public class CharacterSkill : ScriptableObject
{
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

    public bool isCriticalPossible;
    public bool isAuto;
    public Vector3 targetPos;
    public string skillDescription;

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
        switch (coefficientType)
        {
            case SkillCoefficientType.Attack:
                return (int)((data.baseDamage + status.Damage) * coefficient);                
            case SkillCoefficientType.Defense:
                return (int)((data.baseDefense + status.defense) * coefficient);                
            case SkillCoefficientType.MaxHealth:
                return (int)((data.healthPoint + (data.healthPoint * (status.maxHealthIncrease / 100f))) * coefficient);            
            case SkillCoefficientType.Health:
                return (int)((data.currentHp + status.Damage) * coefficient);
        }
        return 0;
    }
    public virtual void OnActiveSkill(AttackableUnit unit) { }
}