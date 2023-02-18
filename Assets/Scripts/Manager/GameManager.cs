using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public SceneIndex currentScene = SceneIndex.Title;
    //private readonly SaveLoadSystem sls;
    private readonly string heroTableLocalPath = "/Tables/HeroTable.json";

    private string MakeFullPath(string localPath)
    {
        return $"{Application.dataPath}{localPath}";
    }

    public override void Awake()
    {
        string str = File.ReadAllText(MakeFullPath(heroTableLocalPath));
        //string jsonFromFile = JsonUtility.FromJson<string>(str);
        Logger.Debug(str);
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

    private void OnApplicationQuit()
    {
        // 종료시 자동 저장할 수 있게 함
        // sls.SaveSequence();
    }

    public void LoadScene(int sceneIdx)
    {
        SceneManager.LoadScene(sceneIdx);
        currentScene = (SceneIndex)sceneIdx;
    }
}