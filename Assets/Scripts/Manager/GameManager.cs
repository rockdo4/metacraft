using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>, IUseAddressable
{
    public SceneIndex currentScene = SceneIndex.Title;
    public List<CharacterData> characters = new();

    private AsyncOperationHandle handle;
    private TextAsset textAsset;

    public AsyncOperationHandle Handle
    {
        get => handle;
        set => handle = value;
    }

    public void LoadAddressable(string address)
    {
        Addressables.LoadAssetAsync<TextAsset>(address).Completed +=
            (AsyncOperationHandle<TextAsset> obj) =>
            {
                handle = obj;
                textAsset = obj.Result;
                List<Dictionary<string, object>> characterTableList = CSVReader.SplitTextAsset(textAsset);

                int count = 0;
                foreach (var item in characterTableList)
                {
                    Addressables.LoadAssetAsync<TextAsset>($"{item["Name"]}.json").Completed +=
                        (AsyncOperationHandle<TextAsset> obj) =>
                        {
                            characters.Add(JsonUtility.FromJson<CharacterData>(obj.Result.text));
                        };
                    count++;
                }
            };
    }

    public void ReleaseAddressable()
    {
        Addressables.Release(handle);
    }

    public override void Awake()
    {
        base.Awake();
        LoadAddressable("CharacterList");
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

    //private readonly SaveLoadSystem sls;
    private void OnApplicationQuit()
    {
        // ����� �ڵ� ������ �� �ְ� ��
        // sls.SaveSequence();
        ReleaseAddressable();
    }

    public void LoadScene(int sceneIdx)
    {
        SceneManager.LoadScene(sceneIdx);
        currentScene = (SceneIndex)sceneIdx;
    }
}