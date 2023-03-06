using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "CharacterSkill", menuName = "Character/CharacterSkill")]
public class CharacterSkill : ScriptableObject
{
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

    public SkillTargetType targetType;

    public virtual void OnActive()
    {

    }
    public virtual IEnumerator SkillCoroutine()
    {
        yield break;  
    }
    //대미지 = (공격자 공격력*스킬계수) * (100/100+방어력) * (1 + 레벨보정)									
    public virtual int CreateDamageResult(LiveData data)
    {        
        var result = 0;
        switch (coefficientType)
        { 
            case SkillCoefficientType.Attack:
                result = (int)(data.baseDamage * coefficient);
                break;
            case SkillCoefficientType.Defense:
                result = (int)(data.baseDefense * coefficient);
                break;
            case SkillCoefficientType.MaxHealth:
                result = (int)(data.healthPoint * coefficient);
                break;
            case SkillCoefficientType.Health:
                result = (int)(data.currentHp * coefficient);
                break;
        }
        if (targetType == SkillTargetType.Friendly)
            result *= -1;

        return result;
    }
    public virtual void OnActiveSkill(LiveData data) { }

    public virtual void SkillCancle() { }
}