using TMPro;
using UnityEngine.UI;
using UnityEngine;

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
    //public Image[] nextSkillIcons;
    //public TextMeshProUGUI[] nextSkillInfo;

    //bottom
    public Image[] meterialIcons;
    public TextMeshProUGUI[] meterialInfo;
    public TextMeshProUGUI gold;
    public Button upgradeButton;

    //Popup
    public Image resultPopup;
    public Image resultHeroIcon;
    public TextMeshProUGUI resultHeroText;

    private GameManager gm;
    private int needGold = 0;
    private int needItemCount = 0;
    private int upgradeItemCount = 0;
    private string currItemID = string.Empty;

    private void Awake()
    {
        gm = GameManager.Instance;
    }

    private void OnEnable()
    {
        SetHeroInfo();
        ShowOutResultPopup();
    }

    private void SetHeroInfo()
    {
        if (gm.currentSelectObject == null)
            return;
        CharacterDataBundle cdb = gm.currentSelectObject.GetComponent<AttackableUnit>().GetUnitData();
        LiveData data = cdb.data;

        Sprite heroIcon = gm.GetSpriteByAddress($"icon_{data.name}");
        string gradeText = string.Empty;

        heroName.text = gm.GetStringByTable(data.name);
        CharacterGrade curGrade = (CharacterGrade)data.grade;

        if (curGrade != CharacterGrade.SS)
        {
            heroGrade.text = $"{curGrade}등급 > {curGrade + 1}등급";
            heroMaxLevel.text =
                $"{gm.maxLevelTable[data.grade - 1]["MaxLevel"]} >" +
                $"{gm.maxLevelTable[data.grade]["MaxLevel"]}";

            gradeText = $"승급 심사 통과를 축하합니다!\n{(CharacterGrade)(data.grade)}등급 히어로로 승급하였습니다!";
        }
        else
        {
            heroGrade.text = $"{curGrade}등급";
            heroMaxLevel.text =
                $"{gm.maxLevelTable[data.grade - 1]["MaxLevel"]}";

            gradeText = $"승급 심사 통과를 축하합니다!\n{(CharacterGrade)(data.grade)}등급 히어로로 승급하였습니다!";
            //gradeText = $"이미 {(CharacterGrade)(data.grade)}등급입니다";
        }

        SetResultInfo(heroIcon, gradeText);
        if (curGrade == CharacterGrade.SS)
        {
            return;
        }

        heroPortrait.sprite = gm.GetSpriteByAddress($"icon_{data.name}");
        prevSkillIcons[0].sprite = gm.GetSpriteByAddress($"{cdb.passiveSkill.skillIcon}");
        prevSkillIcons[1].sprite = gm.GetSpriteByAddress($"{cdb.attacks[0].skillIcon}");
        prevSkillIcons[2].sprite = gm.GetSpriteByAddress($"{cdb.activeSkill.skillIcon}");
        prevSkillInfo[0].text = gm.GetStringByTable($"{cdb.passiveSkill.skillDescription}");
        prevSkillInfo[1].text = gm.GetStringByTable($"{cdb.attacks[0].skillDescription}");
        prevSkillInfo[2].text = gm.GetStringByTable($"{cdb.activeSkill.skillDescription}");

        //nextSkillIcons[0].sprite = gm.GetSpriteByAddress($"{cdb.passiveSkill.skillIcon}");
        //nextSkillIcons[1].sprite = gm.GetSpriteByAddress($"{cdb.attacks[0].skillIcon}");
        //nextSkillIcons[2].sprite = gm.GetSpriteByAddress($"{cdb.activeSkill.skillIcon}");
        //nextSkillInfo[0].text = gm.GetStringByTable($"{cdb.passiveSkill.skillDescription}");
        //nextSkillInfo[1].text = gm.GetStringByTable($"{cdb.attacks[0].skillDescription}");
        //nextSkillInfo[2].text = gm.GetStringByTable($"{cdb.activeSkill.skillDescription}");

        FindUpgradeMaterial(data.name, data.grade);
    }

    private void FindUpgradeMaterial(string name, int grade)
    {
        var ut = gm.upgradeTable;
        var inven = gm.inventoryData;
        var items = gm.itemInfoList;
        int mmaterialAmount;
        int itemCount;

        for (int i = 0; i < ut.Count; i++)
        {
            if (ut[i]["Name"].ToString().Equals(name) && ((int)ut[i]["Grade"]).Equals(grade))
            {
                var itemID = ut[i]["Material1"].ToString();
                for (int j = 0; j < items.Count; j++)
                {
                    if (items[j]["ID"].ToString().Equals(itemID))
                    {
                        meterialIcons[0].sprite = gm.GetSpriteByAddress(items[j]["Icon_Name"].ToString());
                        break;
                    }
                }

                mmaterialAmount = (int)ut[i]["Mmaterial1Amount"];
                if (inven.IsItem(itemID))
                {
                    currItemID = itemID;
                    itemCount = inven.FindItem(itemID).count;
                    meterialInfo[0].text = $"{itemCount}/{mmaterialAmount}";
                }
                else
                {
                    meterialInfo[0].text = $"{0}/{mmaterialAmount}";
                    itemCount = 0;
                }

                upgradeItemCount = itemCount;
                needItemCount = mmaterialAmount;
                needGold = (int)ut[i]["NeedGold"];
                gold.text = $"{gm.playerData.gold}/{needGold}";
            }            
        }
    }

    public void OnClickUpgradeButton()
    {
        CharacterDataBundle cdb = gm.currentSelectObject.GetComponent<AttackableUnit>().GetUnitData();
        LiveData data = cdb.data;

        int currGold = gm.playerData.gold;
        if (currGold < needGold ||
            upgradeItemCount < needItemCount)
        {
            resultHeroText.text = "재료가 부족합니다.";
            ShowOnResultPopup();
            return;
        }

        if (data.grade == (int)CharacterGrade.SS)
        return;

        data.maxLevel = (int)gm.maxLevelTable[data.grade]["MaxLevel"];
        data.grade += 1;
        cdb.attacks[0].skillLevel += 1;
        cdb.passiveSkill.skillLevel += 1;
        cdb.activeSkill.skillLevel += 1;

        //아이템 소모, 골드 소모
        gm.playerData.gold -= needGold;        
        var inven = gm.inventoryData;
        inven.UseItem(currItemID, needItemCount);

        ShowOnResultPopup();
        SetHeroInfo();
        FindUpgradeMaterial(data.name, data.grade);
    }

    private void ShowOnResultPopup()
    {
        resultPopup.gameObject.SetActive(true);
    }
    public void ShowOutResultPopup()
    {
        resultPopup.gameObject.SetActive(false);
    }

    private void SetResultInfo(Sprite heroSprite, string grade)
    {
        resultHeroIcon.sprite = heroSprite;
        resultHeroText.text = grade;
    }
}