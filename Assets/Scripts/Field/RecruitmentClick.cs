
using UnityEngine;
using UnityEngine.EventSystems;

public class RecruitmentClick : MonoBehaviour
{
    public UIManager uiMgr;

    public void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            uiMgr.ShowView(7);
        }
    }
}
