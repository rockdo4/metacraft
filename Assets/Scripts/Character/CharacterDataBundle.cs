using System;
using UnityEngine;

public class CharacterDataBundle : MonoBehaviour, IComparable<CharacterDataBundle>
{
    public CharacterData originData;
    public LiveData data;

    public CharacterSkill[] attacks;  // 일반 공격, 평타. cooldown마다 자동 공격. (필살기/일반 스킬 쓰는 중에 평타 멈춤)
    public CharacterSkill activeSkill;   // 액티브 스킬, 필살기
    public CharacterSkill passiveSkill;  // 패시브 스킬

    // 스킬 스크립트 사용시 null 체크 필요 (스킬이 없을 수 있음)

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