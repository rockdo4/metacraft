using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public SceneIndex currentScene = SceneIndex.Title;

    // Origin Database - Set Prefab & Scriptable Objects
    public List<GameObject> heroDatabase = new();

    // MyData - Craft, Load & Save to this data
    public List<GameObject> myHeroes = new();
    public Transform heroSpawnTransform;

    // Resources - Sprites, TextAsset + (Scriptable Objects, Sound etc)
    public Dictionary<string, Sprite> iconSprites = new();
    public Dictionary<string, Sprite> illustrationSprites = new();
    public List<Dictionary<string, object>> missionInfoList;

    // Office Select
    public GameObject currentSelectObject; // Hero Info
    public List<int?> battleGroups = new (3) { null, null, null }; // Mission select -> Battle Scene

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

    private IEnumerator LoadAllResources()
    {
        // 텍스트 리소스 로드
        var mit = Addressables.LoadAssetAsync<TextAsset>("MissionInfoTable");

        mit.Completed +=
                (AsyncOperationHandle<TextAsset> obj) =>
                {
                    missionInfoList = CSVReader.SplitTextAsset(obj.Result);
                    Addressables.Release(obj);
                };

        List<AsyncOperationHandle> handles = new();

        List<string> iconAddress = new()
        {
            "Icon_다인",
            "Icon_신하루",
            "Icon_이수빈",
            "Icon_한서은",
        };

        List<string> illustrationAddress = new()
        {
            "Illur_다인",
            "Illur_신하루",
            "Illur_이수빈",
            "Illur_한서은",
        };

        foreach (string icon in iconAddress)
        {
            Addressables.LoadAssetAsync<Sprite>(icon).Completed +=
                (AsyncOperationHandle<Sprite> obj) =>
                {
                    iconSprites.Add(icon, obj.Result);
                    handles.Add(obj);
                };
        }

        foreach (string illustration in illustrationAddress)
        {
            Addressables.LoadAssetAsync<Sprite>(illustration).Completed +=
                (AsyncOperationHandle<Sprite> obj) =>
                {
                    illustrationSprites.Add(illustration, obj.Result);
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
        if (Input.GetKeyDown(KeyCode.Home)) // Navigator Home Button
        {
            // 홈버튼
        }
        if (Input.GetKeyDown(KeyCode.Menu)) // Navigator Menu Button
        {
            // 메뉴 버튼
        }
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

    public int GetHeroIndex(GameObject hero)
    {
        int count = myHeroes.Count;
        for (int i = 0; i < count; i++)
        {
            string tableName = myHeroes[i].GetComponent<CharacterDataBundle>().data.name;
            string selectHeroName = hero.GetComponent<CharacterDataBundle>().data.name;
            if (tableName.Equals(selectHeroName))
                return i;
        }
        return -1;
    }

    public Sprite GetSpriteByAddress(string address)
    {
        if (iconSprites.ContainsKey(address))
            return iconSprites[address];

        if (illustrationSprites.ContainsKey(address))
            return illustrationSprites[address];

        return null;
    }
}