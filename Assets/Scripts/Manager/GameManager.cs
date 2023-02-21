using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public SceneIndex currentScene = SceneIndex.Title;
    public List<Hero> newCharacters = new();

    //public List<CharacterData> characters = new();
    //private List<AsyncOperationHandle> heroTableHandles = new();

    //public void LoadAddressable(string address)
    //{
    //    AsyncOperationHandle ctlHandle;

    //    Addressables.LoadAssetAsync<TextAsset>(address).Completed +=
    //        (AsyncOperationHandle<TextAsset> ctl) =>
    //        {
    //            ctlHandle = ctl;
    //            List<Dictionary<string, object>> characterTableList = CSVReader.SplitTextAsset(ctl.Result);
    //            int characterCount = characterTableList.Count;

    //            int count = 0;
    //            foreach (var item in characterTableList)
    //            {
    //                Addressables.LoadAssetAsync<TextAsset>($"{item["Name"]}.json").Completed +=
    //                    (AsyncOperationHandle<TextAsset> eachTable) =>
    //                    {
    //                        heroTableHandles.Add(eachTable);
    //                        characters.Add(JsonUtility.FromJson<CharacterData>(eachTable.Result.text));
    //                        Logger.Debug($"Wait {characters.Count}/{characterCount}");
    //                    };
    //                count++;
    //            }
    //            Addressables.Release(ctlHandle);
    //            StartCoroutine(CoWaitForLoadResource(characterCount));
    //        };
    //}

    //private IEnumerator CoWaitForLoadResource(int characterCount)
    //{
    //    while (characterCount != characters.Count)
    //    {
    //        Logger.Debug($"co Wait {characters.Count}/{characterCount}");
    //        yield return null;
    //    }
    //    Logger.Debug($"co Success {characters.Count}/{characterCount}");
    //    ReleaseAddressable();
    //}

    //public void ReleaseAddressable()
    //{
    //    foreach (var handle in heroTableHandles)
    //    {
    //        Addressables.Release(handle);
    //    }
    //    heroTableHandles.Clear();
    //    heroTableHandles.Capacity = 0;
    //}

    public override void Awake()
    {
        base.Awake();
        //LoadAddressable("CharacterList");
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
    //private void OnApplicationQuit()
    // ����� �ڵ� ������ �� �ְ� ��
    // sls.SaveSequence();
    //ReleaseAddressable();

    public void LoadScene(int sceneIdx)
    {
        SceneManager.LoadScene(sceneIdx);
        currentScene = (SceneIndex)sceneIdx;
    }
}