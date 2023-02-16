using UnityEngine;

// 이펙트 데이터 생성용 스크립터블오브젝트
[CreateAssetMenu(fileName = "Effect", menuName = "Effect/EffectData")]
public class EffectData : ScriptableObject
{
    public ParticleSystem particle;
    public float startDelay;        // 이펙트 시작 전 딜레이
    public float duration;          // 지속시간
    public float effectSize;        // 이펙트 크기
}
