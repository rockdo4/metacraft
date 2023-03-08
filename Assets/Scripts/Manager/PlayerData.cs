using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public string playerId = "000001";
    public string playerName = "리얼스톤";
    public DayOfWeek currentDay = DayOfWeek.월;
    public int cumulateGameDay = 0;
    public int gold = 0;
    public int characterCount = 5;

    [Range(1, 30)]
    public int officeLevel;
    public int officeExperience;
    public int missionDifficulty;
    public int isTrainingOpen;
    public int isDispatchOpen;
    public int trainingLevel;
    public int dispatchLevel;
    public int stamina;
    public int inventoryCount;
    [Range(1, 3)]
    public int battleSpeed = 1;

    private void PlayerInfoUpdate(int level)
    {
        var officeTable = GameManager.Instance.officeInfoList;
        officeLevel = (int)officeTable[level]["OfficeLevel"];
        missionDifficulty = (int)officeTable[level]["MissionDifficulty"];
        isTrainingOpen = (int)officeTable[level]["IsTrainingOpen"];
        isDispatchOpen = (int)officeTable[level]["IsDispatchOpen"];
        trainingLevel = (int)officeTable[level]["TrainingLevel"];
        dispatchLevel = (int)officeTable[level]["DispatchLevel"];
        stamina = (int)officeTable[level]["Stamina"];
        inventoryCount = (int)officeTable[level]["InventoryCount"];
        Logger.Debug($"현재 레벨 : {officeLevel}");
    }

    public void AddOfficeExperience(int exp)
    {
        var officeTable = GameManager.Instance.officeInfoList;
        officeExperience += exp;
        for (int i = 1; i < officeTable.Count; i++)
        {
            if (officeExperience < (int)officeTable[i]["NeedExp"])
            {
                PlayerInfoUpdate(i);
                break;
            }
        }
    }
}