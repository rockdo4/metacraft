using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroInfoButton : MonoBehaviour
{
    public TextMeshProUGUI heroNameText;
    public TextMeshProUGUI gradeText;
    public TextMeshProUGUI jobText;
    public TextMeshProUGUI levelText;
    public Image portrait;

    public void SetData(HeroData data)
    {
        InfoHero info = data.info;
        heroNameText.text = info.name;
        gradeText.text = info.grade;
        jobText.text = info.job;
        levelText.text = info.level.ToString();
        portrait.sprite = GameManager.Instance.testPortraits[info.resourceAddress];
    }
}