using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Character/CharacterData")]
public class CharacterData : ScriptableObject, IComparable<CharacterData>
{
    public new string   name;               // �̸�
    public string       grade;              // ���
    public string       job;                // ����

    public string       maxGrade;           // �±� ���� �ִ� ���
    public int          energy;             // Ȱ���� �Ҹ�

    [Range(1, 20)]
    public int          level = 1;          // ����
    public int          exp;                // ����ġ
    [Range(0, 1000)]
    public int          likeability;        // ȣ����

    public int Power
    {
        // ������ ����
        private set { power = baseDamage + baseDefense + healthPoint * 10; }
        get { return power; }
    }

    private int power;              // ������
    public int baseDamage = 50;     // �Ϲ� ���� ������
    public int baseDefense = 0;     // ����
    public int healthPoint = 500;   // �ִ� ü��
    public int moveSpeed = 3;        // �̵� �ӵ�. ����, �ʱⰪ ���� �ʿ�
    [Range(0f, 1f)]
    public float critical = 0f;      // ũ��Ƽ�� Ȯ��
    [Range(1f, 10f)]
    public float criticalDmg = 2f;   // ũ��Ƽ�� ������ ����
    [Range(0f, 1f)]
    public float evasion = 0f;       // ȸ����
    [Range(0f, 1f)]
    public float accuracy = 1f;    // ���߷�

    public List<int> synergys;       // �ó��� ����Ʈ. ���� or Enum���� ������ �ִٰ�
                                     // ���ݴ뿡 ���� �ó��� index�� ������ �ִ� ������ ������ �ó��� �ߵ�

    public int CompareTo(CharacterData other)
    {
        return name.CompareTo(other.name);
    }
}