using System;
using UnityEngine;

public class CharacterDataBundle : MonoBehaviour, IComparable<CharacterDataBundle>
{
    public CharacterData originData;
    public LiveData data;

    public CharacterSkill[] attacks;  // 일반 공격, 평타. cooldown마다 자동 공격. (스킬 쓰는 중에 평타 멈춤)
    public CharacterSkill activeSkill;   // 액티브 스킬, 필살기
    public CharacterSkill passiveSkill;   // 패시브

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