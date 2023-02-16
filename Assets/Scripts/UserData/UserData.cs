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
        long value = (long)money + (long)m;   //만약을 위한 예외처리
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

    public bool AddExp(int e, int maxExp, bool isMaxLevel) // 레벨업을 했는지 안했는지 반환
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
        lastLoginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //일단은 이렇게 쓰지만, 가비지가 생기므로 Format문자열 만들기.
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