using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public SceneIndex currentScene = SceneIndex.Title;
    public List<Dictionary<string, object>> filePathList;
    public List<CharacterData> characters = new ();

    private void InitCharacterTable()
    {
        FilePathList index = FilePathList.CharacterList;

        List<Dictionary<string, object>> characterTableList = CSVReader.ReadByPath(GetFilePathByIndex(index));

        int count = 0;
        foreach (var item in characterTableList)
        {
            string path = $"{Application.dataPath}/{filePathList[(int)index]["Path"]}/{item["Name"]}.json";
            string test = File.ReadAllText(path);
            characters.Add(JsonUtility.FromJson<CharacterData>(test));
            characters[count].PrintState();
            count++;
        }
    }

    public override void Awake()
    {
        base.Awake();
        filePathList = CSVReader.ReadByPath(GetTableRootPath());
        InitCharacterTable();
        Debug.Log("2");
    }

    private void Update()
    {
#if UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Escape)) // Navigator Back Button
        {
            // 종료하시겠습니까 팝업 띄우기 or 그냥 종료하기
            
            Application.Quit();
        }
#endif
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
    //{
    //    // 종료시 자동 저장할 수 있게 함
    //    // sls.SaveSequence();
    //}

    public void LoadScene(int sceneIdx)
    {
        SceneManager.LoadScene(sceneIdx);
        currentScene = (SceneIndex)sceneIdx;
    }

    private string GetTableRootPath()
    {
        return $"{Application.dataPath}/Tables/PathList.csv";
    }

    public string GetFilePathByIndex(FilePathList index)
    {
        if (index == FilePathList.None || index == FilePathList.Count)
        {
            Logger.Error("Check File Index");
            return string.Empty;
        }
        int i = (int)index;
        return $"{Application.dataPath}/{filePathList[i]["Path"]}/{filePathList[i]["File"]}.{filePathList[i]["Extension"]}";
    }
}