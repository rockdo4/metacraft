using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public SceneIndex currentScene = SceneIndex.Title;
    public PlayerData playerData;

    // Origin Database - Set Prefab & Scriptable Objects
    public List<GameObject> heroDatabase = new();

    // MyData - Craft, Load & Save to this data
    public List<GameObject> myHeroes = new();
    public Transform heroSpawnTransform;

    // Resources - Sprites, TextAsset + (Scriptable Objects, Sound etc)
    public Dictionary<string, Sprite> iconSprites = new();
    public Dictionary<string, Sprite> illustrationSprites = new();
    public List<Dictionary<string, object>> missionInfoList;
    public List<Dictionary<string, object>> dispatchInfoList;
    public List<Dictionary<string, object>> officeInfoList;
    public List<Dictionary<string, object>> eventInfoList;
    public List<Dictionary<string, object>> eventEffectInfoList;
    public Dictionary<string, List<Dictionary<string, string>>> eventEffectTagInfoList;
    public Dictionary<string, List<Dictionary<string, float>>> eventEffectNoTagInfoList;


    // Office Select
    public GameObject currentSelectObject; // Hero Info
    public Dictionary<string, object> currentSelectMission; // Mission Select
    public List<int?> battleGroups = new(3) { null, null, null }; // Mission Select -> Battle Scene
    // Dispatch Select

    public List<Effect> effects; // 사용할 이펙트들

    public override void Awake()
    {
        base.Awake();
        StartCoroutine(LoadAllResources());
    }

    private void Start()
    {
        foreach (var character in myHeroes)
        {
            character.SetActive(false);
        }
    }

    public void SetHeroesOrigin()
    {
        int count = myHeroes.Count;
        for (int i = 0; i < count; i++)
        {
            Utils.CopyPositionAndRotation(myHeroes[i], heroSpawnTransform);
        }
    }

    public List<GameObject> GetSelectedHeroes()
    {
        List<GameObject> selectedHeroes = new();
        int count = battleGroups.Count;

        for (int i = 0; i < count; i++)
        {
            selectedHeroes.Add(battleGroups[i] == null ? null : myHeroes[(int)battleGroups[i]]);
        }

        return selectedHeroes;
    }

    private IEnumerator LoadAllResources()
    {
        // 미션 테이블 로드
        var mit = Addressables.LoadAssetAsync<TextAsset>("MissionInfoTable");

        mit.Completed +=
                (AsyncOperationHandle<TextAsset> obj) =>
                {
                    missionInfoList = CSVReader.SplitTextAsset(obj.Result);
                    Addressables.Release(obj);
                };

        // 파견 테이블 로드
        var dit = Addressables.LoadAssetAsync<TextAsset>("DispatchInfoTable");

        dit.Completed +=
                (AsyncOperationHandle<TextAsset> obj) =>
                {
                    dispatchInfoList = CSVReader.SplitTextAsset(obj.Result);
                    Addressables.Release(obj);
                };

        // 오피스 테이블 로드
        var oit = Addressables.LoadAssetAsync<TextAsset>("OfficeTable");

        oit.Completed +=
                (AsyncOperationHandle<TextAsset> obj) =>
                {
                    officeInfoList = CSVReader.SplitTextAsset(obj.Result);
                    Addressables.Release(obj);
                };

        // 이벤트 테이블 로드
        var eit = Addressables.LoadAssetAsync<TextAsset>("EventTable");

        eit.Completed +=
                (AsyncOperationHandle<TextAsset> obj) =>
                {
                    eventInfoList = CSVReader.SplitTextAsset(obj.Result);
                    Addressables.Release(obj);
                };

        // 이벤트 이펙트 테이블 로드
        var eeit = Addressables.LoadAssetAsync<TextAsset>("EventEffectTable");

        eeit.Completed +=
                (AsyncOperationHandle<TextAsset> obj) =>
                {
                    eventEffectInfoList = CSVReader.SplitTextAsset(obj.Result,true, false);
                    Addressables.Release(obj);

                    FixEventEffectTable();
                };

        List<AsyncOperationHandle> handles = new();

        // Resources 테이블로 뺄 예정
        List<string> spriteAddress = new()
        {
            "다인",
            "신하루",
            "이수빈",
            "한서은",
            "돌격형",
            "방어형",
            "사격형",
            "은밀형",
            "지원형",
            "하율",
        };

        foreach (string address in spriteAddress)
        {
            string iconKey = $"Icon_{address}";
            Addressables.LoadAssetAsync<Sprite>(iconKey).Completed +=
                (AsyncOperationHandle<Sprite> obj) =>
                {
                    iconSprites.Add(iconKey, obj.Result);
                    handles.Add(obj);
                };

            string illurKey = $"Illur_{address}";
            Addressables.LoadAssetAsync<Sprite>(illurKey).Completed +=
                (AsyncOperationHandle<Sprite> obj) =>
                {
                    illustrationSprites.Add(illurKey, obj.Result);
                    handles.Add(obj);
                };
        }

        bool loadAll = false;
        int count = 0;
        // 스프라이트 리소스 로드
        while (!loadAll)
        {
            count = 0;
            loadAll = true;
            foreach (var handle in handles)
            {
                if (!handle.IsDone)
                {
                    loadAll = false;
                    break;
                }
                count++;
            }
            yield return null;
        }
        ReleaseAddressable(handles);

        LoadAllData();
    }

    public void ReleaseAddressable(List<AsyncOperationHandle> handles)
    {
        foreach (var handle in handles)
            Addressables.Release(handle);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Navigator Back Button
        {
            // 종료하시겠습니까 팝업 띄우기 or 그냥 종료하기

            Application.Quit();
        }
        //if (Input.GetKeyDown(KeyCode.Home)) // Navigator Home Button
        //{
        //    // 홈버튼
        //}
        //if (Input.GetKeyDown(KeyCode.Menu)) // Navigator Menu Button
        //{
        //    // 메뉴 버튼
        //}

        //// Test Key Start
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    SaveAllData();
        //}

        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    LoadAllData();
        //}

        //if (Input.GetKeyDown(KeyCode.U))
        //{
        //    AddOfficeExperience(300);
        //}
        // Test Key End
    }

    public void OnApplicationQuit()
    {
        SaveAllData();
    }

    public void SaveAllData()
    {
        StringBuilder sb = new();
        sb.AppendLine("ID;Contents");
        sb.AppendLine($"PlayerData;{JsonUtility.ToJson(playerData)}");

        foreach (var hero in myHeroes)
        {
            LiveData data = hero.GetComponent<CharacterDataBundle>().data;
            sb.AppendLine($"Hero_{data.name};{JsonUtility.ToJson(data)}");
        }
        File.WriteAllText(GetSaveFilePath(), sb.ToString());
    }

    public void LoadAllData()
    {
        if (!File.Exists(GetSaveFilePath()))
            return;

        var loadData = CSVReader.ReadByPath(GetSaveFilePath(), false);
        foreach (var item in loadData)
        {
            string id = item["ID"].ToString();
            string contents = item["Contents"].ToString();
            if (id.Equals("PlayerData"))
            {
                playerData = JsonUtility.FromJson<PlayerData>(contents);
            }
            else if (id.Contains("Hero_"))
            {
                string heroName = id[5..];
                GameObject newHero = CreateNewHero(heroName);
                if (newHero != null)
                {
                    newHero.GetComponent<CharacterDataBundle>().data.SetLoad(contents);
                    newHero.name = heroName;
                    myHeroes.Add(newHero);
                }
                else
                {
                    Logger.Debug($"Load failed {heroName}");
                }
            }
        }
    }

    public GameObject CreateNewHero(string heroName)
    {
        return CreateNewHero(GetHeroIndex(heroName));
    }

    public GameObject CreateNewHero(int index)
    {
        if (index == -1)
            return null;
        return Instantiate(heroDatabase[index], heroSpawnTransform);
    }

    private string GetSaveFilePath()
    {
        return $"{Application.persistentDataPath}/SaveFile.msf";
    }

    public void LoadScene(int sceneIdx)
    {
        SceneManager.LoadScene(sceneIdx);
        currentScene = (SceneIndex)sceneIdx;
    }

    public void ClearBattleGroups()
    {
        battleGroups.Clear();
        for (int i = 0; i < 3; i++)
        {
            battleGroups.Add(null);
        }
    }

    public int GetHeroIndex(string heroName)
    {
        int count = heroDatabase.Count;
        for (int i = 0; i < count; i++)
        {
            string tableName = heroDatabase[i].GetComponent<CharacterDataBundle>().originData.name;
            if (tableName.Equals(heroName))
                return i;
        }
        return -1;
    }

    public int GetHeroIndex(GameObject hero)
    {
        return GetHeroIndex(hero.GetComponent<CharacterDataBundle>().data.name);
    }

    public Sprite GetSpriteByAddress(string address)
    {
        if (iconSprites.ContainsKey(address))
            return iconSprites[address];

        if (illustrationSprites.ContainsKey(address))
            return illustrationSprites[address];

        return null;
    }

    //요일 변경
    public void NextDay()
    {
        playerData.currentDay = playerData.currentDay != DayOfWeek.일 ? playerData.currentDay + 1 : DayOfWeek.월;
    }

    public void AddOfficeExperience(int exp)
    {
        playerData.officeExperience += exp;
        for (int i = 1; i < officeInfoList.Count; i++)
        {
            if (playerData.officeExperience <= (int)officeInfoList[i]["NeedExp"])
            {
                PlayerInfoUpdate(i);
                break;
            }
        }
    }

    private void PlayerInfoUpdate(int level)
    {
        playerData.officeLevel = (int)officeInfoList[level]["OfficeLevel"];
        playerData.missionDifficulty = (int)officeInfoList[level]["MissionDifficulty"];
        playerData.isTrainingOpen = (int)officeInfoList[level]["IsTrainingOpen"];
        playerData.isDispatchOpen = (int)officeInfoList[level]["IsDispatchOpen"];
        playerData.trainingLevel = (int)officeInfoList[level]["TrainingLevel"];
        playerData.dispatchLevel = (int)officeInfoList[level]["DispatchLevel"];
        playerData.stamina = (int)officeInfoList[level]["Stamina"];
        playerData.inventoryCount = (int)officeInfoList[level]["InventoryCount"];
        playerData.officeImage = (string)officeInfoList[level]["OfficeImage"];
        Logger.Debug($"현재 레벨 : {playerData.officeLevel}");
    }

    // 이벤트 이팩트 테이블 분리
    private void FixEventEffectTable()
    {
        eventEffectTagInfoList = new Dictionary<string,List<Dictionary<string,string>>>();
        for (int i = 0; i < eventEffectInfoList.Count; i++)
        {
            var midList = new List<Dictionary<string,string>>();
            for (int j = 0; j<10;j++)
            {
                var smallDic = new Dictionary<string,string>();
                string tag = $"Tag{j+1}";
                string beHaveTag = $"BeHaveTag{j + 1}";
                smallDic.Add((string)eventEffectInfoList[i][tag], (string)eventEffectInfoList[i][beHaveTag]);
                midList.Add(smallDic);
            }
            eventEffectTagInfoList.Add((string)eventEffectInfoList[i]["ID"], midList);
        }

        eventEffectNoTagInfoList = new Dictionary<string, List<Dictionary<string, float>>>();
        for (int i = 0; i < eventEffectInfoList.Count; i++)
        {
            var midList = new List<Dictionary<string, float>>();
            for (int j = 0; j < 3; j++)
            {
                var smallDic = new Dictionary<string, float>();
                string text = $"NoTagsText{j + 1}";
                string rate = $"NoTagsRate{j + 1}";
                smallDic.Add(eventEffectInfoList[i][text].ToString(), float.Parse(eventEffectInfoList[i][rate].ToString()));
                midList.Add(smallDic);
            }
            eventEffectNoTagInfoList.Add((string)eventEffectInfoList[i]["ID"], midList);
        }
    }
}