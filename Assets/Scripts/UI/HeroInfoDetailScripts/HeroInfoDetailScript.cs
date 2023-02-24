using TMPro;
using System.Text;
using UnityEngine.UI;
public class HeroInfoDetailScript : View
{
    //public CharacterData data;
    public Image portrait;
    public TextMeshProUGUI gradeInfoInLeftPanel;
    public TextMeshProUGUI levelInfoInfoInLeftPanel;
    public TextMeshProUGUI statDetail;

    public Button[] trainingPlusButtons;
    //private void Start()
    //{
    //    //gradeInfoInLeftPanel.text = data.grade;
    //    //levelInfoInfoInLeftPanel.text = data.level.ToString();

    //    //statDetail.text = SetHeroStatInfoText();
    //    //SetTrainingPlusButtons();
    //}

    private void OnEnable()
    {
        SetHeroStatInfoText();
    }

    private void SetHeroStatInfoText()
    {
        LiveData data = GameManager.Instance.selectDetail;
        string spriteKey = $"Illur_{data.name}";
        Logger.Debug(spriteKey);
        portrait.sprite = GameManager.Instance.GetSpriteByAddress(spriteKey);

        gradeInfoInLeftPanel.text = data.grade;
        levelInfoInfoInLeftPanel.text = data.level.ToString();
        StringBuilder stringBuilder = new();
        stringBuilder.Append($"����� �� : {data.name}\n");
        stringBuilder.Append($"���ݷ� : {data.baseDamage}\n");
        stringBuilder.Append($"���� : {data.baseDefense}\n");
        stringBuilder.Append($"ü�� : {data.healthPoint}\n");
        stringBuilder.Append($"Ÿ�� : {data.job}\n");
        stringBuilder.Append($"Ȱ���� �Ҹ� : {data.energy}\n");
        stringBuilder.Append($"ġ��Ÿ Ȯ�� : {data.critical}\n");
        stringBuilder.Append($"ġ��Ÿ ���� : {data.criticalDmg}\n");
        stringBuilder.Append($"�̵� �ӵ� : {data.moveSpeed}\n");
        stringBuilder.Append($"���߷� : {data.accuracy}\n");
        stringBuilder.Append($"ȸ���� : {data.evasion}\n");
        statDetail.text = stringBuilder.ToString();
    }

    //private void SetTrainingPlusButtons()
    //{
        //int testStat = 10;
        //trainingPlusButtons[0].onClick.AddListener(()=>data.baseDamage += testStat);
        //trainingPlusButtons[1].onClick.AddListener(()=>data.baseDefense += testStat);
        //trainingPlusButtons[2].onClick.AddListener(()=>data.healthPoint += testStat);
    //}
}