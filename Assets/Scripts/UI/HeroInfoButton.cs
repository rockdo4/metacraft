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
        gradeText.text = data.grade;
        jobText.text = data.job;
        levelText.text = data.level.ToString();
        icon.sprite = GameManager.Instance.iconSprites[$"Icon_{data.name}"];
    }

    public void SelectCharacter()
    {
        GameManager.Instance.selectObject = bundle.gameObject;
    }
}