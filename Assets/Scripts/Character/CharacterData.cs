using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Character/CharacterData")]
public class CharacterData : ScriptableObject, IComparable<CharacterData>
{
    // 정보
    public new string   name;               // 이름
    [Range(1, 5)]
    public int          grade = 1;          // 등급
    [Range(1, 5)]
    public int          maxGrade = 5;       // 승급 가능 최대 등급
    public int          job;                // 직업

    // 스텟
    public float        baseDamage = 0;    // 일반 공격 데미지
    public float        damageLevelCoefficient = 0; // 데미지 레벨업 계수
    public float        baseDefense = 0;    // 방어력
    public float        defenseLevelCoefficient = 0; //방어력 레벨업 계수
    public float        healthPoint = 0;  // 최대 체력
    public float        healthPointLevelCoefficient = 0; // 최대 체력 레벨업 계수
    [Range(0f, 1f)]
    public float        critical = 0f;      // 크리티컬 확률
    [Range(1f, 10f)]
    public float        criticalDmg = 2f;   // 크리티컬 데미지 배율
    public float          moveSpeed = 3;      // 이동 속도. 범위, 초기값 설정 필요
    [Range(0f, 1f)]
    public float        accuracy = 1f;      // 명중률
    [Range(0f, 1f)]
    public float        evasion = 0f;       // 회피율
    public List<string> tags;               // 태그(특성)

    public int CompareTo(CharacterData other)
    {
        return name.CompareTo(other.name);
    }
}