using System.Collections.Generic;
using System;
using UnityEngine;
using System.Text;

[Serializable]
public class LiveData : IComparable<LiveData>
{
    public string name;               // 이름
    public string grade;              // 등급
    public string job;                // 직업

    public string maxGrade;           // 승급 가능 최대 등급
    public int energy;             // 활동력 소모량

    [Range(1, 20)]
    public int level = 1;          // 레벨
    public int exp;                // 경험치
    [Range(0, 1000)]
    public int likeability;        // 호감도

    public int Power
    {
        // 전투력 계산식
        //private set { power = baseDamage + baseDefense + healthPoint * 10; }
        get { return baseDamage + baseDefense + healthPoint * 10; }
    }

    //private int power;              // 전투력
    public int baseDamage = 50;     // 일반 공격 데미지
    public int baseDefense = 0;     // 방어력
    public int healthPoint = 500;   // 최대 체력
    public int currentHP = 500;     // 현재 체력
    public int moveSpeed = 3;        // 이동 속도. 범위, 초기값 설정 필요
    [Range(0f, 1f)]
    public float critical = 0f;      // 크리티컬 확률
    [Range(1f, 10f)]
    public float criticalDmg = 2f;   // 크리티컬 데미지 배율
    [Range(0f, 1f)]
    public float evasion = 0f;       // 회피율
    [Range(0f, 1f)]
    public float accuracy = 1f;    // 명중률

    public List<int> synergys;       // 시너지 리스트. 정수 or Enum으로 가지고 있다가
                                     // 공격대에 같은 시너지 index를 가지고 있는 영웅이 있으면 시너지 발동

    public int CompareTo(LiveData other)
    {
        return name.CompareTo(other.name);
    }

    public void SetInit(CharacterData originData)
    {
        name = originData.name;
        grade = originData.grade;
        job = originData.job;
        maxGrade = originData.maxGrade;
        energy = originData.energy;
        
        // Load from save file
        level = 1;
        exp = 0;
        likeability = originData.likeability;

        baseDamage = originData.baseDamage;
        baseDefense = originData.baseDefense;
        healthPoint = originData.healthPoint;
        currentHP = originData.currentHP;
        critical = originData.critical;
        criticalDmg = originData.criticalDmg;
        evasion = originData.evasion;
        accuracy = originData.accuracy;

        synergys = originData.synergys;
    }

    public void SetLoad(string json)
    {
        LiveData parseData = JsonUtility.FromJson<LiveData>(json);

        name = parseData.name;
        grade = parseData.grade;
        job = parseData.job;
        maxGrade = parseData.maxGrade;
        energy = parseData.energy;

        // Load from save file
        level = parseData.level;
        exp = parseData.exp;
        likeability = parseData.likeability;

        baseDamage = parseData.baseDamage;
        baseDefense = parseData.baseDefense;
        healthPoint = parseData.healthPoint;
        currentHP = parseData.currentHP;
        critical = parseData.critical;
        criticalDmg = parseData.criticalDmg;
        evasion = parseData.evasion;
        accuracy = parseData.accuracy;

        synergys = parseData.synergys;
    }

    public void TestPrint()
    {
        StringBuilder sb = new();
        sb.Append($"히어로 명 : {name}\n");
        sb.Append($"공격력 : {baseDamage}\n");
        sb.Append($"방어력 : {baseDefense}\n");
        sb.Append($"체력 : {currentHP}/{healthPoint}\n");
        sb.Append($"타입 : {job}\n");
        sb.Append($"활동력 소모량 : {energy}\n");
        sb.Append($"치명타 확률 : {critical}\n");
        sb.Append($"치명타 배율 : {criticalDmg}\n");
        sb.Append($"이동 속도 : {moveSpeed}\n");
        sb.Append($"명중률 : {accuracy}\n");
        sb.Append($"회피율 : {evasion}\n");
        Logger.Debug(sb);
    }
}