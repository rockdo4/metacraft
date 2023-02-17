using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private SceneIndex currentScene = SceneIndex.Title;

    public void LoadScene(int sceneIdx)
    {
        SceneManager.LoadScene(sceneIdx);
        currentScene = (SceneIndex)sceneIdx;
    }
}