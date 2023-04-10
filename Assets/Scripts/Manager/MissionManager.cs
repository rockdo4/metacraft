using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : View
{
    public Image portrait;  //Boss portrait
    public TextMeshProUGUI missionNameText;
    public TextMeshProUGUI explanation;  // Mission explanation

    public TextMeshProUGUI ExpectedCost;
    public Image[] heroSlots;
    private int heroSlotsIndex;
    public TextMeshProUGUI[] fitProperties;

    public Slider difficultyAdjustment;

    public GameObject missionPoints;
    private Dictionary<int, List<Dictionary<string, object>>> missionInfoTable;
    private GameObject[] marks;

    public List<GameObject> expectedRewards;

    [Range(1, 5)]
    public int difficulty = 1;
    [Range(1, 7)]
    public int markCount = 3;
    public delegate void clickmark(int num);

    public List<List<int>> nums;

    private GameManager gm;

    private void Awake()
    {
        gm = GameManager.Instance;
        marks = GetComponentInChildren<MissionSpawner>().prefebs;
    }

    private void Start()
    {
        missionInfoTable = gm.missionInfoDifficulty;
        heroSlotsIndex = 0;
        if(gm.playerData.isTutorial)
        {
            difficultyAdjustment.interactable = false;
        }
        else
        {
            difficultyAdjustment.interactable = true;
        }

        nums = new List<List<int>>();
        for (int i = 0; i < 5; i++)
        {
            var num = Utils.DistinctRandomNumbers(missionInfoTable[i + 1].Count, markCount);
            nums.Add(num);
        }
        difficulty = gm.playerData.currentMissionDifficulty;
        difficultyAdjustment.value = difficulty;
        OnAdjustmentDifficulty();
    }

    private void UpdateMissionNameText()
    {
        int k = 0;
        for (int j = 0; j < marks.Length; j++)
        {
            if (marks[j].GetComponent<MissionMarkData>().isMarkOn)
            {
                if (gm.playerData.isTutorial)
                {
                    var missionName = missionInfoTable[0][0]["NameString"];
                    var missionDifficulty = missionInfoTable[0][0]["Level"];
                    string mission = $"Lv.{missionDifficulty}\n{gm.GetStringByTable($"{missionName}")}";
                    marks[1].GetComponentInChildren<TextMeshProUGUI>().text = mission;
                    var buttons = marks[1].GetComponentsInChildren<Button>();
                    foreach (var button in buttons)
                    {
                        button.onClick.AddListener(() => UpdateMissionInfo(0, 0));
                    }
                }
                else
                {
                    int index = k++;
                    var missionName = missionInfoTable[difficulty][nums[difficulty - 1][index]]["NameString"];
                    var missionDifficulty = missionInfoTable[difficulty][nums[difficulty - 1][index]]["Level"];
                    string mission = $"Lv.{missionDifficulty}\n{gm.GetStringByTable($"{missionName}")}";
                    marks[j].GetComponentInChildren<TextMeshProUGUI>().text = mission;
                    var buttons = marks[j].GetComponentsInChildren<Button>();
                    foreach (var button in buttons)
                    {
                        button.onClick.AddListener(() => UpdateMissionInfo(difficulty, nums[difficulty - 1][index]));
                    }
                }
            }
            else
            {
                marks[j].SetActive(false);
            }
        }
    }

    public void UpdateMissionInfo(int difficulty, int num)
    {
        var selectMission = missionInfoTable[difficulty][num];
        gm.currentSelectMission = selectMission;

        portrait.sprite = gm.GetSpriteByAddress($"icon_{selectMission["VillainID"]}");
        string missionName = $"LV. {selectMission["Level"]} {gm.GetStringByTable($"{selectMission["NameString"]}")}";
        missionNameText.text = missionName;
        explanation.text = gm.GetStringByTable($"{selectMission["TooltipID"]}");
        gm.ClearBattleGroups();
        for (int i = 0; i < heroSlots.Length; i++)
        {
            heroSlots[i].sprite = null;
            heroSlots[i].color = new Color(0, 0, 0, 0);
        }
        for (int i = 0; i < fitProperties.Length; i++)
        {
            string key = $"{selectMission[$"BonusType{i + 1}"]}";
            fitProperties[i].text = gm.GetStringByTable(key);
            fitProperties[i].fontStyle = FontStyles.Normal;
            fitProperties[i].color = Color.white;
        }
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
        heroSlots[heroSlotsIndex].sprite = gm.GetSpriteByAddress($"icon_{liveData.name}");
        heroSlots[heroSlotsIndex].color = Color.white;
        selectIndexGroup[heroSlotsIndex] = index;

        if (duplication != null)
        {
            heroSlots[(int)duplication].sprite = null;
            heroSlots[(int)duplication].color = new Color(0, 0, 0, 0);
            selectIndexGroup[(int)duplication] = null;
        }

        PropertyMatchingCheck();
    }

    // Hero Slot 에서 Index 전달
    public void ConfirmSelectButton(int idx)
    {
        heroSlotsIndex = idx;
    }

    // 적합 속성 체크
    private void PropertyMatchingCheck()
    {
        var selectedHeroes = gm.GetSelectedHeroes();

        for (int i = 0; i < fitProperties.Length; i++)
        {
            foreach (var hero in selectedHeroes)
            {
                if (hero != null)
                {
                    string job = gm.GetStringByTable($"herotype_{(CharacterJob)hero.GetComponent<CharacterDataBundle>().data.job}");
                    if (job.Equals(fitProperties[i].text))
                    {
                        fitProperties[i].fontStyle = FontStyles.Bold;
                        fitProperties[i].color = Color.green;
                        gm.fitPropertyFlags[i] = true;
                        break;
                    }
                    fitProperties[i].fontStyle = FontStyles.Normal;
                    fitProperties[i].color = Color.white;
                }
                gm.fitPropertyFlags[i] = false;
            }
        }
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
        else
        {
            UIManager.Instance.ShowPopup(0);
        }
    }

    public void OnAdjustmentDifficulty()
    {
        difficulty = (int)difficultyAdjustment.value;
        difficultyAdjustment.GetComponentInChildren<TextMeshProUGUI>().text = $"{difficulty}";
        gm.playerData.currentMissionDifficulty = difficulty;
        UpdateMissionNameText();
    }
}