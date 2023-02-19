using System;
using UnityEngine;

[Serializable]
public class CharacterData : IComparable<CharacterData>
{
    public string heroName;
    public string grade;
    public string type;
    public string level;

    //public Sprite sprite;

    public int CompareTo(CharacterData other)
    {
        return heroName.CompareTo(other.heroName);
    }

    public void PrintState()
    {
        Logger.Debug($"{heroName} {grade} {type} {level}");
        //sprite = Resources.Load<Sprite>(iconSpriteId);
    }
}