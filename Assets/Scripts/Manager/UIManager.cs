using System.Collections.Generic;
using UnityEngine;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private ViewManager viewManager;
    public List<View> views;
    public View startingView;

    public PopUpManager popUpManager;

    public Color interactablePanelColor;
    public Color noneInteractablePanelColor;

    private void Awake()
    {
        Instance = this;
        viewManager = gameObject.AddComponent<ViewManager>();        
        SetPopupManager();
        SetPopUpHolders();
        SetViewManager();
    }
    private void SetPopupManager()
    {
        popUpManager = Instantiate(popUpManager, transform);
        popUpManager.SetPanelColor(interactablePanelColor, noneInteractablePanelColor);
    }
    private void SetPopUpHolders()
    {
        List<GameObject> popUpHolders;
        popUpHolders = new(views.Count);
        for (int i = 0; i < popUpHolders.Capacity; i++)
        {
            var popUpHolderIndex = views[i].transform.childCount - 1;
            popUpHolders.Add(views[i].transform.GetChild(popUpHolderIndex).gameObject);
        }

        popUpManager.Init(popUpHolders);
    }
    private void SetViewManager()
    {
        viewManager.Init(startingView, views);
    }
    public void ShowView(int index)
    {
        ShowViewWithNoSound(index);

        //AudioManager.Instance.PlayUIAudio(0);
    }
    public void ShowPopup(int index)
    {
        ShowPopupWithNoSound(index);

        //AudioManager.Instance.PlayUIAudio(0);
    }
    public void ClearPopups()
    {
        ClearPopupsWithNoSound();

        //AudioManager.Instance.PlayUIAudio(0);
    }

    public void ShowViewWithNoSound(int index)
    {
        viewManager.Show(index);
        popUpManager.CurrentViewIndex = index;
    }
    public void ShowPopupWithNoSound(int index)
    {
        popUpManager.ShowPopupInHierarchy(index);
    }
    public void ClearPopupsWithNoSound()
    {
        popUpManager.ClearPopups();
    }
    public void ShowPanelInteractable(bool interactable)
    {
        popUpManager.ShowPanelInteractable(interactable);
    }

}
