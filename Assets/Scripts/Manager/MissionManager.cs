using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Rendering.Universal.ShaderGUI.LitGUI;

public class MissionManager : MonoBehaviour
{
    public Image portrait;
    public TextMeshProUGUI explanation;

    public TextMeshProUGUI ExpectedCost;
    public GameObject[] heroSlots;
    private MissionHeroSlotButton[] heroSlotDatas;
    private GameObject curSlot;
    private MissionHeroSlotButton curSlotData;
    public TextMeshProUGUI[] fitProperties;

    public TextMeshProUGUI deductionAP;
    public TextMeshProUGUI ProperCombatPower;

    public GameObject heroSlectWindowPrefab;
    public GameObject missionPoints;
    private GameObject[] marks;
    //TEST용
    private List<Dictionary<string, object>> missionInfoTable;

    public delegate void clickmark(int num);

    public void Start()
    {
        missionInfoTable = GameManager.Instance.missionInfoList;
        var num = Utils.DistinctRandomNumbers(missionInfoTable.Count, 4);
        marks = missionPoints.GetComponentInChildren<MissionSpawner>().prefebs;

        heroSlotDatas = new MissionHeroSlotButton[3];
        for (int i = 0; i < heroSlots.Length; i++)
        {
            heroSlotDatas[i] = heroSlots[i].GetComponent<MissionHeroSlotButton>();

        }
        curSlot = heroSlots[1]; // 이거 왜 1로 넣어둠?
        curSlotData = curSlot.GetComponent<MissionHeroSlotButton>();

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
        for (int i = 0; i < fitProperties.Length; i++)
        {
            var count = $"FitProperties{i + 1}";
            fitProperties[i].text = $"{dic[count]}";
        }
        deductionAP.text = $"AP -{dic["ConsumptionBehavior"]}";
        ProperCombatPower.text = $"1000/{dic["ProperCombatPower"]}";
    }

    public void OnClickHeroSelect(LiveData data)
    {
        //중복제거코드
        //foreach (var slot in heroSlotDatas)
        //{
        //    Logger.Debug($"{slot.data.name} {curSlotData.data.name}");
        //    //if(slot.data.name==curSlotData.data.name)
        //    {
        //    //    slot.data = null;
        //    }
        //}

        curSlot.GetComponent<Image>().sprite = GameManager.Instance.iconSprites[$"Icon_{data.name}"];
        curSlot.GetComponent<MissionHeroSlotButton>().data = data;
    }

    public void ConfirmSelectButton(GameObject obj)
    {
        curSlot = obj;
    }
}
