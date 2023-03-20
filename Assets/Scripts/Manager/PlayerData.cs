using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public string playerId = "000001";
    public string playerName = "default name";
    //public DayOfWeek currentDay = DayOfWeek.¿ù;
    //public int cumulateGameDay = 0;
    public int gold = 0;

    [Range(1, 30)]
    public int officeLevel = 1;
    public int officeExperience = 0;
    public int missionDifficulty;
    public int isTrainingOpen;
    public int isDispatchOpen;
    public int trainingLevel;
    public int dispatchLevel;
    public int stamina;
    public int inventoryCount;
    public string officeImage = "Container";
    [Range(1, 4)]
    public int battleSpeed = 1;
    public bool isAuto = false;
}