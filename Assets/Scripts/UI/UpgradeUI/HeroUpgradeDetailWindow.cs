using TMPro;
using UnityEngine.UI;

public class HeroUpgradeDetailWindow : View
{
    //top
    public TextMeshProUGUI heroName;
    public TextMeshProUGUI heroGrade;
    public TextMeshProUGUI heroMaxLevel;
    public Image heroPortrait;

    //mid
    public Image[] prevSkillIcons;
    public TextMeshProUGUI[] prevSkillInfo;
    public Image[] nextSkillIcons;
    public TextMeshProUGUI[] nextSkillInfo;

    //bottom
    public Image[] meterialIcons;
    public TextMeshProUGUI[] meterialInfo;
    public TextMeshProUGUI gold;
    public Button upgradeButton;

    private void OnEnable()
    {
        SetHeroInfo();
    }

    private void SetHeroInfo()
    {
        GameManager gm = GameManager.Instance;

        if (gm.currentSelectObject == null)
            return;
        CharacterDataBundle cdb = gm.currentSelectObject.GetComponent<AttackableUnit>().GetUnitData();
        LiveData data = cdb.data;
        var maxLevelTable = gm.maxLevelTable;

        heroName.text = gm.GetStringByTable(data.name);
        heroGrade.text = $"{(CharacterGrade)data.grade}등급 > {(CharacterGrade)(data.grade + 1)}등급";
        heroMaxLevel.text = $"{gm.maxLevelTable[data.grade]["MaxLevel"]} > {gm.maxLevelTable[data.grade + 1]["MaxLevel"]}";
        heroPortrait.sprite = gm.GetSpriteByAddress($"icon_{data.name}");
        prevSkillIcons[0].sprite = gm.GetSpriteByAddress($"{cdb.passiveSkill.skillIcon}");
        prevSkillIcons[1].sprite = gm.GetSpriteByAddress($"{cdb.attacks[0].skillIcon}");
        prevSkillIcons[2].sprite = gm.GetSpriteByAddress($"{cdb.activeSkill.skillIcon}");
        prevSkillInfo[0].text = gm.GetStringByTable($"{cdb.passiveSkill.skillDescription}");
        prevSkillInfo[1].text = gm.GetStringByTable($"{cdb.attacks[0].skillDescription}");
        prevSkillInfo[2].text = gm.GetStringByTable($"{cdb.activeSkill.skillDescription}");

        nextSkillIcons[0].sprite = gm.GetSpriteByAddress($"{cdb.passiveSkill.skillIcon}");
        nextSkillIcons[1].sprite = gm.GetSpriteByAddress($"{cdb.attacks[0].skillIcon}");
        nextSkillIcons[2].sprite = gm.GetSpriteByAddress($"{cdb.activeSkill.skillIcon}");
        nextSkillInfo[0].text = gm.GetStringByTable($"{cdb.passiveSkill.skillDescription}");
        nextSkillInfo[1].text = gm.GetStringByTable($"{cdb.attacks[0].skillDescription}");
        nextSkillInfo[2].text = gm.GetStringByTable($"{cdb.activeSkill.skillDescription}");

        FindUpgradeMaterial(data.name, data.grade);
        
    }

    private void FindUpgradeMaterial(string name, int grade)
    {
        GameManager gm = GameManager.Instance;

        for (int i = 0; i < gm.upgradeTable.Count; i++)
        {
            if (gm.upgradeTable[i]["Name"].ToString() == name&&(int)gm.upgradeTable[i]["Grade"] == grade)
            {
                //meterialIcons[0] = gm.inventoryData.FindItem(gm.upgradeTable[i]["Material1"])
                meterialInfo[0].text = $"{0}/{gm.upgradeTable[i]["Mmaterial1Amount"]}";
                gold.text = $"{gm.playerData.gold}/{gm.upgradeTable[i]["NeedGold"]}";
            }
        }
    }
}