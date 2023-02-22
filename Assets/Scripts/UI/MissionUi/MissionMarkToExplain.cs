using UnityEngine;

public class MissionMarkToExplain : MonoBehaviour
{
    public void OpenMissionExplainPopup()
    {
        UIManager.Instance.ShowPopup(2);
        UIManager.Instance.ShowPanelInteractable(true);
    }
}
