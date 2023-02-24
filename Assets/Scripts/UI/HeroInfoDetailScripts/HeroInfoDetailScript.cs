using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.UI;
public class HeroInfoDetailScript : MonoBehaviour
{
    public CharacterData data;

    public TextMeshProUGUI gradeInfoInLeftPanel;
    public TextMeshProUGUI levelInfoInfoInLeftPanel;
    public TextMeshProUGUI statDetail;

    public Button[] trainingPlusButtons;
    private void Start()
    {
        gradeInfoInLeftPanel.text = data.grade;
        levelInfoInfoInLeftPanel.text = data.level.ToString();

        statDetail.text = SetHeroStatInfoText();
        SetTrainingPlusButtons();
    }
    private string SetHeroStatInfoText()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append($"����� �� : {data.name}\n");
        stringBuilder.Append($"���ݷ� : {data.baseDamage}\n");
        stringBuilder.Append($"���� : {data.baseDefense}\n");
        stringBuilder.Append($"ü�� : {data.healthPoint}\n");
        stringBuilder.Append($"Ÿ�� : {data.job}\n");
        stringBuilder.Append($"Ȱ���� �Ҹ� : {data.energy}\n");
        stringBuilder.Append($"ġ��Ÿ Ȯ�� : {data.critical}\n");
        stringBuilder.Append($"ġ��Ÿ ���� : {data.criticalDmg}\n");
        stringBuilder.Append($"�̵� �ӵ� : {data.moveSpeed}\n");
        stringBuilder.Append($"���߷� : {data.accuracy}\n");
        stringBuilder.Append($"ȸ���� : {data.evasion}\n");
        return stringBuilder.ToString();
    }
    private void SetTrainingPlusButtons()
    {
        //int testStat = 10;
        //trainingPlusButtons[0].onClick.AddListener(()=>data.baseDamage += testStat);
        //trainingPlusButtons[1].onClick.AddListener(()=>data.baseDefense += testStat);
        //trainingPlusButtons[2].onClick.AddListener(()=>data.healthPoint += testStat);
    }
}
