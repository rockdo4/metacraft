
using UnityEngine;

public class StoreClick : MonoBehaviour
{
    public UIManager uiMgr;

    public void OnMouseDown()
    {
        uiMgr.ShowView(5);
        Logger.Debug("StoreClick");
    }
}
