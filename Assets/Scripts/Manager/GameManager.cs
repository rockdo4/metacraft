using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public SceneIndex currentScene = SceneIndex.Title;
    public PlayerData playerData;
    public InventoryData inventoryData;

    // MyData - Craft, Load & Save to this data
    public Dictionary<string, GameObject> myHeroes = new();
    public Transform heroSpawnTransform;

    // Resources - Sprites, TextAsset
    private Dictionary<string, Sprite> sprites = new();
    private Dictionary<string, Dictionary<string, object>> stringTable = new();
    public Dictionary<int, List<Dictionary<string, object>>> missionInfoDifficulty; // 작전 정보 난이도 키 추가
    //public List<Dictionary<string, object>> dispatchInfoList; // 파견 정보
    public List<Dictionary<string, object>> officeInfoList;  // 사무소 레벨별 정보
    public List<Dictionary<string, object>> eventInfoList; // 이벤트 노드 정보
    public List<Dictionary<string, object>> eventEffectInfoList;  // 이벤트 노드
    public List<Dictionary<string, object>> enemyInfoList;
    public List<Dictionary<string, object>> enemySpawnList;
    private int languageIndex = 0; // kor    
    public int LanguageIndex { get { return languageIndex; } }

    public List<Dictionary<string, object>> compensationInfoList; // 보상 정보
    public List<Dictionary<string, object>> itemInfoList; // 아이템 정보
    public List<Dictionary<string, object>> itemBoxList; // 아이템 정보

    public List<Dictionary<string, object>> supplyInfoList; // 보급 노드 정보

    public List<Dictionary<string, object>> recruitmentReplacementTable; // 영입 중복 대체 테이블
    public Dictionary<int, int> expRequirementTable; // 경험치 요구량 테이블
    public List<Dictionary<string, object>> maxLevelTable; // 최대 레벨 테이블
    public List<Dictionary<string, object>> upgradeTable; // 승급 테이블
    public List<Dictionary<string, object>> tutorialTable; // 튜토리얼 대사 테이블
    public List<Dictionary<string, object>> tutorialIndexTable;

    // Office Select
    public GameObject currentSelectObject; // Hero Info
    public Dictionary<string, object> currentSelectMission; // Mission Select
    public List<int?> battleGroups = new(3) { null, null, null }; // Mission Select -> Battle Scene
    public List<bool> fitPropertyFlags = new(3) { false, false, false };

    // Origin Database - Set Prefab & Scriptable Objects
    public List<GameObject> heroDatabase = new();
    public Image fadeEffect;
    public TextMeshProUGUI loadText;
    public float sceneLoadFadeDuration = 3f;

    public Color currMapColor;
    public List<Color> mapLigthColors;

    //public override void Awake()
    //{
    //    base.Awake();
    //}

    public void SetHeroesOrigin()
    {
        foreach (var elem in myHeroes)
        {
            Utils.CopyPositionAndRotation(elem.Value, heroSpawnTransform);
        }
    }

    public void SetAssets(Dictionary<string, Sprite> sprites, Dictionary<string, Dictionary<string, object>> stringTable)
    {
        this.sprites = sprites;
        this.stringTable = stringTable;
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

    public string GetStringByTable(string key)
    {
        string languageKey = languageIndex switch
        {
            _ => "Contents",
            // 언어 추가를 하게 되면, 컬럼의 이름을 추가하고 languageIndex 값만 바꿔주면 됨
        };
        string modifyKey = key.ToLower();
        if (stringTable.ContainsKey(modifyKey))
            return $"{stringTable[modifyKey][languageKey]}";
        else
        {
            Logger.Debug($"Load fail to string table. key [{modifyKey}]");
            return modifyKey;
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
        if (currentScene != SceneIndex.Title)
            SaveAllData();
    }

    public void SaveAllData()
    {
        try
        {
            StringBuilder sb = new();
            sb.AppendLine("ID;Contents");
            sb.AppendLine($"PlayerData;{JsonUtility.ToJson(playerData)}");

            foreach (var hero in myHeroes)
            {
                LiveData data = hero.Value.GetComponent<CharacterDataBundle>().data;
                sb.AppendLine($"Hero_{data.name};{JsonUtility.ToJson(data)}");
            }
            sb.AppendLine($"Inventory;{JsonUtility.ToJson(inventoryData)}");
            File.WriteAllText(GetSaveFilePath(), sb.ToString());
            Logger.Debug("Save Success");
        }
        catch (Exception e)
        {
            Logger.Debug($"File save error {e}");
        }
    }

    public bool LoadAllData()
    {
        if (!File.Exists(GetSaveFilePath()))
            return false;

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
            else if (id.Contains("Inventory"))
            {
                inventoryData = JsonUtility.FromJson<InventoryData>(contents);
            }
        }
        SetHeroesActive(false);
        return true;
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
        StartCoroutine(ChangeSceneFadeEffect(sceneIdx, sceneLoadFadeDuration));
    }

    private IEnumerator ChangeSceneFadeEffect(int sceneIdx, float duration)
    {
        Time.timeScale = 1f;

        fadeEffect.gameObject.SetActive(true);
        float timer = 0f;
        float halfDuration = duration * 0.5f;
        Color fadeIn = new(0, 0, 0, 0);
        Color fadeOut = new(0, 0, 0, 1);
        string loadString = "Loading";
        StringBuilder dots = new();

        while (timer < halfDuration)
        {
            fadeEffect.color = Color.Lerp(fadeIn, fadeOut, timer / halfDuration);
            yield return null;
            timer += Time.deltaTime;
        }
        fadeEffect.color = fadeOut;
        AsyncOperation loadSceneHandle = SceneManager.LoadSceneAsync(sceneIdx);
        loadText.gameObject.SetActive(true);
        //SceneManager.LoadScene(sceneIdx);
        currentScene = (SceneIndex)sceneIdx;
        WaitForSeconds wfs = new(duration * 0.1f);
        while (!loadSceneHandle.isDone)
        {
            loadText.text = $"{loadString}{dots}";
            yield return wfs;
            if (dots.Length < 3)
                dots.Append(".");
            else
                dots.Clear();
        }
        loadText.gameObject.SetActive(false);

        while (timer < duration)
        {
            fadeEffect.color = Color.Lerp(fadeOut, fadeIn, (timer - halfDuration) / halfDuration);
            yield return null;
            timer += Time.deltaTime;
        }
        fadeEffect.color = fadeIn;
        fadeEffect.gameObject.SetActive(false);
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
        string lower = address.ToLower();
        if (sprites.ContainsKey(lower))
        {
            return sprites[lower];
        }

        Logger.Debug($"Load sprite fail. address: {address}");
        return null;
    }

    public void AddOfficeExperience(int exp)
    {
        playerData.officeExperience += exp;
        PlayerInfoUpdate(playerData.officeLevel);
    }

    private void PlayerInfoUpdate(int level)
    {
        int needExp = (int)officeInfoList[level - 1]["NeedExp"];

        // Level up
        if (playerData.officeExperience >= needExp)
        {
            playerData.officeExperience -= needExp;
            playerData.officeLevel++;

            playerData.officeImage = (string)officeInfoList[level - 1]["OfficeImage"];
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