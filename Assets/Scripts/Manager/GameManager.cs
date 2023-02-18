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
            // �����Ͻðڽ��ϱ� �˾� ���� or �׳� �����ϱ�
            
            Application.Quit();
        }
#endif
        if (Input.GetKeyDown(KeyCode.Home)) // Navigator Home Button
        {
            // Ȩ��ư
        }
        if (Input.GetKeyDown(KeyCode.Menu)) // Navigator Menu Button
        {
            // �޴� ��ư
        }
    }

    private void OnApplicationQuit()
    {
        // ����� �ڵ� ������ �� �ְ� ��
        // sls.SaveSequence();
    }

    public void LoadScene(int sceneIdx)
    {
        SceneManager.LoadScene(sceneIdx);
        currentScene = (SceneIndex)sceneIdx;
    }
}