using TMPro;
using System.Text;
using UnityEngine.UI;

public class HeroInfoDetailScript : View
{
    public Image portrait;
    public TextMeshProUGUI gradeInfoInLeftPanel;
    public TextMeshProUGUI levelInfoInLeftPanel;
    public TextMeshProUGUI typeInfoInLeftPanel;
    public TextMeshProUGUI statDetail;
    public Toggle[] firstToggleTabs;

    public Button[] trainingPlusButtons;

    public Slider expBar;

    public Image[] skillIcons;
    public TextMeshProUGUI[] skillInfo;

    private void OnEnable()
    {
        SetHeroStatInfoText();
        int count = firstToggleTabs.Length;
        for (int i = 0; i < count; i++)
        {
            firstToggleTabs[i].isOn = (i == 0);
        }
    }

    private void SetHeroStatInfoText()
    {
        GameManager gm = GameManager.Instance;

        if (gm.currentSelectObject == null)
            return;
        CharacterDataBundle cdb = gm.currentSelectObject.GetComponent<AttackableUnit>().GetUnitData();
        LiveData data = cdb.data;

        portrait.sprite = gm.GetSpriteByAddress($"illu_{data.name}");

        gradeInfoInLeftPanel.text = $"{(CharacterGrade)data.grade}";
        levelInfoInLeftPanel.text = data.level.ToString();
        typeInfoInLeftPanel.text = $"{gm.GetStringByTable($"herotype_{(CharacterJob)data.job}")}\n";
        StringBuilder stringBuilder = new();
        stringBuilder.Append($"����� �� : {gm.GetStringByTable(data.name)}\n");
        stringBuilder.Append($"���ݷ� : {data.baseDamage:0.00}\n");
        stringBuilder.Append($"���� : {data.baseDefense:0.00}\n");
        stringBuilder.Append($"ü�� : {data.healthPoint:0}\n");
        stringBuilder.Append($"�̵� �ӵ� : {data.moveSpeed}\n");
        stringBuilder.Append($"ũ��Ƽ�� Ȯ�� : {data.critical * 100:0}%\n");
        stringBuilder.Append($"ũ��Ƽ�� ���� : {data.criticalDmg * 100:0}%\n");
        stringBuilder.Append($"���߷� : {data.accuracy * 100:0}%\n");
        stringBuilder.Append($"ȸ���� : {data.evasion * 100:0}%\n");
        statDetail.text = stringBuilder.ToString();
        expBar.value = data.exp;
        expBar.maxValue = (int)gm.expRequirementTable[data.level - 1]["NEEDEXP"];
        expBar.GetComponentInChildren<TextMeshProUGUI>().text = $"EXP {expBar.value:0} / {expBar.maxValue:0}";

        //skillIcons[0].sprite = 
        skillInfo[0].text = $"{gm.GetStringByTable(cdb.attacks[0].skillName)}\n{gm.GetStringByTable(cdb.attacks[0].skillDescription)}";
        //skillIcons[1].sprite = 
        skillInfo[1].text = $"{gm.GetStringByTable(cdb.passiveSkill.skillName)}\n{gm.GetStringByTable(cdb.passiveSkill.skillDescription)}";
        //skillIcons[2].sprite = 
        skillInfo[2].text = $"{gm.GetStringByTable(cdb.activeSkill.skillName)}\n{gm.GetStringByTable(cdb.activeSkill.skillDescription)}";
    }

    //private void SetTrainingPlusButtons()
    //{
    //int testStat = 10;
    //trainingPlusButtons[0].onClick.AddListener(()=>data.baseDamage += testStat);
    //trainingPlusButtons[1].onClick.AddListener(()=>data.baseDefense += testStat);
    //trainingPlusButtons[2].onClick.AddListener(()=>data.healthPoint += testStat);
    //}
}