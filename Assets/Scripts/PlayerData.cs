using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public string playerId = "000001";
    public string playerName = "������";

    [Range(1, 10)]
    public int officeLevel = 1;
    public int officeExperience = 0;
    public int inventoryCount = 5;
    public int characterCount = 5;
    public int trainingCount = 1;
    public int dispatchCount = 1;
    public DayOfWeek currentDay = DayOfWeek.��;
    public int cumulateGameDay = 0;
    public int gold = 0;
    [Range(1, 3)]
    public int battleSpeed = 1;
}