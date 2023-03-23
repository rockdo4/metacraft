using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Character/CharacterData")]
public class CharacterData : ScriptableObject, IComparable<CharacterData>
{
    // ����
    public new string   name;               // �̸�
    [Range(1, 5)]
    public int          grade = 1;          // ���
    [Range(1, 5)]
    public int          maxGrade = 5;       // �±� ���� �ִ� ���
    public int          job;                // ����

    // ����
    public float        baseDamage = 0;    // �Ϲ� ���� ������
    public float        damageLevelCoefficient = 0; // ������ ������ ���
    public float        baseDefense = 0;    // ����
    public float        defenseLevelCoefficient = 0; //���� ������ ���
    public float        healthPoint = 0;  // �ִ� ü��
    public float        healthPointLevelCoefficient = 0; // �ִ� ü�� ������ ���
    [Range(0f, 1f)]
    public float        critical = 0f;      // ũ��Ƽ�� Ȯ��
    [Range(1f, 10f)]
    public float        criticalDmg = 2f;   // ũ��Ƽ�� ������ ����
    public float          moveSpeed = 3;      // �̵� �ӵ�. ����, �ʱⰪ ���� �ʿ�
    [Range(0f, 1f)]
    public float        accuracy = 1f;      // ���߷�
    [Range(0f, 1f)]
    public float        evasion = 0f;       // ȸ����
    public List<string> tags;               // �±�(Ư��)

    public int CompareTo(CharacterData other)
    {
        return name.CompareTo(other.name);
    }
}