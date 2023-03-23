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

    //Popup
    public Image resultHeroIcon;
    public TextMeshProUGUI resultHeroText;

    private GameManager gm;

    private void Start()
    {
        gm = GameManager.Instance;
    }

    private void OnEnable()
    {
        SetHeroInfo();
    }

    private void SetHeroInfo()
    {
        if (gm.currentSelectObject == null)
            return;
        CharacterDataBundle cdb = gm.currentSelectObject.GetComponent<AttackableUnit>().GetUnitData();
        LiveData data = cdb.data;

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

        resultHeroIcon.sprite = gm.GetSpriteByAddress($"icon_{data.name}");
        resultHeroText.text = $"승급 심사 통과를 축하합니다!\n{(CharacterGrade)(data.grade + 1)}등급 히어로로 승급하였습니다!";

    }

    private void FindUpgradeMaterial(string name, int grade)
    {
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

    public void OnClickUpgradeButton()
    {
        CharacterDataBundle cdb = gm.currentSelectObject.GetComponent<AttackableUnit>().GetUnitData();
        LiveData data = cdb.data;

        data.grade += 1;
        data.maxLevel = (int)gm.maxLevelTable[data.grade + 1]["MaxLevel"];
        cdb.attacks[0].skillLevel += 1;
        cdb.passiveSkill.skillLevel += 1;
        cdb.activeSkill.skillLevel += 1;

        //아이템 소모, 골드 소모
    }
}