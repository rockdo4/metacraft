using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DispatchInfo : MonoBehaviour
{
    //before
    public Image addRewardCharacterPortrait;
    public TextMeshProUGUI dispatchDescription;
    public TextMeshProUGUI beforeButton;
    //proceeding
    public Image turnPortrait;
    public TextMeshProUGUI turn;
    public Image[] heroPortarits;
    public TextMeshProUGUI rewardDescription;
    public TextMeshProUGUI proceedingButton;

    public void SetData(Dictionary<string, object> dispatchInfo)
    {
        addRewardCharacterPortrait.sprite = GameManager.Instance.iconSprites[$"Icon_{dispatchInfo["AddRewardCharaID"]}"];
        dispatchDescription.text = $"{dispatchInfo["Time"]}\n{dispatchInfo["Name"]}\n{dispatchInfo["Text"]}";
        beforeButton.text = $"ÆÄ°ß";
    }

    //public override void OnClick()
    //{
    //    base.OnClick();
    //    MissionManager mm = FindObjectOfType<MissionManager>();
    //    mm.OnClickHeroSelect(bundle);
    //}
}