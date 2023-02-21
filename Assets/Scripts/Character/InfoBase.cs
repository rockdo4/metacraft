using System;
using UnityEngine;

public abstract class InfoBase : ScriptableObject, IComparable<InfoBase>
{
    public new string   name;               // 이름
    public string       grade;              // 등급
    public int          level;              // 레벨
    public string       resourceAddress;    // 어드레서블 에셋 주소

    public int CompareTo(InfoBase other)
    {
        return name.CompareTo(other.name);
    }
}