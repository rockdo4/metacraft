using TMPro;

public class OfficeMainWindow : View
{
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI officeLevel;
    public TextMeshProUGUI officeExp;
    public TextMeshProUGUI playerGold;
    public TextMeshProUGUI currentTurn;

    private void OnEnable()
    {
        SetOfficeDatas();
    }

    public void SetOfficeDatas()
    {
        GameManager gm = GameManager.Instance;
        PlayerData pd = gm.playerData;
        playerName.text = pd.playerName;
        officeLevel.text = pd.officeLevel.ToString();
        officeExp.text = pd.officeExperience.ToString();
        playerGold.text = pd.gold.ToString();
        currentTurn.text = $"ÇöÀç ÅÏ: {pd.currentGameDay}";
    }
}