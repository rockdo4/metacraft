using System.Collections.Generic;
using UnityEngine;

public abstract class StatsBase : ScriptableObject
{
    private int             power;              // ������
    public int Power
    {
        // ������ ����
        private set { power = value; }
        get { return power; }
    }

    public int              baseDamage;         // �Ϲ� ���� ������
    public int              baseDefense;        // ����
    public int              healthPoint;        // �ִ� ü��
    public int              moveSpeed;          // �̵� �ӵ�

    public float            attackDistance;     // ���� ���� = �׺���̼� ��ž ���Ͻ�
    public int              attackCount;        // �Ϲݽ�ų �ִ� ���� ��ü��

    [Range(0f, 1f)]
    public float            critical;           // ũ��Ƽ�� Ȯ��
    [Range(1f, 10f)]        
    public float            criticalDmg;        // ũ��Ƽ�� ������ ����
    [Range(0f, 1f)]
    public float            evasion;            // ȸ����
    [Range(0f, 1f)]
    public float            accuracy;           // ���߷�

    public List<float>      cooldowns;          // ��Ÿ��
    // public List<Skill>   skills;             // ��ų ���
}