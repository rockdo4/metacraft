using UnityEngine;

[CreateAssetMenu(fileName = "SkillSpecial", menuName = "SkillBase/SkillSpecial")]

public class SkillSpecial : SkillBase
{
    // 돌진, 도트 데미지, 버프, 디버프, 상태 이상 등
    // 추가 효과가 필요한 스킬을 위한 클래스
    public float dashDistance;
}