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

    private void OnEnable()
    {
        SetOfficeDatas();
        DestroyAllIcons();
        CreateIcons();
        setPortraitText.text = GameManager.Instance.GetStringByTable("set_portrait_title");
    }

    public void SetPortrait(string spriteKey)
    {
        portrait.sprite = GameManager.Instance.GetSpriteByAddress(spriteKey);
    }

    private void SetOfficeDatas()
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