using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecruitmentInfo : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI gradeText;
    public TextMeshProUGUI heroNameText;
    public CharacterData originData;
    protected CharacterDataBundle bundle;

    public virtual void SetData(CharacterDataBundle dataBundle)
    {
        bundle = dataBundle;
        originData = dataBundle.originData;
        heroNameText.text = GameManager.Instance.GetStringByTable(originData.name);
        gradeText.text = $"{(CharacterGrade)originData.grade}";
        icon.sprite = GameManager.Instance.GetSpriteByAddress($"Icon_{originData.name}");
    }

    //아이템 연결
}
