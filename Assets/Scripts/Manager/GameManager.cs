using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.VisualScripting;
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
    public List<Dictionary<string, object>> missionInfoList; // 작전 정보
    public Dictionary<int, List<Dictionary<string, object>>> missionInfoDifficulty; // 작전 정보 난이도 키 추가
    public List<Dictionary<string, object>> dispatchInfoList; // 파견 정보
    public List<Dictionary<string, object>> officeInfoList;  // 사무소 레벨별 정보
    public List<Dictionary<string, object>> eventInfoList; // 이벤트 노드 정보
    public List<Dictionary<string, object>> eventEffectInfoList;
    public Dictionary<string, List<Dictionary<string, List<string>>>> eventEffectTagInfoList;
    public Dictionary<string, List<Dictionary<string, List<string>>>> eventEffectNoTagInfoList;
    public List<Dictionary<string, object>> stringTable;

    public List<Dictionary<string, object>> compensationInfoList; // 보상 정보

    public List<Dictionary<string, object>> supplyInfoList; // 보급 노드 정보

    // Office Select
    public GameObject currentSelectObject; // Hero Info
    public Dictionary<string, object> currentSelectMission; // Mission Select
    public List<int?> battleGroups = new(3) { null, null, null }; // Mission Select -> Battle Scene
    // Dispatch Select

    public List<Effect> effects; // 사용할 이펙트들
    public Color currMapColor;
    public List<Color> mapLigthColors;

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
        Dictionary<string, AsyncOperationHandle> handles = new();

        // Resources 테이블로 뺄 예정
        List<string> tableNames = new()
        {
            "MissionInfoTable",
            "EventEffectTable",
            "DispatchInfoTable",
            "OfficeTable",
            "EventTable",
            "CompensationTable",
            "SupplyTable",
        };

        // Load TextAssets
        int count = tableNames.Count;
        for (int i = 0; i < count; i++)
            handles.Add(tableNames[i], Addressables.LoadAssetAsync<TextAsset>(tableNames[i]));

        // Load Sprites
        List<string> heroNames = new();

        count = heroDatabase.Count;
        for (int i = 0; i < count; i++)
        {
            string address = heroDatabase[i].GetComponent<CharacterDataBundle>().originData.name;
            string iconAddress = $"Icon_{address}";
            string IllurAddress = $"Illu_{address}";
            handles.Add(iconAddress, Addressables.LoadAssetAsync<Sprite>(iconAddress));
            handles.Add(IllurAddress, Addressables.LoadAssetAsync<Sprite>(IllurAddress));
            heroNames.Add(address);
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

        missionInfoList = CSVReader.SplitTextAsset(handles["MissionInfoTable"].Result as TextAsset);
        eventEffectInfoList = CSVReader.SplitTextAsset(handles["EventEffectTable"].Result as TextAsset);
        dispatchInfoList = CSVReader.SplitTextAsset(handles["DispatchInfoTable"].Result as TextAsset);
        officeInfoList = CSVReader.SplitTextAsset(handles["OfficeTable"].Result as TextAsset);
        eventInfoList = CSVReader.SplitTextAsset(handles["EventTable"].Result as TextAsset);
        compensationInfoList = CSVReader.SplitTextAsset(handles["CompensationTable"].Result as TextAsset);
        supplyInfoList = CSVReader.SplitTextAsset(handles["SupplyTable"].Result as TextAsset);

        count = heroNames.Count;
        for (int i = 0; i < count; i++)
        {
            string iconKey = $"Icon_{heroNames[i]}";
            iconSprites.Add(iconKey, handles[iconKey].Result as Sprite);
            string illuKey = $"Illu_{heroNames[i]}";
            illustrationSprites.Add(illuKey, handles[illuKey].Result as Sprite);
        }

        ReleaseAddressable(handles);
        handles.Clear();

        LoadAllData();
        FixMissionTable();
        FixEventEffectTable();
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
        eventEffectTagInfoList = new Dictionary<string, List<Dictionary<string, List<string>>>>();
        for (int i = 0; i < eventEffectInfoList.Count; i++)
        {
            var midList = new List<Dictionary<string, List<string>>>();
            for (int j = 0; j < 10; j++)
            {
                var smallDic = new Dictionary<string, List<string>>();
                var list = new List<string>();
                string priorityTag = $"PriorityTag{j + 1}";
                string priorityText = $"PriorityText{j + 1}";
                string priorityRewardType = $"PriorityRewardType{j + 1}";
                string priorityReward = $"PriorityReward{j + 1}";
                list.Add(priorityText);
                list.Add(priorityRewardType);
                list.Add(priorityReward);
                smallDic.Add((string)eventEffectInfoList[i][priorityTag], list);
                midList.Add(smallDic);
            }
            eventEffectTagInfoList.Add((string)eventEffectInfoList[i]["ID"], midList);
        }

        eventEffectNoTagInfoList = new Dictionary<string, List<Dictionary<string, List<string>>>>();
        for (int i = 0; i < eventEffectInfoList.Count; i++)
        {
            var midList = new List<Dictionary<string, List<string>>>();
            for (int j = 0; j < 3; j++)
            {
                var smallDic = new Dictionary<string, List<string>>();
                var list = new List<string>();
                string text = $"NormalvalueText{j + 1}";
                string rate = $"Normalvalue{j + 1}";
                string rewardType = $"NormalRewardType{j + 1}";
                string reward = $"NormalReward{j + 1}";
                list.Add(rate);
                list.Add(rewardType);
                list.Add(reward);
                smallDic.Add(eventEffectInfoList[i][text].ToString(), list);
                midList.Add(smallDic);
            }
            eventEffectNoTagInfoList.Add((string)eventEffectInfoList[i]["ID"], midList);
        }
    }

    // 작전 테이블 난이도 구분
    private void FixMissionTable()
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