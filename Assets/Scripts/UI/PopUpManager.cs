using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class PopUpManager : MonoBehaviour
{
    public static PopUpManager Instance { get; private set; }    
    public GameObject popupHolder;
    public GameObject[] popUpsInCanvas;

    public GameObject[] prefabs;
    private bool[] isInstaced;

    public GameObject interactablePanel;
    public GameObject noneInteractablePanel;
    
    private void Awake()
    {
        Instance = this;
        isInstaced = new bool[prefabs.Length];
        popupHolder.transform.SetAsLastSibling();
    }
    public void Show(int index)
    {
        popupHolder.SetActive(true);
        HidePopups();
        popUpsInCanvas[index].SetActive(true);        
    }
    public void ShowPrefab(int index)
    {
        popupHolder.SetActive(true);
        HidePopups();
        if (!isInstaced[index])
        {
            prefabs[index] = Instantiate(prefabs[index], popupHolder.transform);
            isInstaced[index] = true;
        }
        prefabs[index].SetActive(true);        
    }
    [Tooltip("체크시 상호작용 가능한 패널생성, 반대는 상호작용 불가")]
    public void ShowPanelInteractableOrNot(bool interactable = true)
    {
        interactablePanel.SetActive(interactable);
        noneInteractablePanel.SetActive(!interactable);
    }
    public void HidePanel()
    {
        interactablePanel.SetActive(false);
        noneInteractablePanel.SetActive(false);
    }
    private void HidePopups()
    {
        foreach(var popup in popUpsInCanvas)
        {
            popup.gameObject.SetActive(false);
        }
        foreach(var prefab in prefabs)
        {
            prefab.gameObject.SetActive(false);
        }
    }
    public void ClearPopupWindow()
    {
        popupHolder.SetActive(false);
        HidePanel();
    }
}
