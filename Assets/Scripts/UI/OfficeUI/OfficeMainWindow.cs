using TMPro;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class OfficeMainWindow : View
{
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI officeLevel;
    public TextMeshProUGUI officeExp;
    public TextMeshProUGUI playerGold;
    public Image expFillImage;

    private void OnEnable()
    {
        SetOfficeDatas();
    }

    public void SetOfficeDatas()
    {
        GameManager gm = GameManager.Instance;
        PlayerData pd = gm.playerData;
        playerName.text = pd.playerName;
        int curLevel = pd.officeLevel;
        int needExp = (int)gm.officeInfoList[curLevel - 1]["NeedExp"];
        float expRatio = (float)pd.officeExperience / needExp;
        officeLevel.text = $"{curLevel}";
        officeExp.text = $"{expRatio * 100:0.00}%";
        expFillImage.fillAmount = expRatio;
        playerGold.text = $"{pd.gold}";
    }
}