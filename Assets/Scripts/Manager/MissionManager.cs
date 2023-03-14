using Mono.Cecil;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : View
{
    public TextMeshProUGUI dayOfweek;
    // public Slider apGauge;

    public Image portrait;  //Boss portrait
    public TextMeshProUGUI explanation;  //

    public TextMeshProUGUI ExpectedCost;
    public GameObject[] heroSlots;
    private int heroSlotsIndex;
    public TextMeshProUGUI[] fitProperties;

    //public TextMeshProUGUI deductionAP;
    public TextMeshProUGUI ProperCombatPower;

    public Slider difficultyAdjustment;

    public GameObject missionPoints;
    //private GameObject[] marks;
    private List<Dictionary<string, object>> missionInfoTable;
    private GameObject[] marks;

    public List<GameObject> expectedRewards;

    [Range(1,5)]
    public int difficulty = 1;
    [Range(1,7)]
    public int markCount = 4;
    public delegate void clickmark(int num);
    //private int missionNum;
    private GameManager gm;

    private void OnEnable()
    {
        gm = GameManager.Instance;
        UpdateMissionDay();
    }

    private void Start()
    {
        missionInfoTable = gm.missionInfoList;

        heroSlotsIndex = 0;

            marks = GetComponentInChildren<MissionSpawner>().prefebs;
            var num = Utils.DistinctRandomNumbers(missionInfoTable.Count, markCount);
            int k = 0;
            for (int j = 0; j < marks.Length; j++)
            {
                if (marks[j].GetComponent<MissionMarkData>().isMarkOn)
                {
                    var index = k++;
                    marks[j].GetComponentInChildren<TextMeshProUGUI>().text = $"{missionInfoTable[num[index]]["Name"]}";
                    marks[j].GetComponentInChildren<Button>().onClick.AddListener(() => UpdateMissionInfo(num[index]));
                }
                else
                {
                    marks[j].SetActive(false);
                }
            }
    }

    public void UpdateMissionDay()
    {
        dayOfweek.text = $"{gm.playerData.currentDay}����";
    }

    public void UpdateMissionInfo(int num)
    {
        //missionNum = num;
        var dic = missionInfoTable[num];
        gm.currentSelectMission = dic;

        //portrait.sprite = gm.iconSprites[$"Icon_{dic["BossID"]}"];  �������̵� �� ���̺��� �ҷ��;���
        explanation.text = $"{dic["OperationDescription"]}";
        //ExpectedCost.text = $"{dic["ExpectedCostID"]}";  ��������
        gm.ClearBattleGroups();
        for (int i = 0; i < heroSlots.Length; i++)
        {
            heroSlots[i].GetComponent<Image>().sprite = null;
        }
        for (int i = 0; i < fitProperties.Length; i++)
        {
            var count = $"FitProperty{i + 1}";
            fitProperties[i].text = $"{dic[count]}";
            fitProperties[i].fontStyle = FontStyles.Normal;
            fitProperties[i].color = Color.white;
        }
        //deductionAP.text = $"AP -{dic["ConsumptionBehavior"]}";
        //ProperCombatPower.text = $"0/{dic["ProperCombatPower"]}";
        ProperCombatPower.color = Color.white;

        //���� ���̺� ���� �ʿ�
        //int erCount = expectedRewards.Count;
        //for (int i = 0; i < erCount; i++)
        //{
        //    RewardItem ri = expectedRewards[i].GetComponent<RewardItem>();
        //    if (i == 0)
        //        ri.SetData("���", $"{dic["Compensation"]}");
        //    else
        //        ri.SetData($"������{i}");
        //}
    }

    // Mission Hero Info Button ���� ȣ��
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
        //TotalPowerCheck(); ���� ����
    }

    // Hero Slot ���� Index ����
    public void ConfirmSelectButton(int idx)
    {
        heroSlotsIndex = idx;
    }

    // ���� �Ӽ� üũ
    private void PropertyMatchingCheck()
    {
        var selectedHeroes = gm.GetSelectedHeroes();

        for (int i = 0; i < fitProperties.Length; i++)
        {
            foreach (var hero in selectedHeroes)
            {
                if (hero != null)
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
    }

    //// ������ �հ� üũ  ���� ����
    //private void TotalPowerCheck()
    //{
    //    var selectedHeroes = gm.GetSelectedHeroes();

    //    int totalPower = 0;
    //    foreach (var hero in selectedHeroes)
    //    {
    //        if (hero != null)
    //            totalPower += hero.GetComponent<CharacterDataBundle>().data.Power;
    //    }
    //    var properCombatPower = gm.currentSelectMission["ProperCombatPower"];
    //    ProperCombatPower.text = $"{totalPower}/{properCombatPower}";
    //    ProperCombatPower.color = totalPower < (int)properCombatPower ? Color.red : Color.white;
    //}

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
            //gm.CreateBattleMap();
            switch (gm.currentSelectMission["Type"])
            {
                case 0:
                case 1:
                    gm.LoadScene((int)SceneIndex.Battle);
                    //gm.EnableBattleMap();
                    break;
                case 2:
                    gm.LoadScene((int)SceneIndex.Defense);
                    //gm.EnableBattleMap();
                    break;
            }
        }
    }

    public void OnAdjustmentDifficulty()
    {
        difficultyAdjustment.GetComponentInChildren<TextMeshProUGUI>().text = difficultyAdjustment.value.ToString();
    }
}