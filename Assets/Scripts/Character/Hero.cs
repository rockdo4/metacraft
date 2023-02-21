using System;
using UnityEngine;

public class Hero : MonoBehaviour, IComparable<Hero>
{
    public InfoHero info;
    public StatsHero stats;
    public SkillBase commonSkill;
    public SkillBase passiveSkill;
    public SkillBase activeSkill;
    // ��ų ��ũ��Ʈ ���� null üũ �ʿ� (��ų�� ���� �� ����)

    public int CompareTo(Hero other)
    {
        return info.name.CompareTo(other.info.name);
    }
}