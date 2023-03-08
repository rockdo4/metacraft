using UnityEngine;

public class ControlUIManagerFlow : MonoBehaviour
{
    public void ShowView(int index)
    {
        UIManager.Instance.ShowView(index);
    }
    public void ShowPopup(int index)
    {
        UIManager.Instance.ShowPopup(index);
    }
    public void ClearPopup()
    {
        UIManager.Instance.ClearPopups();
    }
    public void ShowInteratablePanel(bool interactable)
    {
        UIManager.Instance.ShowPanelInteractable(interactable);
    }
}
