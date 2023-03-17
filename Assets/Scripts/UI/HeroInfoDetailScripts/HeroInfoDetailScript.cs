using TMPro;
using System.Text;
using UnityEngine.UI;
public class HeroInfoDetailScript : View
{
    public Image portrait;
    public TextMeshProUGUI gradeInfoInLeftPanel;
    public TextMeshProUGUI levelInfoInfoInLeftPanel;
    public TextMeshProUGUI statDetail;

    public Button[] trainingPlusButtons;

    private void OnEnable()
    {
        SetHeroStatInfoText();
    }

    private void SetHeroStatInfoText()
    {
        GameManager gm = GameManager.Instance;
        LiveData data = gm.currentSelectObject.GetComponent<AttackableUnit>().GetUnitData().data;
        portrait.sprite = gm.GetSpriteByAddress($"Illu_{data.name}");

        gradeInfoInLeftPanel.text = $"{(CharacterGrade)data.grade}";
        levelInfoInfoInLeftPanel.text = data.level.ToString();
        StringBuilder stringBuilder = new();
        stringBuilder.Append($"����� �� : {gm.GetStringByTable(data.name)}\n");
        stringBuilder.Append($"���ݷ� : {data.baseDamage}\n");
        stringBuilder.Append($"���� : {data.baseDefense}\n");
        stringBuilder.Append($"ü�� : {data.healthPoint}\n");
        stringBuilder.Append($"Ÿ�� : {GameManager.Instance.GetStringByTable($"herotype_{(CharacterJob)data.job}")}\n");
        stringBuilder.Append($"ġ��Ÿ Ȯ�� : {data.critical * 100:.0}%\n");
        stringBuilder.Append($"ġ��Ÿ ���� : {data.criticalDmg * 100:00.0}%\n");
        stringBuilder.Append($"�̵� �ӵ� : {data.moveSpeed}\n");
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