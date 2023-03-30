using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public string playerId = "000001";
    public string playerName = "default name";
    public int gold = 0;

    [Range(1, 30)]
    public int officeLevel = 1;
    public int officeExperience = 0;
    public string officeImage = "Container";

    [Range(1, 4)]
    public int battleSpeed = 1;
    public bool isAuto = false;
    //public bool isTutorial = true;
}