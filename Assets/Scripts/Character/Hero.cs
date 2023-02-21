using System;
using UnityEngine;

public class Hero : MonoBehaviour, IComparable<Hero>
{
    public InfoHero info;
    public StatsHero stats;
    public SkillBase commonSkill;
    public SkillBase passiveSkill;
    public SkillBase activeSkill;
    // 스킬 스크립트 사용시 null 체크 필요 (스킬이 없을 수 있음)

    public int CompareTo(Hero other)
    {
        return info.name.CompareTo(other.info.name);
    }
}