using System;
using UnityEngine;

public class CharacterDataBundle : MonoBehaviour, IComparable<CharacterDataBundle>
{
    public CharacterInfo info;
    public CharacterStats stats;
    
    public CharacterSkill normalAttack;  // �Ϲ� ����, ��Ÿ. cooldown���� �ڵ� ����. (�ʻ��/�Ϲ� ��ų ���� �߿� ��Ÿ ����)
    public CharacterSkill normalSkill;   // �Ϲ� ��ų, ���� �ð�(cooldown)���� ���
    public CharacterSkill passiveSkill;  // �нú� ��ų
    public CharacterSkill activeSkill;   // ��Ƽ�� ��ų, �ʻ��

    // ��ų ��ũ��Ʈ ���� null üũ �ʿ� (��ų�� ���� �� ����)

    public int CompareTo(CharacterDataBundle other)
    {
        return info.name.CompareTo(other.info.name);
    }
}