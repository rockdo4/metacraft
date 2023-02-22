using UnityEngine;

[CreateAssetMenu(fileName = "SkillBase", menuName = "SkillBase/SkillBase")]
public class SkillBase : ScriptableObject
{
    public float    cooldown;       // 보정 되지 않은 쿨다운
    public float    distance;       // 적용 범위 (= 네비게이션 정지 거리)
    public int      count;          // 스킬 최대 적용 개체수
    public float    angle;          // 적용 범위 각. gameObject.transform.forward를 기준으로 좌우로 angle/2(도) 만큼 적용
    
    // 공격 이외 디버프, 버프도 있을 수 있기 때문에 적용으로 명명함
    // 일반 스킬(평타)을 해당 클래스 통해서 작성
}