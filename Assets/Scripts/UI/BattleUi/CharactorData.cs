using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CharactorData
{
    public string heroName;
    public int damage; //�Ϲ� ���� ������
    public int def; //����
    public int hp; //ü��
    public int speed; //�̼�


    public float attackDistance; //���� ���� = �׺���̼� ��ž ���Ͻ�
    public int attackCount; // �Ϲݽ�ų �ִ� ���� ��ü��

    public float chritical; //ũ��Ƽ�� Ȯ��
    public float chriticalDmg; //ũ��Ƽ�� ������ ����
    public float evasion; //ȸ����
    public float accuracy; //���߷�
    public char grade; //���
    public int level; //����
    public string type; //����

    public float cooldown;
    public float skillCooldown;

    public int exp;

    private string printFormat = "{0} : {1}";
        
    public void PrintBattleInfo()
    {
        Logger.Debug(string.Format(printFormat, "������", damage));
        //Logger.Debug(string.Format(printFormat, "����", def));
        //Logger.Debug(string.Format(printFormat, "ü��", hp));
        //Logger.Debug(string.Format(printFormat, "�̵��ӵ�", speed));
        //Logger.Debug(string.Format(printFormat, "ũ��Ƽ��", chritical));
        //Logger.Debug(string.Format(printFormat, "ũ��Ƽ�� ����", chriticalDmg));
        //Logger.Debug(string.Format(printFormat, "���߷�", accuracy));
        //Logger.Debug(string.Format(printFormat, "����", level));
        //Logger.Debug(string.Format(printFormat, "Ÿ��", type));
        Logger.Debug("");

        
    }
}