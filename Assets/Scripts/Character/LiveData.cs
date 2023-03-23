using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class LiveData : IComparable<LiveData>
{
    // 정보
    public string name;            // 이름
    public int grade = 1;          // 등급
    public int maxGrade = 5;       // 승급 가능 최대 등급

    public int job;                // 직업
    public List<string> tags;      // 태그(특성)

    public int level = 1;          // 레벨
    public int maxLevel;           // 최대 레벨
    public int exp = 0;            // 경험치
    public int likeability = 0;    // 호감도

    // 스텟
    public float baseDamage = 50;     // 일반 공격 데미지
    public float baseDefense = 0;     // 방어력
    public float healthPoint = 500;   // 최대 체력
    public float currentHp = 500;     // 현재 체력
    public float moveSpeed = 3;       // 이동 속도. 범위, 초기값 설정 필요
    public float critical = 0f;     // 크리티컬 확률
    public float criticalDmg = 2f;  // 크리티컬 데미지 배율
    public float evasion = 0f;      // 회피율
    public float accuracy = 1f;     // 명중률

    public int CompareTo(LiveData other)
    {
        return name.CompareTo(other.name);
    }

    public void SetInit(CharacterData originData)
    {
        name = originData.name;
        grade = originData.grade;
        maxGrade = originData.maxGrade;
        job = originData.job;
        
        level = 1;
        exp = 0;
        likeability = 0;

        baseDamage = originData.baseDamage;
        baseDefense = originData.baseDefense;
        healthPoint = originData.healthPoint;
        currentHp = healthPoint;
        moveSpeed = originData.moveSpeed;
        critical = originData.critical;
        criticalDmg = originData.criticalDmg;
        evasion = originData.evasion;
        accuracy = originData.accuracy;

        tags = originData.tags;
    }

    public void SetLoad(string json)
    {
        LiveData parseData = JsonUtility.FromJson<LiveData>(json);

        name = parseData.name;
        grade = parseData.grade;
        maxGrade = parseData.maxGrade;
        job = parseData.job;

        level = parseData.level;
        exp = parseData.exp;
        likeability = parseData.likeability;

        baseDamage = parseData.baseDamage;
        baseDefense = parseData.baseDefense;
        healthPoint = parseData.healthPoint;
        currentHp = healthPoint;
        moveSpeed = parseData.moveSpeed;
        critical = parseData.critical;
        criticalDmg = parseData.criticalDmg;
        evasion = parseData.evasion;
        accuracy = parseData.accuracy;
    }
}