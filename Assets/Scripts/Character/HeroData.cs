using System;
using UnityEngine;

public class HeroData : MonoBehaviour, IComparable<HeroData>
{
    public InfoHero info;
    public StatsHero stats;
    public SkillBase commonSkill;   // 일반 공격, 평타
    public SkillBase autoSkill;     // 특수 스킬
    public SkillBase passiveSkill;  // 패시브 스킬
    public SkillBase activeSkill;   // 액티브 스킬, 필살기
    // 스킬 스크립트 사용시 null 체크 필요 (스킬이 없을 수 있음)

    public int CompareTo(HeroData other)
    {
        return info.name.CompareTo(other.info.name);
    }
}