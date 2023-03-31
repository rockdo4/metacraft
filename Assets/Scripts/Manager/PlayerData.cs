using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public string playerName = string.Empty;
    public string portraitKey = "icon_hero_egostick";
    public int gold = 0;

    [Range(1, 30)]
    public int officeLevel = 1;
    public int officeExperience = 0;
    public string officeImage = "Container";

    [Range(1, 4)]
    public int battleSpeed = 1;
    public bool isAuto = false;
}