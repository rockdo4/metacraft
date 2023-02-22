using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public SceneIndex currentScene = SceneIndex.Title;
    public List<HeroData> newCharacters = new();
    public Dictionary<string, Sprite> testPortraits = new();

    public override void Awake()
    {
        base.Awake();
        StartCoroutine(LoadAllResources());
    }

    private IEnumerator LoadAllResources()
    {
        List<AsyncOperationHandle> handles = new();

        foreach (var character in newCharacters)
        {
            string address = character.info.resourceAddress;
            Addressables.LoadAssetAsync<Sprite>(address).Completed +=
                (AsyncOperationHandle<Sprite> obj) =>
                {
                    testPortraits.Add(address, obj.Result);
                    handles.Add(obj);
                };
        }

        int count = 0;
        bool loadAll = false;
        // 리소스 로드
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