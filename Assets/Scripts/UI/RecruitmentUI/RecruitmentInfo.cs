using System.Collections.Generic;
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
        icon.sprite = GameManager.Instance.GetSpriteByAddress($"icon_{originData.name}");
    }

    public void SetStoneData(CharacterDataBundle dataBundle, Dictionary<string, object> stoneData)
    {
        //bundle = dataBundle;
        //originData = dataBundle.originData;
        heroNameText.text = GameManager.Instance.GetStringByTable(stoneData["Item_Name"].ToString());
        //gradeText.text = $"{(CharacterGrade)originData.grade}";
        icon.sprite = GameManager.Instance.GetSpriteByAddress(stoneData["Icon_Name"].ToString());
    }

    //아이템 연결
}
