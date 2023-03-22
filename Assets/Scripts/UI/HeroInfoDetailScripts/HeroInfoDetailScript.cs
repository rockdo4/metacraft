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
        LiveData data = gm.currentSelectObject.GetComponent<AttackableUnit>().GetUnitData().data;
        portrait.sprite = gm.GetSpriteByAddress($"Illu_{data.name}");

        gradeInfoInLeftPanel.text = $"{(CharacterGrade)data.grade}";
        levelInfoInLeftPanel.text = data.level.ToString();
        typeInfoInLeftPanel.text = $"{gm.GetStringByTable($"herotype_{(CharacterJob)data.job}")}\n";
        StringBuilder stringBuilder = new();
        stringBuilder.Append($"����� �� : {gm.GetStringByTable(data.name)}\n");
        stringBuilder.Append($"���ݷ� : {data.baseDamage}\n");
        stringBuilder.Append($"���� : {data.baseDefense}\n");
        stringBuilder.Append($"ü�� : {data.healthPoint}\n");
        stringBuilder.Append($"Ÿ�� : {gm.GetStringByTable($"herotype_{(CharacterJob)data.job}")}\n");
        stringBuilder.Append($"ġ��Ÿ Ȯ�� : {data.critical * 100:0}%\n");
        stringBuilder.Append($"ġ��Ÿ ���� : {data.criticalDmg * 100:0}%\n");
        stringBuilder.Append($"�̵� �ӵ� : {data.moveSpeed}\n");
        statDetail.text = stringBuilder.ToString();
        expBar.value = data.exp;
        expBar.maxValue = (int)gm.expRequirementTable[data.level - 1]["NEEDEXP"];
        expBar.GetComponentInChildren<TextMeshProUGUI>().text = $"EXP {expBar.value:0} / {expBar.maxValue:0}";
    }

    //private void SetTrainingPlusButtons()
    //{
    //int testStat = 10;
    //trainingPlusButtons[0].onClick.AddListener(()=>data.baseDamage += testStat);
    //trainingPlusButtons[1].onClick.AddListener(()=>data.baseDefense += testStat);
    //trainingPlusButtons[2].onClick.AddListener(()=>data.healthPoint += testStat);
    //}
}