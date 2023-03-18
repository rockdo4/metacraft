
using UnityEngine;
using UnityEngine.EventSystems;

public class OfficeClick : MonoBehaviour
{
    public UIManager uiMgr;

    public void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        uiMgr.ShowView(2);
                    }
                }
            }
        }
    }
}
