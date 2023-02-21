using UnityEngine;

[CreateAssetMenu(fileName = "InfoHero", menuName = "InfoBase/InfoHero")]

public class InfoHero : InfoBase
{
    public string       job;            // 직업
    public string       maxGrade;       // 승급 가능 최대 등급
    public int          energy;         // 활동력 소모량
    public int          likeability;    // 호감도
}