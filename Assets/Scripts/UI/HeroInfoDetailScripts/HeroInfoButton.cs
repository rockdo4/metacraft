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
    public LiveData data;
    protected CharacterDataBundle bundle;

    public virtual void SetData(CharacterDataBundle dataBundle)
    {
        bundle = dataBundle;
        data = dataBundle.data;
        heroNameText.text = data.name;
        gradeText.text = $"{(CharacterGrade)data.grade}";
        jobText.text = $"{(CharacterJob)data.job}";
        levelText.text = $"{data.level}";
        icon.sprite = GameManager.Instance.iconSprites[$"Icon_{data.name}"];
    }

    public virtual void OnClick()
    {
        GameManager.Instance.currentSelectObject = bundle.gameObject;
    }
}