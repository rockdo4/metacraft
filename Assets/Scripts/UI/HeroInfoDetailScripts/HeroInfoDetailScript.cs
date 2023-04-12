using TMPro;
using System.Text;
using UnityEngine.UI;
using System.Collections.Generic;

public class HeroInfoDetailScript : View
{
    public Image portrait;
    public TextMeshProUGUI gradeInfoInLeftPanel;
    public TextMeshProUGUI levelInfoInLeftPanel;
    public Image typeInfoInLeftPanel;
    public TextMeshProUGUI statDetail;
    public List<TextMeshProUGUI> statDetailNames;
    public List<TextMeshProUGUI> statDetailTexts;
    public Toggle[] firstToggleTabs;

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
        typeInfoInLeftPanel.sprite = gm.GetSpriteByAddress($"icon_type_{(CharacterJob)data.job}");

        List<string> infoNames = new List<string>
        {
            "히어로 명",
            "공격력",
            "방어력",
            "체력",
            "이동 속도",
            "크리티컬 확률",
            "크리티컬 배율"
        };
        List<string> infos = new List<string>
        {
            $"{gm.GetStringByTable(data.name)}",
            $"{data.baseDamage:0.00}",
            $"{data.baseDefense:0.00}",
            $"{data.healthPoint:0}",
            $"{data.moveSpeed}",
            $"{data.critical * 100:0}",
            $"{data.criticalDmg * 100:0}"
        };
        SetInfoText(statDetailNames, infoNames);
        SetInfoText(statDetailTexts, infos);
        expBar.maxValue = gm.expRequirementTable[data.level];
        expBar.value = data.exp;
        expBar.GetComponentInChildren<TextMeshProUGUI>().text = $"EXP {expBar.value:0} / {expBar.maxValue:0}";

        skillIcons[0].sprite = gm.GetSpriteByAddress($"{cdb.attacks[0].skillIcon}");
        skillInfo[0].text = $"{gm.GetStringByTable(cdb.attacks[0].skillName)}\n{gm.GetStringByTable(cdb.attacks[0].skillDescription)}";
        skillIcons[1].sprite = gm.GetSpriteByAddress($"{cdb.passiveSkill.skillIcon}");
        skillInfo[1].text = $"{gm.GetStringByTable(cdb.passiveSkill.skillName)}\n{gm.GetStringByTable(cdb.passiveSkill.skillDescription)}";
        skillIcons[2].sprite = gm.GetSpriteByAddress($"{cdb.activeSkill.skillIcon}");
        skillInfo[2].text = $"{gm.GetStringByTable(cdb.activeSkill.skillName)}\n{gm.GetStringByTable(cdb.activeSkill.skillDescription)}";
    }

    private void SetInfoText(List<TextMeshProUGUI> infoTexts, List<string> buildTexts)
    {
        for (int i = 0; i < infoTexts.Count; i++)
        {
            infoTexts[i].text = buildTexts[i];
        }
    }
}