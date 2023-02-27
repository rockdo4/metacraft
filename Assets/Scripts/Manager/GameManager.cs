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
    public GameRule gameRule;

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
        // �ؽ�Ʈ ���ҽ� �ε�
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
            "Icon_����",
            "Icon_���Ϸ�",
            "Icon_�̼���",
            "Icon_�Ѽ���",
            "Icon_������",
            "Icon_�����",
            "Icon_������",
            "Icon_������",
            "Icon_���Ÿ�",

        };

        List<string> illustrationAddress = new()
        {
            "Illur_����",
            "Illur_���Ϸ�",
            "Illur_�̼���",
            "Illur_�Ѽ���",
            "Illur_������",
            "Illur_�����",
            "Illur_������",
            "Illur_������",
            "Illur_���Ÿ�",
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
        // ��������Ʈ ���ҽ� �ε�
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
            // �����Ͻðڽ��ϱ� �˾� ���� or �׳� �����ϱ�

            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.Home)) // Navigator Home Button
        {
            // Ȩ��ư
        }
        if (Input.GetKeyDown(KeyCode.Menu)) // Navigator Menu Button
        {
            // �޴� ��ư
        }

        // Test Key
        if (Input.GetKeyDown(KeyCode.L))
        {
            StringBuilder sb = new();
            int count = 0;
            sb.AppendLine("{");
            foreach (var hero in myHeroes)
            {
                LiveData data = hero.GetComponent<CharacterDataBundle>().data;
                count++;
                sb.Append($"\"{data.name}\":\n{JsonUtility.ToJson(data, true)}");
                if (myHeroes.Count != count)
                    sb.AppendLine(",");
            }
            sb.Append("\n}");
            File.WriteAllText($"{Application.persistentDataPath}/saveTest.json", sb.ToString());
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