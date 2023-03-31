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

        for (int i = 0; i < attacks.Length; i++)
        {
            attacks[i] = Instantiate(attacks[i]);
        }

        if (activeSkill != null)
            activeSkill = Instantiate(activeSkill);

        if (passiveSkill != null)
            passiveSkill = Instantiate(passiveSkill);


        if (activeSkill != null)
        {
            for (int i = 0; i < activeSkill.buffInfos.Count; i++)
            {
                activeSkill.buffInfos[i].buffLevel = activeSkill.skillLevel;
                activeSkill.buffInfos[i].addBuffState = activeSkill.addBuffState[i];
            }
        }

        foreach (var attack in attacks)
        {
            for (int i = 0; i < attack.buffInfos.Count; i++)
            {
                attack.buffInfos[i].buffLevel = attack.skillLevel;
                attack.buffInfos[i].addBuffState = attack.addBuffState[i];
            }
        }

        if (passiveSkill != null)
        {
            for (int i = 0; i < passiveSkill.buffInfos.Count; i++)
            {
                passiveSkill.buffInfos[i].buffLevel = passiveSkill.skillLevel;
                passiveSkill.buffInfos[i].addBuffState = passiveSkill.addBuffState[i];
            }
        }
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