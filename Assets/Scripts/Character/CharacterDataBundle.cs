using System;
using UnityEngine;

public class CharacterDataBundle : MonoBehaviour, IComparable<CharacterDataBundle>
{
    public CharacterData data;
    
    public CharacterSkill attack;  // �Ϲ� ����, ��Ÿ. cooldown���� �ڵ� ����. (�ʻ��/�Ϲ� ��ų ���� �߿� ��Ÿ ����)
    public CharacterSkill skill;   // ��Ƽ�� ��ų, �ʻ��
    //public CharacterSkill normalSkill;   // �Ϲ� ��ų, ���� �ð�(cooldown)���� ���
    //public CharacterSkill passiveSkill;  // �нú� ��ų

    // ��ų ��ũ��Ʈ ���� null üũ �ʿ� (��ų�� ���� �� ����)

    public int CompareTo(CharacterDataBundle other)
    {
        return data.name.CompareTo(other.data.name);
    }
}