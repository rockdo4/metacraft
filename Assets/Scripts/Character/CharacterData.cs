using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Character/CharacterData")]
public class CharacterData : ScriptableObject, IComparable<CharacterData>
{
    public new string   name;               // �̸�
    [Range(1, 5)]
    public int          grade;              // ���
    public int          maxGrade;           // �±� ���� �ִ� ���

    public string       job;                // ����, Ư��

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
    public int currentHp = 500;     // ���� ü��
    public int moveSpeed = 3;        // �̵� �ӵ�. ����, �ʱⰪ ���� �ʿ�
    [Range(0f, 1f)]
    public float critical = 0f;      // ũ��Ƽ�� Ȯ��
    [Range(1f, 10f)]
    public float criticalDmg = 2f;   // ũ��Ƽ�� ������ ����
    [Range(0f, 1f)]
    public float evasion = 0f;       // ȸ����
    [Range(0f, 1f)]
    public float accuracy = 1f;    // ���߷�

    public int CompareTo(CharacterData other)
    {
        return name.CompareTo(other.name);
    }
}