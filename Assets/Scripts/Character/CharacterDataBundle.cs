using System;
using UnityEngine;

public class CharacterDataBundle : MonoBehaviour, IComparable<CharacterDataBundle>
{
    public CharacterData originData;
    public LiveData data;

    public CharacterSkill[] attacks;  // �Ϲ� ����, ��Ÿ. cooldown���� �ڵ� ����. (��ų ���� �߿� ��Ÿ ����)
    public CharacterSkill activeSkill;   // ��Ƽ�� ��ų, �ʻ��
    public CharacterSkill passiveSkill;   // �нú�

    private void Awake()
    {
        InitSetting();

        for(int i = 0; i < attacks.Length; i++)
        {
            attacks[i] = Instantiate(attacks[i]);
        }      

        if(activeSkill != null)
            activeSkill = Instantiate(activeSkill);
    }

    public void InitSetting()
    {
        data.SetInit(originData);
        gameObject.name = originData.name;
    }

    public int CompareTo(CharacterDataBundle other)
    {
        return data.name.CompareTo(other.data.name);
    }
}