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
        // 텍스트 리소스 로드
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
}