using System.Collections;
using System.Collections.Generic;
using System.Data;
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

    // MyData - Craft, Load & Save to this data
    public List<GameObject> myHeroes = new();
    public Transform heroSpawnTransform;

    // Resources - Sprites, TextAsset + (Scriptable Objects, Sound etc)
    private Dictionary<string, Sprite> iconSprites = new();
    private Dictionary<string, Sprite> illustrationSprites = new();
    public Dictionary<int, List<Dictionary<string, object>>> missionInfoDifficulty; // 작전 정보 난이도 키 추가
    public List<Dictionary<string, object>> dispatchInfoList; // 파견 정보
    public List<Dictionary<string, object>> officeInfoList;  // 사무소 레벨별 정보
    public List<Dictionary<string, object>> eventInfoList; // 이벤트 노드 정보
    public List<Dictionary<string, object>> eventEffectInfoList;  // 이벤트 노드 일반보상만 연결해놓기 위해 임시로 살림, 태그 검사 추가 시 추후 삭제 예정
    public Dictionary<string, List<Dictionary<string, List<string>>>> eventEffectTagInfoList;
    public Dictionary<string, List<Dictionary<string, List<string>>>> eventEffectNoTagInfoList;
    private Dictionary<string, Dictionary<string, object>> stringTable = new();
    private int languageIndex = 0; // kor

    public List<Dictionary<string, object>> compensationInfoList; // 보상 정보
    public List<Dictionary<string, object>> itemInfoList; // 아이템 정보

    public List<Dictionary<string, object>> supplyInfoList; // 보급 노드 정보

    // Office Select
    public GameObject currentSelectObject; // Hero Info
    public Dictionary<string, object> currentSelectMission; // Mission Select
    public List<int?> battleGroups = new(3) { null, null, null }; // Mission Select -> Battle Scene

    // Origin Database - Set Prefab & Scriptable Objects
    private List<GameObject> heroDatabase = new();

    public Color currMapColor;
    public List<Color> mapLigthColors;

    public override void Awake()
    {
        base.Awake();
        StartCoroutine(LoadAllResources());
    }

    public int heroDataCounts()
    {
        return heroDatabase.Count;
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
        Dictionary<string, AsyncOperationHandle> handles = new();

        // Load TextAssets
        TextAsset ta = Resources.Load<TextAsset>("TextAssetList");
        var tableNames = ta.text.Split("\r\n");

        int count = tableNames.Length;
        for (int i = 0; i < count; i++)
            handles.Add(tableNames[i], Addressables.LoadAssetAsync<TextAsset>(tableNames[i]));

        // Load Character Prefabs



        // Load Sprites
        count = heroDatabase.Count;
        for (int i = 0; i < count; i++)
        {
            string address = heroDatabase[i].GetComponent<CharacterDataBundle>().originData.name;
            string iconAddress = $"Icon_{address}";
            AsyncOperationHandle<Sprite> iconHandle = Addressables.LoadAssetAsync<Sprite>(iconAddress);
            iconHandle.Completed +=
                (AsyncOperationHandle<Sprite> obj) =>
                {
                    Sprite sprite = obj.Result;
                    iconSprites.Add(iconAddress, sprite);
                };
            //handles.Add(iconAddress, iconHandle);

            string IllurAddress = $"Illu_{address}";
            AsyncOperationHandle<Sprite> illuHandle = Addressables.LoadAssetAsync<Sprite>(IllurAddress);
            illuHandle.Completed +=
                (AsyncOperationHandle<Sprite> obj) =>
                {
                    Sprite sprite = obj.Result;
                    illustrationSprites.Add(IllurAddress, sprite);
                };
            //handles.Add(IllurAddress, illuHandle);
        }

        // 스프라이트 리소스 로드 대기
        bool loadAll = false;
        while (!loadAll)
        {
            count = 0;
            loadAll = true;
            foreach (var handle in handles)
            {
                if (!handle.Value.IsDone)
                {
                    loadAll = false;
                    break;
                }
                count++;
            }
            yield return null;
        }
        dispatchInfoList = CSVReader.SplitTextAsset(handles["DispatchInfoTable"].Result as TextAsset);
        officeInfoList = CSVReader.SplitTextAsset(handles["OfficeTable"].Result as TextAsset);
        eventInfoList = CSVReader.SplitTextAsset(handles["EventTable"].Result as TextAsset);
        compensationInfoList = CSVReader.SplitTextAsset(handles["CompensationTable"].Result as TextAsset);
        supplyInfoList = CSVReader.SplitTextAsset(handles["SupplyTable"].Result as TextAsset);
        itemInfoList = CSVReader.SplitTextAsset(handles["ItemInfoTable"].Result as TextAsset);
        eventEffectInfoList = CSVReader.SplitTextAsset(handles["EventEffectTable"].Result as TextAsset);  // 이벤트 노드 일반보상만 연결해놓기 위해 임시로 살림, 태그 검사 추가 시 추후 삭제 예정

        LoadAllData();
        FixMissionTable(CSVReader.SplitTextAsset(handles["MissionInfoTable"].Result as TextAsset));
        //FixEventEffectTable(CSVReader.SplitTextAsset(handles["EventEffectTable"].Result as TextAsset));
        AppendStringTable(CSVReader.SplitTextAsset(handles["StringTable_Desc"].Result as TextAsset), "StringTable_Desc");
        AppendStringTable(CSVReader.SplitTextAsset(handles["StringTable_Event"].Result as TextAsset), "StringTable_Event");
        AppendStringTable(CSVReader.SplitTextAsset(handles["StringTable_Proper"].Result as TextAsset), "StringTable_Proper");
        AppendStringTable(CSVReader.SplitTextAsset(handles["StringTable_UI"].Result as TextAsset), "StringTable_UI");

        ReleaseAddressable(handles);
        handles.Clear();
    }

    private void AppendStringTable(List<Dictionary<string, object>> rawData, string tableName)
    {
        int count = rawData.Count;
        for (int i = 0; i < count; i++)
        {
            var copy = rawData[i];
            string id = $"{rawData[i]["ID"]}";
            copy.Remove("ID");
            if (stringTable.ContainsKey(id))
            {
                Logger.Debug($"중복 키 : {tableName}, {id}");
            }
            else
                stringTable.Add($"{id}", copy);
        }
    }

    public string GetStringByTable(string key)
    {
        string languageKey = languageIndex switch
        {
            _ => "Contents",
        };
        if (stringTable.ContainsKey(key))
            return $"{stringTable[key][languageKey]}";
        else
        {
            Logger.Debug($"Load fail to string table. key [{key}]");
            return $"Load fail to string table. key [{key}]";
        }
    }

    public void ReleaseAddressable(Dictionary<string, AsyncOperationHandle> handles)
    {
        foreach (var elem in handles.Values)
        {
            Addressables.Release(elem);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Navigator Back Button
        {
            // 종료하시겠습니까 팝업 띄우기 or 그냥 종료하기

            Application.Quit();
        }

        //if (Input.GetKeyDown(KeyCode.P)) // 캐릭터 생성 임시코드
        //{
        //    GameManager gm = Instance;
        //    int count = gm.myHeroes.Count;
        //    if (count == gm.heroDatabase.Count)
        //        return;

        //    GameObject newHero = Instantiate(gm.heroDatabase[count], gm.heroSpawnTransform);
        //    gm.myHeroes.Add(newHero);
        //    newHero.SetActive(false);
        //}
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
        SetHeroesActive(false);
    }

    public void SetHeroesActive(bool value)
    {
        foreach (var character in myHeroes)
        {
            character.SetActive(value);
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
            {
                //Logger.Debug($"index: [{i}], name: [{heroName}]");
                return i;
            }
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
        {
            return iconSprites[address];
        }

        if (illustrationSprites.ContainsKey(address))
        {
            return illustrationSprites[address];
        }

        Logger.Debug($"Load sprite fail. address: {address}");
        return null;
    }

    //요일 변경
    //public void NextDay()
    //{
    //    playerData.currentDay = playerData.currentDay != DayOfWeek.일 ? playerData.currentDay + 1 : DayOfWeek.월;
    //    playerData.cumulateGameDay++;
    //}

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
        //Logger.Debug($"현재 레벨 : {playerData.officeLevel}");
    }

    // 이벤트 이팩트 테이블 분리
    //private void FixEventEffectTable(List<Dictionary<string, object>> eventEffectInfoList)
    //{
    //    eventEffectTagInfoList = new Dictionary<string, List<Dictionary<string, List<string>>>>();
    //    for (int i = 0; i < eventEffectInfoList.Count; i++)
    //    {
    //        var midList = new List<Dictionary<string, List<string>>>();
    //        for (int j = 0; j < 10; j++)
    //        {
    //            var smallDic = new Dictionary<string, List<string>>();
    //            var list = new List<string>();
    //            string priorityTag = $"PriorityTag{j + 1}";
    //            string priorityText = $"PriorityText{j + 1}";
    //            string priorityRewardType = $"PriorityRewardType{j + 1}";
    //            string priorityReward = $"PriorityReward{j + 1}";
    //            list.Add(priorityText);
    //            list.Add(priorityRewardType);
    //            list.Add(priorityReward);
    //            smallDic.Add((string)eventEffectInfoList[i][priorityTag], list);
    //            midList.Add(smallDic);
    //        }
    //        eventEffectTagInfoList.Add((string)eventEffectInfoList[i]["ID"], midList);
    //    }

    //    eventEffectNoTagInfoList = new Dictionary<string, List<Dictionary<string, List<string>>>>();
    //    for (int i = 0; i < eventEffectInfoList.Count; i++)
    //    {
    //        var midList = new List<Dictionary<string, List<string>>>();
    //        for (int j = 0; j < 3; j++)
    //        {
    //            var smallDic = new Dictionary<string, List<string>>();
    //            var list = new List<string>();
    //            string text = $"NormalvalueText{j + 1}";
    //            string rate = $"Normalvalue{j + 1}";
    //            string rewardType = $"NormalRewardType{j + 1}";
    //            string reward = $"NormalReward{j + 1}";
    //            list.Add(rate);
    //            list.Add(rewardType);
    //            list.Add(reward);
    //            smallDic.Add(eventEffectInfoList[i][text].ToString(), list);
    //            midList.Add(smallDic);
    //        }
    //        eventEffectNoTagInfoList.Add((string)eventEffectInfoList[i]["ID"], midList);
    //    }
    //}

    // 작전 테이블 난이도 구분
    private void FixMissionTable(List<Dictionary<string, object>> missionInfoList)
    {
        missionInfoDifficulty = new Dictionary<int, List<Dictionary<string, object>>>();
        for (int i = 1; i < 6; i++)
        {
            missionInfoDifficulty.Add(i, new List<Dictionary<string, object>>());
        }
        for (int i = 0; i < missionInfoList.Count; i++)
        {
            switch ((int)missionInfoList[i]["Difficulty"])
            {
                case 1:
                    missionInfoDifficulty[1].Add(missionInfoList[i]);
                    break;
                case 2:
                    missionInfoDifficulty[2].Add(missionInfoList[i]);
                    break;
                case 3:
                    missionInfoDifficulty[3].Add(missionInfoList[i]);
                    break;
                case 4:
                    missionInfoDifficulty[4].Add(missionInfoList[i]);
                    break;
                case 5:
                    missionInfoDifficulty[5].Add(missionInfoList[i]);
                    break;
            }
        }
    }

    /************************************* Minu *******************************************/
    public void SetDifferentColor()
    {
        if (currMapColor.Equals(Color.white))
        {
            currMapColor = mapLigthColors[0];
            return;
        }

        Color prevColor;
        do
        {
            int randomRange = UnityEngine.Random.Range(0, 101);
            prevColor = currMapColor;

            if (randomRange >= 60)
            {
                currMapColor = mapLigthColors[0];
            }
            else if (randomRange >= 20)
            {
                currMapColor = mapLigthColors[1];
            }
            else if (randomRange >= 0)
            {
                currMapColor = mapLigthColors[2];
            }

        } while (currMapColor.Equals(prevColor));
    }

    public Color GetMapLightColor()
    {
        return currMapColor;
    }
}