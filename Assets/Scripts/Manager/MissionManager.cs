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

    public void Start()
    {
        missionInfoTable = GameManager.Instance.missionInfoList;
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
        portrait.sprite = GameManager.Instance.iconSprites[$"Icon_{dic["BossID"]}"];
        explanation.text = $"{dic["OperationDescription"]}";
        ExpectedCost.text = $"{dic["ExpectedCostID"]}";
        GameManager.Instance.ClearBattleGroups();
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

    // Mission Hero Info Button ���� ȣ��
    public void OnClickHeroSelect(CharacterDataBundle bundle)
    {
        int index = GameManager.Instance.GetHeroIndex(bundle.gameObject);
        var selectIndexGroup = GameManager.Instance.battleGroups;

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
        heroSlots[heroSlotsIndex].GetComponent<Image>().sprite = GameManager.Instance.GetSpriteByAddress($"Icon_{liveData.name}");
        selectIndexGroup[heroSlotsIndex] = index;

        if (duplication != null)
        {
            heroSlots[(int)duplication].GetComponent<Image>().sprite = null;
            selectIndexGroup[(int)duplication] = null;
        }

        PropertyMatchingCheck();
        TotalPowerCheck();
    }

    // Hero Slot ���� Index ����
    public void ConfirmSelectButton(int idx)
    {
        heroSlotsIndex = idx;
    }

    //���� �Ӽ� üũ
    private void PropertyMatchingCheck()
    {
        GameManager gm = GameManager.Instance;
        var myHeroes = gm.myHeroes;

        for (int i = 0; i < fitProperties.Length; i++)
        {
            foreach (int? idx in GameManager.Instance.battleGroups)
            {
                if (idx == null)
                    continue;
                if (myHeroes[(int)idx].GetComponent<CharacterDataBundle>().data.job.Equals(fitProperties[i].text))
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

    //������ �հ� üũ
    private void TotalPowerCheck()
    {
        GameManager gm = GameManager.Instance;
        var myHeroes = gm.myHeroes;
        int totalPower = 0;
        foreach (int? idx in GameManager.Instance.battleGroups)
        {
            if (idx == null)
                continue;
            totalPower += myHeroes[(int)idx].GetComponent<CharacterDataBundle>().data.Power;
        }
        var properCombatPower = missionInfoTable[missionNum]["ProperCombatPower"];
        ProperCombatPower.text = $"{totalPower}/{properCombatPower}";
        if(totalPower< (int)properCombatPower)
        {
            ProperCombatPower.color = Color.red;
        }
        else
        {
            ProperCombatPower.color = Color.white;
        }
    }

    public void StartMission()
    {
        GameManager gm = GameManager.Instance;
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

    private void OnEnable()
    {
        GameManager gm = GameManager.Instance;
        int index = 0;
        foreach (var num in gm.battleGroups)
        {
            if (num == null)
                heroSlots[index].GetComponent<Image>().sprite = null;
            index++;
        }
    }
}
