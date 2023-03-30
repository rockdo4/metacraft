using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroInfoButton : MonoBehaviour
{
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
        gradeText.text = $"{(CharacterGrade)data.grade}";
        jobText.text = GameManager.Instance.GetStringByTable($"herotype_{(CharacterJob)data.job}");
        levelText.text = $"{data.level}";
        icon.sprite = GameManager.Instance.GetSpriteByAddress($"icon_{data.name}");
    }   

    public virtual void OnClick()
    {
        GameManager.Instance.currentSelectObject = bundle.gameObject;
        //if (GameManager.Instance.playerData.isTutorial)
        //{
        //    var tutoMgr = FindObjectOfType<TutorialManager>();

        //    //if (tutoMgr != null)
        //    //    tutoMgr.OnNextChat();
        //}
    }
}