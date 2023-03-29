using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class AssetLoadProgress : MonoBehaviour
{
    public Image background;
    public Image fill;
    public TextMeshProUGUI text;
    public Button waitUntilClick;
    public GameObject setNicknamePopup;
    public GameManager gm;

    private Dictionary<string, Dictionary<string, object>> stringTable = new();
    private Dictionary<string, Sprite> sprites = new();

    private void Awake()
    {
        StartCoroutine(LoadAllResources());
    }

    public void StartGame()
    {
        if (gm.playerData.playerName.Equals("default name"))
            setNicknamePopup.SetActive(true);
        else
            LoadOfficeScene();
    }

    public void LoadOfficeScene()
    {
        AudioManager.Instance.ChangeBGMFadeCross(1);
        gm.LoadScene((int)SceneIndex.Office);
    }

    public void ChangeNickname(string str)
    {
        gm.playerData.playerName = str;
    }

    private IEnumerator LoadAllResources()
    {
        Dictionary<string, AsyncOperationHandle> releaseHandles = new();
        List<AsyncOperationHandle> unReleaseHandles = new();
        int total = 0;

        // Load TextAssets
        TextAsset ta = Resources.Load<TextAsset>("TextAssetList");
        TextAsset sn = Resources.Load<TextAsset>("SpriteNameList");
        var tableNames = ta.text.Split("\r\n");
        var spriteNames = sn.text.Split("\r\n");

        int count = tableNames.Length;
        for (int i = 0; i < count; i++)
        {
            string key = tableNames[i];
            if (key.Length != 0)
            {
                AsyncOperationHandle<TextAsset> tas = Addressables.LoadAssetAsync<TextAsset>(key);
                releaseHandles.Add(key, tas);
                //tas.Completed +=
                //    (AsyncOperationHandle<TextAsset> obj) =>
                //    {
                //        //Logger.Debug($"{key} load success");
                //    };
                total++;
            }
        }

        // Load Sprites
        count = gm.heroDatabase.Count;
        for (int i = 0; i < count; i++)
        {
            string address = gm.heroDatabase[i].GetComponent<CharacterDataBundle>().originData.name;
            unReleaseHandles.Add(LoadSprite($"icon_{address}"));
            unReleaseHandles.Add(LoadSprite($"illu_{address}"));
            total += 2;
        }

        count = 29; //임시. 나중에 버프 테이블 불러오게 수정할 예정
        for (int i = 1; i <= count; i++)
        {
            unReleaseHandles.Add(LoadSprite($"state{i}"));
            total++;
        }

        // 임시코드
        string[] villains =
        {
            "icon_enemy_demon_villain",
            "icon_enemy_fanatic_villain",
            "icon_enemy_jmc_villain",
            //"icon_enemy_moonlight_boss",
            //"icon_enemy_shintia_boss",
        };

        for (int i = 0; i < 3; i++)
        {
            unReleaseHandles.Add(LoadSprite(villains[i]));
            total++;
        }

        int spriteCount = spriteNames.Length;
        for (int i = 0; i < spriteCount; i++)
        {
            unReleaseHandles.Add(LoadSprite($"{spriteNames[i]}"));
            total++;
        }

        // 스프라이트 리소스 로드 대기
        bool loadAll = false;
        while (!loadAll)
        {
            count = 0;
            loadAll = true;
            foreach (var handle in releaseHandles)
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
                SetProgress(count, total);
                yield return null;
            }

            foreach (var handle in unReleaseHandles)
            {
                if (!handle.IsDone)
                {
                    loadAll = false;
                    break;
                }
                count++;
            }
            SetProgress(count, total);
            yield return null;
        }

        gm.dispatchInfoList = CSVReader.SplitTextAsset(releaseHandles["DispatchInfoTable"].Result as TextAsset);
        gm.officeInfoList = CSVReader.SplitTextAsset(releaseHandles["OfficeTable"].Result as TextAsset);
        gm.eventInfoList = CSVReader.SplitTextAsset(releaseHandles["EventTable"].Result as TextAsset);
        gm.compensationInfoList = CSVReader.SplitTextAsset(releaseHandles["CompensationTable"].Result as TextAsset);
        gm.supplyInfoList = CSVReader.SplitTextAsset(releaseHandles["SupplyTable"].Result as TextAsset);
        gm.itemInfoList = CSVReader.SplitTextAsset(releaseHandles["ItemInfoTable"].Result as TextAsset);
        gm.itemBoxList = CSVReader.SplitTextAsset(releaseHandles["ItemBoxTable"].Result as TextAsset);
        gm.enemyInfoList = CSVReader.SplitTextAsset(releaseHandles["EnemyInfoTable"].Result as TextAsset);
        gm.enemySpawnList = CSVReader.SplitTextAsset(releaseHandles["EnemySpawnTable"].Result as TextAsset);
        gm.recruitmentReplacementTable = CSVReader.SplitTextAsset(releaseHandles["RecruitmentReplacementTable"].Result as TextAsset);
        gm.eventEffectInfoList = CSVReader.SplitTextAsset(releaseHandles["EventEffectTable"].Result as TextAsset);
        gm.expRequirementTable = FixExpTable(CSVReader.SplitTextAsset(releaseHandles["ExpRequirementTable"].Result as TextAsset));
        gm.maxLevelTable = CSVReader.SplitTextAsset(releaseHandles["MaxLevelTable"].Result as TextAsset);
        gm.upgradeTable = CSVReader.SplitTextAsset(releaseHandles["UpgradeTable"].Result as TextAsset);
        //tutorialTextTable = CSVReader.SplitTextAsset(releasehandles["TutorialTextTable"].Result as TextAsset);

        bool saveFileExist = gm.LoadAllData();
        FixMissionTable(CSVReader.SplitTextAsset(releaseHandles["MissionInfoTable"].Result as TextAsset));
        AppendStringTable(CSVReader.SplitTextAsset(releaseHandles["StringTable_Desc"].Result as TextAsset, false), "StringTable_Desc");
        AppendStringTable(CSVReader.SplitTextAsset(releaseHandles["StringTable_Event"].Result as TextAsset, false), "StringTable_Event");
        AppendStringTable(CSVReader.SplitTextAsset(releaseHandles["StringTable_Proper"].Result as TextAsset, false), "StringTable_Proper");
        AppendStringTable(CSVReader.SplitTextAsset(releaseHandles["StringTable_UI"].Result as TextAsset, false), "StringTable_UI");

        ReleaseAddressable(releaseHandles);
        releaseHandles.Clear();
        gm.SetAssets(sprites, stringTable);
        CompleteProgress();

        if (!saveFileExist)
            gm.CreateNewHero("hero_egostick");
    }

    public void ReleaseAddressable(Dictionary<string, AsyncOperationHandle> handles)
    {
        foreach (var elem in handles.Values)
        {
            Addressables.Release(elem);
        }
    }

    private AsyncOperationHandle<Sprite> LoadSprite(string address)
    {
        AsyncOperationHandle<Sprite> itemIconHandle = Addressables.LoadAssetAsync<Sprite>(address);

        itemIconHandle.Completed +=
            (AsyncOperationHandle<Sprite> obj) =>
            {
                Sprite sprite = obj.Result;
                sprites.Add(address, sprite);
            };
        return itemIconHandle;
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

    public void SetProgress(int count, int total)
    {
        float value = (float)count / total;
        fill.fillAmount = value;
        text.text = $"{value * 100f:0.00}%";
    }

    public void CompleteProgress()
    {
        text.text = $"{gm.GetStringByTable("title_load_complete")}";
        fill.fillAmount = 1;
        waitUntilClick.gameObject.SetActive(true);
    }

    // 작전 테이블 난이도 구분
    private void FixMissionTable(List<Dictionary<string, object>> missionInfoList)
    {
        gm.missionInfoDifficulty = new Dictionary<int, List<Dictionary<string, object>>>();
        for (int i = 1; i < 6; i++)
        {
            gm.missionInfoDifficulty.Add(i, new List<Dictionary<string, object>>());
        }
        for (int i = 0; i < missionInfoList.Count; i++)
        {
            switch ((int)missionInfoList[i]["Difficulty"])
            {
                case 1:
                    gm.missionInfoDifficulty[1].Add(missionInfoList[i]);
                    break;
                case 2:
                    gm.missionInfoDifficulty[2].Add(missionInfoList[i]);
                    break;
                case 3:
                    gm.missionInfoDifficulty[3].Add(missionInfoList[i]);
                    break;
                case 4:
                    gm.missionInfoDifficulty[4].Add(missionInfoList[i]);
                    break;
                case 5:
                    gm.missionInfoDifficulty[5].Add(missionInfoList[i]);
                    break;
            }
        }
    }

    private Dictionary<int, int> FixExpTable(List<Dictionary<string, object>> expTable)
    {
        Dictionary<int, int> result = new ();

        int count = expTable.Count;
        for (int i = 0; i < count; i++)
        {
            result.Add((int)expTable[i]["LEVEL"], (int)expTable[i]["NEEDEXP"]);
        }
        
        return result;
    }
}