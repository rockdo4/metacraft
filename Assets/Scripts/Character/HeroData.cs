using System;
using UnityEngine;

public class HeroData : MonoBehaviour, IComparable<HeroData>
{
    public InfoHero info;
    public StatsHero stats;
    public SkillBase commonSkill;   // �Ϲ� ����, ��Ÿ
    public SkillBase autoSkill;     // Ư�� ��ų
    public SkillBase passiveSkill;  // �нú� ��ų
    public SkillBase activeSkill;   // ��Ƽ�� ��ų, �ʻ��
    // ��ų ��ũ��Ʈ ���� null üũ �ʿ� (��ų�� ���� �� ����)

    public int CompareTo(HeroData other)
    {
        return info.name.CompareTo(other.info.name);
    }
}