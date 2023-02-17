using UnityEngine;

public class TitleSceneControl : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.Instance.LoadScene((int)SceneIndex.Office);
        }
    }
}
