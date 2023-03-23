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
    public Image[] preSkillIcons;
    public TextMeshProUGUI[] previousSkillInfo;
    public Image[] nextSkillIcons;
    public TextMeshProUGUI[] nextSkillInfo;

    //bottom
    public Image[] meterialIcons;
    public TextMeshProUGUI[] meterialInfo;
    public TextMeshProUGUI gold;
    public Button upgradeButton;

    private void SetHeroInfo()
    {
        GameManager gm = GameManager.Instance;

        if (gm.currentSelectObject == null)
            return;
        CharacterDataBundle cdb = gm.currentSelectObject.GetComponent<AttackableUnit>().GetUnitData();
        LiveData data = cdb.data;
        var maxLevelTable = gm.maxLevelTable;

        heroName.text = data.name;
        heroGrade.text = $"{(CharacterGrade)data.grade}등급 > {(CharacterGrade)(data.grade + 1)}등급";
        heroMaxLevel.text = $"{gm.maxLevelTable[data.grade]} > {gm.maxLevelTable[data.grade + 1]}";
        heroPortrait.sprite = gm.GetSpriteByAddress($"Icon_{data.name}");
        foreach(var preskillIcon in preSkillIcons)
        {
            preskillIcon.sprite = gm.GetSpriteByAddress($"Icon_{data.name}");
        }
    }
}