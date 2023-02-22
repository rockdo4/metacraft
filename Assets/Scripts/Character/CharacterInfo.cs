using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterInfo", menuName = "Character/CharacterInfo")]
public class CharacterInfo : ScriptableObject, IComparable<CharacterInfo>
{
    public new string   name;               // 이름
    public string       grade;              // 등급
    public string       job;                // 직업

    public string       maxGrade;           // 승급 가능 최대 등급
    public int          energy;             // 활동력 소모량
    public int          likeability;        // 호감도
    public int          level;              // 레벨
    public int          exp;                // 경험치

    public int CompareTo(CharacterInfo other)
    {
        return name.CompareTo(other.name);
    }
}