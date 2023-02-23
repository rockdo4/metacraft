using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public SceneIndex currentScene = SceneIndex.Title;
    public List<GameObject> characterTable = new();
    //public Dictionary<string, Sprite> testPortraits = new();
    public Dictionary<string, Sprite> iconSprites = new();
    public Dictionary<string, Sprite> illustrationSprites = new();

    private List<Dictionary<string, object>> characterList;
    public List<Dictionary<string, object>> missionInfoList;

    public Sprite GetSpriteByAddress(string address)
    {
        if (iconSprites.ContainsKey(address))
            return iconSprites[address];

        if (illustrationSprites.ContainsKey(address))
            return illustrationSprites[address];

        return null;
    }

    public override void Awake()
    {
        base.Awake();
        StartCoroutine(LoadAllResources());
    }

    private void Start()
    {
        foreach (var character in characterTable)
        {
            character.SetActive(false);
        }
    }

    private IEnumerator LoadAllResources()
    {
        var cl = Addressables.LoadAssetAsync<TextAsset>("CharacterList");
        var mit = Addressables.LoadAssetAsync<TextAsset>("MissionInfoTable");

        cl.Completed +=
                (AsyncOperationHandle<TextAsset> obj) =>
                {
                    characterList = CSVReader.SplitTextAsset(obj.Result);
                    Addressables.Release(obj);
                };
        mit.Completed +=
                (AsyncOperationHandle<TextAsset> obj) =>
                {
                    missionInfoList = CSVReader.SplitTextAsset(obj.Result);
                    Addressables.Release(obj);
                };

        bool loadAll = false;
        // �ؽ�Ʈ ���ҽ� �ε�
        while (!loadAll)
        {
            if (cl.IsDone)
                loadAll = true;
            yield return null;
        }

        List<AsyncOperationHandle> handles = new();
        //foreach (var character in characterList)
        //{
        //    string address = (string)character["Name"];
        //    Debug.Log(address);
        //    Addressables.LoadAssetAsync<Sprite>(address).Completed +=
        //        (AsyncOperationHandle<Sprite> obj) =>
        //        {
        //            testPortraits.Add(address, obj.Result);
        //            handles.Add(obj);
        //        };
        //}

        List<string> iconAddress = new()
        {
            "Icon_����",
            "Icon_���Ϸ�",
            "Icon_�̼���",
            "Icon_�Ѽ���",
        };

        List<string> illustrationAddress = new()
        {
            "Illu_����",
            "Illu_���Ϸ�",
            "Illu_�̼���",
            "Illu_�Ѽ���",
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


        int count = 0;
        loadAll = false;
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
            //Logger.Debug($"progress {count} / {handles.Count}");
            yield return null;
        }
        //Logger.Debug("Load All Resources");
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
    }

    public void LoadScene(int sceneIdx)
    {
        SceneManager.LoadScene(sceneIdx);
        currentScene = (SceneIndex)sceneIdx;
    }
}