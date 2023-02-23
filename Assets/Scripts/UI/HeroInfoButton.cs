using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroInfoButton : MonoBehaviour
{
    public TextMeshProUGUI heroNameText;
    public TextMeshProUGUI gradeText;
    public TextMeshProUGUI jobText;
    public TextMeshProUGUI levelText;
    public Image icon;

    public virtual void SetData(CharacterDataBundle data)
    {
        var info = data.data;
        heroNameText.text = info.name;
        gradeText.text = info.grade;
        jobText.text = info.job;
        levelText.text = info.level.ToString();
        icon.sprite = GameManager.Instance.iconSprites[$"Icon_{info.name}"];
    }
}