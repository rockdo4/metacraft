using System;
using UnityEngine;

public abstract class InfoBase : ScriptableObject, IComparable<InfoBase>
{
    public new string   name;               // �̸�
    public string       grade;              // ���
    public int          level;              // ����
    public string       resourceAddress;    // ��巹���� ���� �ּ�

    public int CompareTo(InfoBase other)
    {
        return name.CompareTo(other.name);
    }
}