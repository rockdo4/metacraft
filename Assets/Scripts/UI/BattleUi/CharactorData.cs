using System;

public class CharactorData
{
    public int damage; //�Ϲ� ���� ������
    public int def; //����
    public int hp; //ü��
    public int speed; //�̼�
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

    public Action attackEvent;
    public Action skillEvent;
    public Action passiveEvent;
}