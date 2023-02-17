using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

//GameManager ��ſ� ����� Test Ŭ����
public class UserDataTest : MonoBehaviour
{
    [SerializeField]
    UserData data; //�����Ϸ��� ������

    string folderPath = @"C:/TeamProject/Json/"; //���� ���� ���� ���
    string userId = "userData.json";
    string userPath; //���� �� ���� Combine �Ͽ� ���

    [SerializeField]
    Dictionary<int, int> levelExpTable = new Dictionary<int, int>(); //������ ����ġ
    int maxLevel; // levelExpTable�� Ű�߿� �ִ밪

    float lastSaveTime;
    float saveInterval = 3f;

    private void Awake()
    {
        userPath = Path.Combine(folderPath, userId);
        LoadGameDataTest();
        Load();
    }

    [ContextMenu("Save")]
    public void Save()
    {
        File.WriteAllText(userPath, JsonUtility.ToJson(data));
    }

    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(userPath))
        {
            data = JsonUtility.FromJson<UserData>(File.ReadAllText(userPath));
            data.Login();
        }
        else
        {
            Logger.Debug("Not Data");
        }
    }

    public void AddMoneyTest(int value)
    {
        data.AddMoney(value);
        Save();
    }

    public void UseMoneyTest()
    {
        data.UseMoney(10);
        Save();
    }

    public void AddExpTest()
    {
        if (data.AddExp(100, levelExpTable[data.Level], data.Level == maxLevel))
            Logger.Debug("Level Up");

        Save();
    }

    public void LoadGameDataTest()
    {
        lastSaveTime = 0f;

        levelExpTable[1] = 1000;
        levelExpTable[2] = 1200;
        levelExpTable[3] = 1300;
        levelExpTable[4] = 1400;
        levelExpTable[5] = 1500;

        maxLevel = levelExpTable.Keys.Max();
    }

    public void AutoSave()
    {
        lastSaveTime += Time.deltaTime;
        if (lastSaveTime > saveInterval)
        {
            lastSaveTime = 0;
            Save();
        }
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    public void Update()
    {
        AutoSave();

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            AddExpTest();
        }
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            AddMoneyTest(10000000);
        }
    }
}
