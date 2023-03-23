using UnityEngine;

public class LoadSceneButton : MonoBehaviour
{
    public void LoadSceneByIndex(int index)
    {        
        GameManager.Instance.LoadScene(index);
    }
}