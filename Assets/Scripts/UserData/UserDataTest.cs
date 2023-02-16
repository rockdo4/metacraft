using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

//GameManager 대신에 사용할 Test 클래스
public class UserDataTest : MonoBehaviour
{
    [SerializeField]
    private UserData data; //저장하려는 데이터

    readonly string folderPath = @"C:/TeamProject/Json/"; //유저 정보 저장 경로
    private string userId = "userData.json";
    private string userPath; //위에 두 변서 Combine 하여 사용

    [SerializeField]
    private Dictionary<int, int> levelExpTable = new Dictionary<int, int>(); //레벨별 경험치
    private int maxLevel; // levelExpTable의 키중에 최대값

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
            data.Init("user_0001","0001"); //임시 이름, 아이디로 생성. 나중에 매개변수를 중복체크같은 작업이 이루어진 데이터 전달
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
