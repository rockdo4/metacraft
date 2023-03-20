
using UnityEngine;
using UnityEngine.EventSystems;

public class StoreClick : MonoBehaviour
{
    public UIManager uiMgr;

    public void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        uiMgr.ShowView(5);
                    }
                }
            }
        }
    }
}
