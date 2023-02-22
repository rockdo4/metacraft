using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterInfo", menuName = "Character/CharacterInfo")]
public class CharacterInfo : ScriptableObject, IComparable<CharacterInfo>
{
    public new string   name;               // �̸�
    public string       grade;              // ���
    public string       job;                // ����

    public string       maxGrade;           // �±� ���� �ִ� ���
    public int          energy;             // Ȱ���� �Ҹ�
    public int          likeability;        // ȣ����
    public int          level;              // ����
    public int          exp;                // ����ġ

    public int CompareTo(CharacterInfo other)
    {
        return name.CompareTo(other.name);
    }
}