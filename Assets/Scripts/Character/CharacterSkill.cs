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
    public EffectEnum effectEnum;

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
        var result = 0;
        switch (coefficientType)
        {
            case SkillCoefficientType.Attack:
                result = (int)((data.baseDamage + status.Damage) * coefficient);
                break;
            case SkillCoefficientType.Defense:
                result = (int)((data.baseDefense + status.defense) * coefficient);
                break;
            case SkillCoefficientType.MaxHealth:
                result = (int)((data.healthPoint + (data.healthPoint * (status.maxHealthIncrease / 100f))) * coefficient);
                break;
            case SkillCoefficientType.Health:
                result = (int)((data.currentHp + status.Damage) * coefficient);
                break;
        }
        //if (targetType == SkillTargetType.Friendly)
        //    result *= -1;

        return result;
    }
    public virtual void OnActiveSkill(int damaage, int level , bool isCritical) { }
}