using System;

public class CharactorData
{
    public int damage; //일반 공격 데미지
    public int def; //방어력
    public int hp; //체력
    public int speed; //이속
    public float chritical; //크리티컬 확률
    public float chriticalDmg; //크리티컬 데미지 비율
    public float evasion; //회피율
    public float accuracy; //명중률
    public char grade; //등급
    public int level; //레벨
    public string type; //직업

    public float cooldown;
    public float skillCooldown;

    public int exp;

    public Action attackEvent;
    public Action skillEvent;
    public Action passiveEvent;
}