
using UnityEngine;

public class DispatchClick : MonoBehaviour
{
    public UIManager uiMgr;

    public void OnMouseDown()
    {
        uiMgr.ShowView(6);
        Logger.Debug("DispatchClick");
    }
}
