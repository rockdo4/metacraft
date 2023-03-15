
using UnityEngine;
using UnityEngine.EventSystems;

public class DispatchClick : MonoBehaviour
{
    public UIManager uiMgr;

    public void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            uiMgr.ShowView(6);
        }
    }
}
