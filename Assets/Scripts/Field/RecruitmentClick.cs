using UnityEngine;
using UnityEngine.EventSystems;

public class RecruitmentClick : MonoBehaviour, IPointerDownHandler
{
    public UIManager uiMgr;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!EventSystem.current.IsPointerOverGameObject(eventData.pointerId))
        {
            uiMgr.ShowView(7);
        }
    }
}
