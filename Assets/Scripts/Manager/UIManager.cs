using System.Collections.Generic;
using UnityEngine;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private ViewManager viewManager;
    public List<View> views;
    public View startingView;    

    public PopUpManager popUpManager;
    private List<GameObject> popUpHolders;
    private void Awake()
    {
        Instance = this;
        SetViewManager();
        popUpHolders = new(views.Count);
        SetPopUpHolders();
        InitManagers();
    }
    private void SetViewManager()
    {
        GameObject viewMgr = new GameObject("VeiwManager");
        viewMgr.transform.parent = transform;
        viewMgr.AddComponent<ViewManager>();
        viewManager = viewMgr.GetComponent<ViewManager>();
    }
    private void SetPopUpHolders()
    {
        for (int i = 0; i < popUpHolders.Capacity; i++)
        {
            var popUpHolderIndex = views[i].transform.childCount - 1;
            popUpHolders.Add(views[i].transform.GetChild(popUpHolderIndex).gameObject);            
        }
    }
    private void InitManagers()
    {
        viewManager.Init(startingView, views);
        popUpManager.Init(popUpHolders);        
    }
    public void ShowView(int index)
    {
        viewManager.Show(index);
        popUpManager.CurrentViewIndex = index;
    }
    public void ShowPopup(int index)
    {
        popUpManager.ShowPopupInHierarchy(index);
    }   
    public void ShowPanelInteractable(bool interactable)
    {
        popUpManager.ShowPanelInteractable(interactable);
    }
    public void ClearPopups()
    {
        popUpManager.ClearPopups();
    }
}
