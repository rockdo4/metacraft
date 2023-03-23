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
    public InventoryData inventoryData;

    // MyData - Craft, Load & Save to this data
    public Dictionary<string, GameObject> myHeroes = new();
    public Transform heroSpawnTransform;    

    // Resources - Sprites, TextAsset + (Scriptable Objects, Sound etc)
    private Dictionary<string, Sprite> iconSprites = new();
    private Dictionary<string, Sprite> illustrationSprites = new();
    private Dictionary<string, Sprite> stateIconSprites = new();
    private Dictionary<string, Sprite> itemSprites = new();
    public Dictionary<int, List<Dictionary<string, object>>> missionInfoDifficulty; // 작전 정보 난이도 키 추가
    public List<Dictionary<string, object>> dispatchInfoList; // 파견 정보
    public List<Dictionary<string, object>> officeInfoList;  // 사무소 레벨별 정보
    public List<Dictionary<string, object>> eventInfoList; // 이벤트 노드 정보
    public List<Dictionary<string, object>> eventEffectInfoList;  // 이벤트 노드 일반보상만 연결해놓기 위해 임시로 살림, 태그 검사 추가 시 추후 삭제 예정
    public List<Dictionary<string, object>> enemyInfoList;
    public List<Dictionary<string, object>> enemySpawnList;
    public Dictionary<string, List<Dictionary<string, List<string>>>> eventEffectTagInfoList;
    public Dictionary<string, List<Dictionary<string, List<string>>>> eventEffectNoTagInfoList;
    private Dictionary<string, Dictionary<string, object>> stringTable = new();
    private int languageIndex = 0; // kor

    public List<Dictionary<string, object>> compensationInfoList; // 보상 정보
    public List<Dictionary<string, object>> itemInfoList; // 아이템 정보

    public List<Dictionary<string, object>> supplyInfoList; // 보급 노드 정보

    public List<Dictionary<string, object>> recruitmentReplacementTable; // 영입 중복 대체 테이블
    public List<Dictionary<string, object>> expRequirementTable; // 경험치 요구량 테이블
    public List<Dictionary<string, object>> maxLevelTable; // 최대 레벨 테이블

    // Office Select
    public GameObject currentSelectObject; // Hero Info
    public Dictionary<string, object> currentSelectMission; // Mission Select
    public List<int?> battleGroups = new(3) { null, null, null }; // Mission Select -> Battle Scene
    public List<bool> fitPropertyFlags = new(3) { false, false, false };

    // Origin Database - Set Prefab & Scriptable Objects
    public List<GameObject> heroDatabase = new();
    public AssetLoadProgress progress;

    public Color currMapColor;
    public List<Color> mapLigthColors;

    public override void Awake()
    {
        base.Awake();
        StartCoroutine(LoadAllResources());
    }

    public void SetHeroesOrigin()
    {
        foreach (var elem in myHeroes)
        {
            Utils.CopyPositionAndRotation(elem.Value, heroSpawnTransform);
        }
    }

    public List<GameObject> GetSelectedHeroes()
    {
        List<GameObject> selectedHeroes = new();
        int count = battleGroups.Count;

        for (int i = 0; i < count; i++)
        {
            int key = battleGroups[i] == null ? -1 : (int)battleGroups[i];
            selectedHeroes.Add(battleGroups[i] == null ? null : myHeroes[heroDatabase[key].name.ToLower()]);
        }

        return selectedHeroes;
    }

    private IEnumerator LoadAllResources()
    {
        Dictionary<string, AsyncOperationHandle> releasehandles = new();
        List<AsyncOperationHandle> unreleasehandles = new();
        int total = 0;

        // Load TextAssets
        TextAsset ta = Resources.Load<TextAsset>("TextAssetList");
        TextAsset ia = Resources.Load<TextAsset>("ItemAssetList");
        var tableNames = ta.text.Split("\r\n");
        var itemNames = ia.text.Split("\r\n");

        int count = tableNames.Length;
        for (int i = 0; i < count; i++)
        {
            string key = tableNames[i];
            if (key.Length != 0)
            {
                AsyncOperationHandle<TextAsset> tas = Addressables.LoadAssetAsync<TextAsset>(key);
                releasehandles.Add(key, tas);
                tas.Completed +=
                    (AsyncOperationHandle<TextAsset> obj) =>
                {
                    //Logger.Debug($"{key} load success");
                };
                total++;
            }
        }

        // Load Sprites
        count = heroDatabase.Count;
        for (int i = 0; i < count; i++)
        {
            string address = heroDatabase[i].GetComponent<CharacterDataBundle>().originData.name;
            string iconAddress = $"icon_{address}";
            AsyncOperationHandle<Sprite> iconHandle = Addressables.LoadAssetAsync<Sprite>(iconAddress);
            iconHandle.Completed +=
                (AsyncOperationHandle<Sprite> obj) =>
                {
                    Sprite sprite = obj.Result;
                    iconSprites.Add(iconAddress, sprite);
                };
            unreleasehandles.Add(iconHandle);

            string IllurAddress = $"illu_{address}";
            AsyncOperationHandle<Sprite> illuHandle = Addressables.LoadAssetAsync<Sprite>(IllurAddress);
            illuHandle.Completed +=
                (AsyncOperationHandle<Sprite> obj) =>
                {
                    Sprite sprite = obj.Result;
                    illustrationSprites.Add(IllurAddress, sprite);
                };
            unreleasehandles.Add(illuHandle);
            total += 2;
        }

        count = 29; //임시. 나중에 버프 테이블 불러오게 수정할 예정
        for (int i = 1; i <= count; i++)
        {
            string address = string.Format("state{0}",i);
            AsyncOperationHandle<Sprite> stateIconHandle = Addressables.LoadAssetAsync<Sprite>(address);
            stateIconHandle.Completed +=
                (AsyncOperationHandle<Sprite> obj) =>
                {
                    Sprite sprite = obj.Result;
                    stateIconSprites.Add(address, sprite);
                };
            unreleasehandles.Add(stateIconHandle);
            total++;
        }

        int itemCount = itemNames.Length;
        for (int i = 0; i < itemCount; i++)
        {
            string address = $"{itemNames[i]}";
            AsyncOperationHandle<Sprite> itemIconHandle = Addressables.LoadAssetAsync<Sprite>(address);

            itemIconHandle.Completed +=
                (AsyncOperationHandle<Sprite> obj) =>
                {
                    Sprite sprite = obj.Result;
                    itemSprites.Add(address, sprite);
                };
            unreleasehandles.Add(itemIconHandle);
            total++;
        }

        // 스프라이트 리소스 로드 대기
        bool loadAll = false;
        while (!loadAll)
        {
            count = 0;
            loadAll = true;
            foreach (var handle in releasehandles)
            {
                if (!handle.Value.IsDone)
                {
                    loadAll = false;
                    break;
                }
                count++;
            }
            if (!loadAll)
            {
                progress.SetProgress(count, total);
                yield return null;
            }

            foreach (var handle in unreleasehandles)
            {
                if (!handle.IsDone)
                {
                    loadAll = false;
                    break;
                }
                count++;
            }
            progress.SetProgress(count, total);
            yield return null;
        }

        dispatchInfoList = CSVReader.SplitTextAsset(releasehandles["DispatchInfoTable"].Result as TextAsset);
        officeInfoList = CSVReader.SplitTextAsset(releasehandles["OfficeTable"].Result as TextAsset);
        eventInfoList = CSVReader.SplitTextAsset(releasehandles["EventTable"].Result as TextAsset);
        compensationInfoList = CSVReader.SplitTextAsset(releasehandles["CompensationTable"].Result as TextAsset);
        supplyInfoList = CSVReader.SplitTextAsset(releasehandles["SupplyTable"].Result as TextAsset);
        itemInfoList = CSVReader.SplitTextAsset(releasehandles["ItemInfoTable"].Result as TextAsset);
        enemyInfoList = CSVReader.SplitTextAsset(releasehandles["EnemyInfoTable"].Result as TextAsset);
        enemySpawnList = CSVReader.SplitTextAsset(releasehandles["EnemySpawnTable"].Result as TextAsset);
        recruitmentReplacementTable = CSVReader.SplitTextAsset(releasehandles["RecruitmentReplacementTable"].Result as TextAsset);
        eventEffectInfoList = CSVReader.SplitTextAsset(releasehandles["EventEffectTable"].Result as TextAsset);
        expRequirementTable = CSVReader.SplitTextAsset(releasehandles["ExpRequirementTable"].Result as TextAsset);
        maxLevelTable = CSVReader.SplitTextAsset(releasehandles["MaxLevelTable"].Result as TextAsset);
        
        LoadAllData();
        FixMissionTable(CSVReader.SplitTextAsset(releasehandles["MissionInfoTable"].Result as TextAsset));
        AppendStringTable(CSVReader.SplitTextAsset(releasehandles["StringTable_Desc"].Result as TextAsset, false), "StringTable_Desc");
        AppendStringTable(CSVReader.SplitTextAsset(releasehandles["StringTable_Event"].Result as TextAsset, false), "StringTable_Event");
        AppendStringTable(CSVReader.SplitTextAsset(releasehandles["StringTable_Proper"].Result as TextAsset, false), "StringTable_Proper");
        AppendStringTable(CSVReader.SplitTextAsset(releasehandles["StringTable_UI"].Result as TextAsset, false), "StringTable_UI");

        ReleaseAddressable(releasehandles);
        releasehandles.Clear();
        progress.CompleteProgress();
    }

    private void AppendStringTable(List<Dictionary<string, object>> rawData, string tableName)
    {
        int count = rawData.Count;
        for (int i = 0; i < count; i++)
        {
            var copy = rawData[i];
            string id = $"{rawData[i]["ID"]}".ToLower();
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
        string modifyKey = key.ToLower();
        if (stringTable.ContainsKey(modifyKey))
            return $"{stringTable[modifyKey][languageKey]}";
        else
        {
            Logger.Debug($"Load fail to string table. key [{modifyKey}]");
            return $"Load fail to string table. key [{modifyKey}]";
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
        sb.AppendLine($"Inventory;{JsonUtility.ToJson(inventoryData)}");

        foreach (var hero in myHeroes)
        {
            LiveData data = hero.Value.GetComponent<CharacterDataBundle>().data;
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
                }
                else
                {
                    Logger.Debug($"Load failed {heroName}");
                }
            }
            else if(id.Contains("Inventory"))
            {
                inventoryData = JsonUtility.FromJson<InventoryData>(contents);
            }
        }
        SetHeroesActive(false);
    }

    public void SetHeroesActive(bool value)
    {
        foreach (var character in myHeroes)
        {
            character.Value.SetActive(value);
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

        GameObject newHero = Instantiate(heroDatabase[index], heroSpawnTransform);
        string key = newHero.name;
        myHeroes.Add(key, newHero);
        newHero.SetActive(false);
        return newHero;
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
        fitPropertyFlags.Clear();
        for (int i = 0; i < 3; i++)
        {
            battleGroups.Add(null);
            fitPropertyFlags.Add(false);
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

        if (stateIconSprites.ContainsKey(address))
        {
            return stateIconSprites[address];
        }

        if (itemSprites.ContainsKey(address))
        {
            return itemSprites[address];
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