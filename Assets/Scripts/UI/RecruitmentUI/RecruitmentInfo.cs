using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecruitmentInfo : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI gradeText;
    public TextMeshProUGUI heroNameText;
    public LiveData data;
    protected CharacterDataBundle bundle;

    public virtual void SetData(CharacterDataBundle dataBundle)
    {
        bundle = dataBundle;
        data = dataBundle.data;
        heroNameText.text = GameManager.Instance.GetStringByTable(data.name);
        gradeText.text = $"{(CharacterGrade)data.grade}";
        icon.sprite = GameManager.Instance.GetSpriteByAddress($"Icon_{data.name}");
    }
}
