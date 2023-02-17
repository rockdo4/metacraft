using System;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
struct UserData
{
    [SerializeField] private string nickName;
    [SerializeField] private string id;

    [SerializeField] private string lastLoginTime;

    [SerializeField] private int level;
    [SerializeField] private int exp;
    [SerializeField] private int money;
    [SerializeField] private int stamina;

    public string LastLoginTime { get { return lastLoginTime; } }
    public int Level { get { return level; } }
    public int Exp { get => exp; }
    public int Money { get => money; }


    public int AddMoney(int m)
    {
        long value = (long)money + (long)m;   //������ ���� ����ó��
        if (value > int.MaxValue)
        {
            return money = int.MaxValue;
        }
        else
        {
            return money = (int)value;
        }
    }

    public bool UseMoney(int m)
    {
        if (money - m < 0)
        {
            return false;
        }
        else
        {
            money -= m;
            return true;
        }
    }

    public bool AddExp(int e, int maxExp, bool isMaxLevel) // �������� �ߴ��� ���ߴ��� ��ȯ
    {
        exp += e;

        if (isMaxLevel)
        {
            exp = Mathf.Min(exp, maxExp);
            return false;
        }
        else
        {
            if (exp >= maxExp)
            {
                exp -= maxExp;
                level++;
                return true;
            }
            else
                return false;
        }
    }

    public void Login()
    {
        lastLoginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //�ϴ��� �̷��� ������, �������� ����Ƿ� Format���ڿ� �����.
    }

    public void Init(string newName ,string newId)
    {
        nickName = newName;
        id = newId;
        level = 1;
        money = 0;
        exp = 0;
        stamina = 100;
        Login();
    }
}