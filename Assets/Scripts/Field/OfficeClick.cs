
using UnityEngine;

public class OfficeClick : MonoBehaviour
{
    public UIManager uiMgr;

    public void OnMouseDown()
    {
        uiMgr.ShowView(2);
        Logger.Debug("Clicked");
    }
}
