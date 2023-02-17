using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private SceneIndex currentScene = SceneIndex.Title;
    private SaveLoadSystem sls;

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
        sls.SaveSequence();
    }

    public void LoadScene(int sceneIdx)
    {
        SceneManager.LoadScene(sceneIdx);
        currentScene = (SceneIndex)sceneIdx;
    }
}