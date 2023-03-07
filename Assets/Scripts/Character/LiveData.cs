using System;
using UnityEngine;
using System.Text;

[Serializable]
public class LiveData : IComparable<LiveData>
{
    public string name;            // 이름
    public int grade;              // 등급
    public int maxGrade;           // 승급 가능 최대 등급

    public string job;             // 직업, 특성

    public int level = 1;          // 레벨
    public int exp;                // 경험치
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
    public int currentHp = 500;   // 현재 체력
    public int moveSpeed = 3;        // 이동 속도. 범위, 초기값 설정 필요
    public float critical = 0f;      // 크리티컬 확률
    public float criticalDmg = 2f;   // 크리티컬 데미지 배율
    public float evasion = 0f;       // 회피율
    public float accuracy = 1f;    // 명중률

    public GameObject equipment; // 장비

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
        likeability = originData.likeability;

        baseDamage = originData.baseDamage;
        baseDefense = originData.baseDefense;
        healthPoint = originData.healthPoint;
        currentHp = healthPoint;
        moveSpeed = originData.moveSpeed;
        critical = originData.critical;
        criticalDmg = originData.criticalDmg;
        evasion = originData.evasion;
        accuracy = originData.accuracy;
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

    public void TestPrint()
    {
        StringBuilder sb = new();
        sb.Append($"히어로 명 : {name}\n");
        sb.Append($"공격력 : {baseDamage}\n");
        sb.Append($"방어력 : {baseDefense}\n");
        sb.Append($"체력 : {healthPoint}\n");
        sb.Append($"이동 속도 : {moveSpeed}\n");
        sb.Append($"타입 : {job}\n");
        sb.Append($"치명타 확률 : {critical}\n");
        sb.Append($"치명타 배율 : {criticalDmg}\n");
        sb.Append($"명중률 : {accuracy}\n");
        sb.Append($"회피율 : {evasion}\n");
        Logger.Debug(sb);
    }
}