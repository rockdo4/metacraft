using UnityEngine;

public abstract class StatsBase : ScriptableObject
{
    public int Power
    {
        // 전투력 계산식
        private set { power = baseDamage + healthPoint; }
        get { return power; }
    }
    
    private int             power;              // 전투력
    public int              baseDamage = 50;    // 일반 공격 데미지
    public int              baseDefense = 0;    // 방어력
    public int              healthPoint = 500;  // 최대 체력
    public int              moveSpeed;          // 이동 속도. 범위, 초기값 설정 필요
    [Range(0f, 1f)]
    public float            critical = 0f;      // 크리티컬 확률
    [Range(1f, 10f)]                            
    public float            criticalDmg = 2f;   // 크리티컬 데미지 배율
    [Range(0f, 1f)]                             
    public float            evasion = 0f;       // 회피율
    [Range(0f, 1f)]                             
    public float            accuracy = 0.5f;    // 명중률
}