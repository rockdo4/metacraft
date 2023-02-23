using UnityEngine;
using TMPro;
using System.Text;

public class HeroInfoDetailScript : MonoBehaviour
{
    public CharacterData data;

    public TextMeshProUGUI gradeInfoInLeftPanel;
    public TextMeshProUGUI levelInfoInfoInLeftPanel;
    public TextMeshProUGUI statDetail;
    private void Start()
    {
        gradeInfoInLeftPanel.text = data.grade;
        levelInfoInfoInLeftPanel.text = data.level.ToString();

        statDetail.text = SetText();
    }
    private string SetText()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append($"히어로 명 : {data.name}\n");
        stringBuilder.Append($"공격력 : {data.baseDamage}\n");
        stringBuilder.Append($"방어력 : {data.baseDefense}\n");
        stringBuilder.Append($"체력 : {data.healthPoint}\n");
        stringBuilder.Append($"타입 : {data.job}\n");
        stringBuilder.Append($"활동력 소모량 : {data.energy}\n");
        stringBuilder.Append($"치명타 확률 : {data.critical}\n");
        stringBuilder.Append($"치명타 배율 : {data.criticalDmg}\n");
        stringBuilder.Append($"이동 속도 : {data.moveSpeed}\n");
        stringBuilder.Append($"명중률 : {data.accuracy}\n");
        stringBuilder.Append($"회피율 : {data.evasion}\n");
        return stringBuilder.ToString();
    }
}
