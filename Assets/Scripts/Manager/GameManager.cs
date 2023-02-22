using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public SceneIndex currentScene = SceneIndex.Title;
    public List<CharacterDataBundle> characterTable = new();
    public Dictionary<string, Sprite> testPortraits = new();
    public GameObject testCharacterPrefab;

    private List<Dictionary<string, object>> characterList;

    public override void Awake()
    {
        base.Awake();
        StartCoroutine(LoadAllResources());
    }

    private IEnumerator LoadAllResources()
    {
        var cl = Addressables.LoadAssetAsync<TextAsset>("CharacterList");

        cl.Completed +=
                (AsyncOperationHandle<TextAsset> obj) =>
                {
                    characterList = CSVReader.SplitTextAsset(obj.Result);
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
        foreach (var character in characterTable)
        {
            string address = character.data.name;
            Addressables.LoadAssetAsync<Sprite>(address).Completed +=
                (AsyncOperationHandle<Sprite> obj) =>
                {
                    testPortraits.Add(address, obj.Result);
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
            Logger.Debug($"progress {count} / {handles.Count}");
            yield return null;
        }
        Logger.Debug("Load All Resources");
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