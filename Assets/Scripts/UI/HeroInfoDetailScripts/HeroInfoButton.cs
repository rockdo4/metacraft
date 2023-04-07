using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroInfoButton : MonoBehaviour
{
    public TextMeshProUGUI gradeText;    
    public TextMeshProUGUI levelText;
    public Image typeIcon;
    public Image icon;
    public TypeColor colorInfo;
    public LiveData data;
    protected CharacterDataBundle bundle;

    public virtual void SetData(CharacterDataBundle dataBundle)
    {
        bundle = dataBundle;
        data = dataBundle.data;
        gradeText.text = $"{(CharacterGrade)data.grade}";
        typeIcon.sprite = GameManager.Instance.GetSpriteByAddress($"icon_type_{(CharacterJob)data.job}");
        typeIcon.color = colorInfo.GetTypeColor((CharacterJob)data.job);
        levelText.text = $"{data.level}";
        icon.sprite = GameManager.Instance.GetSpriteByAddress($"icon_{data.name}");
    }   

    public virtual void OnClick()
    {
        GameManager.Instance.currentSelectObject = bundle.gameObject;
    }
}