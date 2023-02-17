using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

//GameManager ��ſ� ����� Test Ŭ����
public class UserDataTest : MonoBehaviour
{
    [SerializeField]
    private UserData data; //�����Ϸ��� ������

    readonly string folderPath = @"C:/TeamProject/Json/"; //���� ���� ���� ���
    private string userId = "userData.json";
    private string userPath; //���� �� ���� Combine �Ͽ� ���

    [SerializeField]
    private Dictionary<int, int> levelExpTable = new Dictionary<int, int>(); //������ ����ġ
    private int maxLevel; // levelExpTable�� Ű�߿� �ִ밪

    private float lastSaveTime;
    private float saveInterval = 3f;

    private void Awake()
    {
        userPath = Path.Combine(folderPath, userId);
        LoadGameDataTest();
        LoadData();
    }

    [ContextMenu("SaveData")]
    public void SaveData()
    {
        
        File.WriteAllText(userPath, JsonUtility.ToJson(data));
    }

    [ContextMenu("Load")]
    public void LoadData()
    {
        if (File.Exists(userPath))
        {
            data = JsonUtility.FromJson<UserData>(File.ReadAllText(userPath));
            data.Login();
        }
        else
        {
            Logger.Debug("Not Find Data. Make new userData");

            data = new UserData();
            data.Init("user_0001","0001"); //�ӽ� �̸�, ���̵�� ����. ���߿� �Ű������� �ߺ�üũ���� �۾��� �̷���� ������ ����
            File.WriteAllText(userPath, JsonUtility.ToJson(data));

        }
    }

    public void AddMoneyTest(int value)
    {
        data.AddMoney(value);
        SaveData();
    }

    public void UseMoneyTest()
    {
        data.UseMoney(10);
        SaveData();
    }

    public void AddExpTest()
    {
        if (data.AddExp(100, levelExpTable[data.Level], data.Level == maxLevel))
            Logger.Debug("Level Up");

        SaveData();
    }

    private void LoadGameDataTest()
    {
        lastSaveTime = 0f;

        levelExpTable[1] = 1000;
        levelExpTable[2] = 1200;
        levelExpTable[3] = 1300;
        levelExpTable[4] = 1400;
        levelExpTable[5] = 1500;

        maxLevel = levelExpTable.Keys.Max();
    }

    private void AutoSave()
    {
        lastSaveTime += Time.deltaTime;
        if (lastSaveTime > saveInterval)
        {
            lastSaveTime = 0;
            SaveData();
        }
    }

    private void OnApplicationQuit()
    {
        SaveData();
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
