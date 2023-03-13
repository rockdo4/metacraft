using System;
using UnityEngine;

public class CharacterDataBundle : MonoBehaviour, IComparable<CharacterDataBundle>
{
    public CharacterData originData;
    public LiveData data;

    //public CharacterSkill attack;  // �Ϲ� ����, ��Ÿ. cooldown���� �ڵ� ����. (�ʻ��/�Ϲ� ��ų ���� �߿� ��Ÿ ����)
    public CharacterSkill[] attacks;  // �Ϲ� ����, ��Ÿ. cooldown���� �ڵ� ����. (�ʻ��/�Ϲ� ��ų ���� �߿� ��Ÿ ����)
    public CharacterSkill activeSkill;   // ��Ƽ�� ��ų, �ʻ��
    public CharacterSkill passiveSkill;  // �нú� ��ų
    //public CharacterSkill normalSkill;   // �Ϲ� ��ų, ���� �ð�(cooldown)���� ���

    // ��ų ��ũ��Ʈ ���� null üũ �ʿ� (��ų�� ���� �� ����)

    private void Awake()
    {
        InitSetting();

        activeSkill = Instantiate(activeSkill);
    }

    public void InitSetting()
    {
        data.SetInit(originData);
    }

    public int CompareTo(CharacterDataBundle other)
    {
        return data.name.CompareTo(other.data.name);
    }
}