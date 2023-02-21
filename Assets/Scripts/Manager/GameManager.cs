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

    //private readonly SaveLoadSystem sls;
    //private void OnApplicationQuit()
    // 종료시 자동 저장할 수 있게 함
    // sls.SaveSequence();
    //ReleaseAddressable();

    public void LoadScene(int sceneIdx)
    {
        SceneManager.LoadScene(sceneIdx);
        currentScene = (SceneIndex)sceneIdx;
    }
}