using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OfficeMainWindow : View
{
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI officeLevel;
    public TextMeshProUGUI officeExp;
    public TextMeshProUGUI playerGold;
    public Image expFillImage;
    public Transform playerIconsTransform;
    public GameObject iconPrefab;
    public TextMeshProUGUI setPortraitText;
    public Image portrait;
    private List<GameObject> icons = new ();
    private GameManager gm;

    private void Awake()
    {
        if (gm == null)
            gm = GameManager.Instance;
    }

    private void OnEnable()
    {
        if (gm == null)
            gm = GameManager.Instance;

        SetPortrait(gm.playerData.portraitKey);
        SetOfficeDatas();
        DestroyAllIcons();
        CreateIcons();
        setPortraitText.text = gm.GetStringByTable("set_portrait_title");
    }

    public void SetPortrait(string spriteKey)
    {
        portrait.sprite = gm.GetSpriteByAddress(spriteKey);
    }

    private void SetOfficeDatas()
    {
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

    private void CreateIcons()
    {
        GameManager gm = GameManager.Instance;
        Dictionary<string, GameObject> myHeroes = gm.myHeroes;

        foreach (var hero in myHeroes)
        {
            GameObject iconObject = Instantiate(iconPrefab, playerIconsTransform);
            PlayerPortrait pp = iconObject.GetComponent<PlayerPortrait>();
            string iconKey = $"icon_{hero.Value.GetComponent<CharacterDataBundle>().originData.name}";
            pp.iconButton.onClick.AddListener(() => pp.SetPortraitImage(portrait, iconKey));
            pp.iconButton.onClick.AddListener(() => UIManager.Instance.ClearPopups());
            pp.SetSprite(iconKey);
            icons.Add(iconObject);
        }
    }

    private void DestroyAllIcons()
    {
        int count = icons.Count;
        for (int i = 0; i < count; i++)
            Destroy(icons[i]);

        icons.Clear();
    }
}