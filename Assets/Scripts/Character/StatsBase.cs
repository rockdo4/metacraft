using System.Collections.Generic;
using UnityEngine;

public abstract class StatsBase : ScriptableObject
{
    private int             power;              // 전투력
    public int Power
    {
        // 전투력 계산식
        private set { power = value; }
        get { return power; }
    }

    public int              baseDamage;         // 일반 공격 데미지
    public int              baseDefense;        // 방어력
    public int              healthPoint;        // 최대 체력
    public int              moveSpeed;          // 이동 속도

    public float            attackDistance;     // 공격 범위 = 네비게이션 스탑 디스턴스
    public int              attackCount;        // 일반스킬 최대 공격 개체수

    [Range(0f, 1f)]
    public float            critical;           // 크리티컬 확률
    [Range(1f, 10f)]        
    public float            criticalDmg;        // 크리티컬 데미지 배율
    [Range(0f, 1f)]
    public float            evasion;            // 회피율
    [Range(0f, 1f)]
    public float            accuracy;           // 명중률

    public List<float>      cooldowns;          // 쿨타임
    // public List<Skill>   skills;             // 스킬 목록
}