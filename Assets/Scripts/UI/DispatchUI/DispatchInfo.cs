using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Data;

public class DispatchInfo : MonoBehaviour
{
    //before
    public GameObject before;
    public Image addRewardCharacterPortrait;
    public TextMeshProUGUI dispatchDescription;
    public TextMeshProUGUI beforeButton;
    //proceeding
    public GameObject proceeding;
    public Image turnPortrait;
    public TextMeshProUGUI turn;
    public Image[] heroPortarits;
    public TextMeshProUGUI rewardDescription;
    public TextMeshProUGUI proceedingButton;
 
    public int completeTime;
    public DateTime startTime;
    public Dictionary<string, object> dispatchInfo;

    //알림
    public AndroidNotificationManager androidNotificationManager;

    public void SetData(Dictionary<string, object> info)
    {
        dispatchInfo = info;
        completeTime = (int)dispatchInfo["Time"];
        addRewardCharacterPortrait.sprite = GameManager.Instance.GetSpriteByAddress($"Icon_{dispatchInfo["AddRewardCharaID"]}");
        dispatchDescription.text = $"{completeTime}\n{dispatchInfo["Name"]}\n{dispatchInfo["Text"]}";
        beforeButton.text = $"파견";
    }

    public void OnClickBeforeButton()
    {
        before.SetActive(false);
        proceeding.SetActive(true);
        startTime = DateTime.Now;
        //androidNotificationManager.OnClickedNotification(10, "Metacraft", "Dispatch is complete"); // 알람 비활성화
    }

    public void OnClickProceedingButton()
    {
        proceeding.SetActive(false);
        before.SetActive(true);
    }
}