
using UnityEngine;

public class RecruitmentClick : MonoBehaviour
{
    public UIManager uiMgr;

    public void OnMouseDown()
    {
        uiMgr.ShowView(7);
        Logger.Debug("RecruitmentClick");
    }
}
