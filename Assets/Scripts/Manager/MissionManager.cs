using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    public Image portrait;
    public TextMeshProUGUI explanation;

    public TextMeshProUGUI ExpectedCost;
    public GameObject[] heroSlots;
    private GameObject curSlot;
    public TextMeshProUGUI[] fitProperties;

    public TextMeshProUGUI deductionAP;
    public TextMeshProUGUI ProperCombatPower;

    public GameObject missionPoints;
    private GameObject[] marks;
    private List<Dictionary<string, object>> missionInfoTable;

    public delegate void clickmark(int num);

    public void Start()
    {
        missionInfoTable = GameManager.Instance.missionInfoList;
        var num = Utils.DistinctRandomNumbers(missionInfoTable.Count, 4);
        marks = GetComponentInChildren<MissionSpawner>().prefebs;

        curSlot = heroSlots[0];

        int j = 0;
        for (int i = 0; i < marks.Length; i++)
        {
            if(marks[i].GetComponent<MissionMarkData>().isMarkOn)
            {
                var index = j++;
                marks[i].GetComponentInChildren<TextMeshProUGUI>().text = $"{missionInfoTable[num[index]]["Name"]}";
                marks[i].GetComponentInChildren<Button>().onClick.AddListener(()=>UpdateMissionInfo(num[index]));
            }
            else
            {
                marks[i].SetActive(false);
            }
        }
     }

    public void UpdateMissionInfo(int num)
    {
        var dic = missionInfoTable[num];
        portrait.sprite = GameManager.Instance.iconSprites[$"Icon_{dic["BossID"]}"];
        explanation.text = $"{dic["OperationDescription"]}";
        ExpectedCost.text = $"{dic["ExpectedCostID"]}";
        for (int i = 0; i < fitProperties.Length; i++)
        {
            var count = $"FitProperties{i + 1}";
            fitProperties[i].text = $"{dic[count]}";
        }
        deductionAP.text = $"AP -{dic["ConsumptionBehavior"]}";
        ProperCombatPower.text = $"1000/{dic["ProperCombatPower"]}";
    }

    public void OnClickHeroSelect(CharacterDataBundle data)
    {
        curSlot.GetComponent<Image>().sprite = GameManager.Instance.iconSprites[$"Icon_{data.name}"];
    }

    public void ConfirmSelectButton(GameObject obj)
    {
        curSlot = obj;
    }
}
