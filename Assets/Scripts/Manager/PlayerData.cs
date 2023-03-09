using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public string playerId = "000001";
    public string playerName = "¸®¾ó½ºÅæ";
    public DayOfWeek currentDay = DayOfWeek.¿ù;
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
    public string officeImage;
    [Range(1, 3)]
    public int battleSpeed = 1;
    public bool isAuto = false;
}