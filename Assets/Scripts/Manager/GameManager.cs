using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private SceneIndex currentScene = SceneIndex.Title;
    private SaveLoadSystem sls;

    private void Update()
    {
#if UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // �����Ͻðڽ��ϱ� �˾� ���� or �׳� �����ϱ�
            
            Application.Quit();
        }
#endif
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