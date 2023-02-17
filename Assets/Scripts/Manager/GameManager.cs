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
        sls.SaveSequence();
    }

    public void LoadScene(int sceneIdx)
    {
        SceneManager.LoadScene(sceneIdx);
        currentScene = (SceneIndex)sceneIdx;
    }
}