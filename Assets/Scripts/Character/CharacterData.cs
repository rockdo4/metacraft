using System;
using UnityEngine;

[Serializable]
public class CharacterData
{
    public string heroName;
    public string grade;
    public string type;
    public string level;

    public void PrintState()
    {
        Logger.Debug($"{heroName} {grade} {type} {level}");
    }
}