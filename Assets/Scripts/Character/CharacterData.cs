using System;

[Serializable]
public class CharacterData : IComparable<CharacterData>
{
    // �⺻ ����
    public string   heroName;         //�̸�
    public string   grade;            //���
    public string   job;              //����
    public int      level;            //����

    // ���� ����
    public int damage; //�Ϲ� ���� ������
    public int def; //����
    public int hp; //ü��
    public int speed; //�̼�

    public float attackDistance; //���� ���� = �׺���̼� ��ž ���Ͻ�
    public int attackCount; // �Ϲݽ�ų �ִ� ���� ��ü��

    public float critical; //ũ��Ƽ�� Ȯ��
    public float criticalDmg; //ũ��Ƽ�� ������ ����
    public float evasion; //ȸ����
    public float accuracy; //���߷�

    public float cooldown;
    public float skillCooldown;

    public int exp;

    public int CompareTo(CharacterData other)
    {
        return heroName.CompareTo(other.heroName);
    }

    public void PrintBattleInfo()
    {
        string printFormat = "{0} : {1}";
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