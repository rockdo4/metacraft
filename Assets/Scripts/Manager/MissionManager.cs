using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    public Image portrait;
    public TextMeshProUGUI explanation;

    public TextMeshProUGUI ExpectedCost;
    public GameObject[] heroSlots;
    private int heroSlotsIndex;
    public TextMeshProUGUI[] fitProperties;

    public TextMeshProUGUI deductionAP;
    public TextMeshProUGUI ProperCombatPower;

    public GameObject missionPoints;
    private GameObject[] marks;
    private List<Dictionary<string, object>> missionInfoTable;

    public delegate void clickmark(int num);
    private int missionNum;
    private GameManager gm = GameManager.Instance;

    public void Start()
    {
        missionInfoTable = gm.missionInfoList;
        var num = Utils.DistinctRandomNumbers(missionInfoTable.Count, 4);
        marks = GetComponentInChildren<MissionSpawner>().prefebs;

        heroSlotsIndex = 0;

        int j = 0;
        for (int i = 0; i < marks.Length; i++)
        {
            if (marks[i].GetComponent<MissionMarkData>().isMarkOn)
            {
                var index = j++;
                marks[i].GetComponentInChildren<TextMeshProUGUI>().text = $"{missionInfoTable[num[index]]["Name"]}";
                marks[i].GetComponentInChildren<Button>().onClick.AddListener(() => UpdateMissionInfo(num[index]));
            }
            else
            {
                marks[i].SetActive(false);
            }
        }
    }

    public void UpdateMissionInfo(int num)
    {
        missionNum = num;
        var dic = missionInfoTable[num];
        portrait.sprite = gm.iconSprites[$"Icon_{dic["BossID"]}"];
        explanation.text = $"{dic["OperationDescription"]}";
        ExpectedCost.text = $"{dic["ExpectedCostID"]}";
        gm.ClearBattleGroups();
        for (int i = 0; i < heroSlots.Length; i++)
        {
            heroSlots[i].GetComponent<Image>().sprite = null;
        }
        for (int i = 0; i < fitProperties.Length; i++)
        {
            var count = $"FitProperties{i + 1}";
            fitProperties[i].text = $"{dic[count]}";
            fitProperties[i].fontStyle = FontStyles.Normal;
            fitProperties[i].color = Color.white;
        }
        deductionAP.text = $"AP -{dic["ConsumptionBehavior"]}";
        ProperCombatPower.text = $"0/{dic["ProperCombatPower"]}";
        ProperCombatPower.color = Color.white;
    }

    // Mission Hero Info Button 에서 호출
    public void OnClickHeroSelect(CharacterDataBundle bundle)
    {
        int index = gm.GetHeroIndex(bundle.gameObject);
        var selectIndexGroup = gm.battleGroups;

        int? duplication = null;
        for (int i = 0; i < 3; i++)
        {
            if (selectIndexGroup[i] == index)
            {
                duplication = i;
                break;
            }
        }

        LiveData liveData = bundle.data;
        heroSlots[heroSlotsIndex].GetComponent<Image>().sprite = gm.GetSpriteByAddress($"Icon_{liveData.name}");
        selectIndexGroup[heroSlotsIndex] = index;

        if (duplication != null)
        {
            heroSlots[(int)duplication].GetComponent<Image>().sprite = null;
            selectIndexGroup[(int)duplication] = null;
        }

        PropertyMatchingCheck();
        TotalPowerCheck();
    }

    // Hero Slot 에서 Index 전달
    public void ConfirmSelectButton(int idx)
    {
        heroSlotsIndex = idx;
    }

    //적합 속성 체크
    private void PropertyMatchingCheck()
    {
        var selectedHeroes = gm.GetSelectedHeroes();

        for (int i = 0; i < fitProperties.Length; i++)
        {
            foreach (var hero in selectedHeroes)
            {
                if (hero.GetComponent<CharacterDataBundle>().data.job.Equals(fitProperties[i].text))
                {
                    fitProperties[i].fontStyle = FontStyles.Bold;
                    fitProperties[i].color = Color.red;
                    break;
                }
                fitProperties[i].fontStyle = FontStyles.Normal;
                fitProperties[i].color = Color.white;
            }
        }
    }

    //전투력 합계 체크
    private void TotalPowerCheck()
    {
        var selectedHeroes = gm.GetSelectedHeroes();

        int totalPower = 0;
        foreach (var hero in selectedHeroes)
        {
            totalPower += hero.GetComponent<CharacterDataBundle>().data.Power;
        }
        var properCombatPower = missionInfoTable[missionNum]["ProperCombatPower"];
        ProperCombatPower.text = $"{totalPower}/{properCombatPower}";
        ProperCombatPower.color = totalPower < (int)properCombatPower ? Color.red : Color.white;
    }

    public void StartMission()
    {
        int count = 0;
        foreach (var num in gm.battleGroups)
        {
            if (num != null)
                count++;
        }
        if (count > 0)
        {
            gm.LoadScene((int)SceneIndex.Battle);
        }
    }
}