using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    public Image portrait;
    public TextMeshProUGUI explanation;
    public TextMeshProUGUI ExpectedCost;
    public GameObject heroSlot1;
    public GameObject heroSlot2;
    public GameObject heroSlot3;
    public TextMeshProUGUI fitProperties1;
    public TextMeshProUGUI fitProperties2;
    public TextMeshProUGUI fitProperties3;
    public TextMeshProUGUI deductionAP;
    public TextMeshProUGUI ProperCombatPower;

    public GameObject heroSlectWindowPrefab;
    public GameObject missionPoints;
    //TEST¿ë
    private List<Dictionary<string, object>> missionInfoTable;
    private GameObject[] marks;
    private GameObject curButton;

    public delegate void clickmark(int num);

    public void Start()
    {
        missionInfoTable = GameManager.Instance.missionInfoList;
        var num = Utils.DistinctRandomNumbers(missionInfoTable.Count, 4);
        marks = missionPoints.GetComponentInChildren<MissionSpawner>().prefebs;
        int j = 0;
        for (int i = 0; i < marks.Length; i++)
        {
            if(marks[i].activeInHierarchy)
            {
                var index = j++;
                marks[i].GetComponentInChildren<TextMeshProUGUI>().text = $"{missionInfoTable[num[index]]["Name"]}";
                marks[i].GetComponentInChildren<Button>().onClick.AddListener(()=>UpdateMissionInfo(num[index]));
            } 
        }
     }

    public void UpdateMissionInfo(int num)
    {
        var dic = missionInfoTable[num];
        portrait.sprite = GameManager.Instance.iconSprites[$"Icon_{dic["BossID"]}"];
        explanation.text = $"{dic["OperationDescription"]}";
        ExpectedCost.text = $"{dic["ExpectedCostID"]}";
        fitProperties1.text = $"{dic["FitProperties1"]}";
        fitProperties2.text = $"{dic["FitProperties2"]}";
        fitProperties3.text = $"{dic["FitProperties3"]}";
        deductionAP.text = $"AP -{dic["ConsumptionBehavior"]}";
        ProperCombatPower.text = $"1000/{dic["ProperCombatPower"]}";
    }

    public void OnClickHeroSelect(LiveData data)
    {
        Logger.Debug(2);
        curButton.GetComponent<Image>().sprite = GameManager.Instance.iconSprites[$"Icon_{data.name}"];
    }

    public void ConfirmSelectButton(GameObject obj)
    {
        curButton = obj;
    }
}
