using System;
using UnityEngine;

public class CharacterDataBundle : MonoBehaviour, IComparable<CharacterDataBundle>
{
    public CharacterData originData;
    public LiveData data;

    public CharacterSkill[] attacks;  // �Ϲ� ����, ��Ÿ. cooldown���� �ڵ� ����. (��ų ���� �߿� ��Ÿ ����)
    public CharacterSkill activeSkill;   // ��Ƽ�� ��ų, �ʻ��

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