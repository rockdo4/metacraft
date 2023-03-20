using UnityEngine;
using UnityEngine.EventSystems;

public class DispatchClick : MonoBehaviour
{
    public UIManager uiMgr;

    void Start()
    {
        addPhysicsRaycaster();
    }

    void addPhysicsRaycaster()
    {
        PhysicsRaycaster physicsRaycaster = GameObject.FindObjectOfType<PhysicsRaycaster>();
        if (physicsRaycaster == null)
        {
            Camera.main.gameObject.AddComponent<PhysicsRaycaster>();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        uiMgr.ShowView(6);
    }
}