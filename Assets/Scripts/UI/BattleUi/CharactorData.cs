using System;
using System.Runtime.CompilerServices;

public class CharactorData
{
    public string heroName;
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

    private string printFormat = "{0} : {1}";
        
    public void PrintBattleInfo()
    {
        Logger.Debug(string.Format(printFormat, "데미지", damage));
        //Logger.Debug(string.Format(printFormat, "방어력", def));
        //Logger.Debug(string.Format(printFormat, "체력", hp));
        //Logger.Debug(string.Format(printFormat, "이동속도", speed));
        //Logger.Debug(string.Format(printFormat, "크리티컬", chritical));
        //Logger.Debug(string.Format(printFormat, "크리티컬 배율", chriticalDmg));
        //Logger.Debug(string.Format(printFormat, "명중률", accuracy));
        //Logger.Debug(string.Format(printFormat, "레벨", level));
        //Logger.Debug(string.Format(printFormat, "타입", type));
        Logger.Debug("");
    }
}