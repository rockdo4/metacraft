using UnityEngine;

public class LoadScene : MonoBehaviour
{
    public void DoLoadScene(int index)
    {
        GameManager.Instance.LoadScene(index);
    } 
}
