using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
struct panelHolder
{
    public panelHolder(GameObject interactable, GameObject noneInteractable)
    {
        this.interactable = interactable;
        this.noneInteractable= noneInteractable;
    }
    public GameObject interactable;
    public GameObject noneInteractable;

    public void HidePanels()
    {
        interactable.SetActive(false);
        noneInteractable.SetActive(false);
    }
}
public class PopUpManager : MonoBehaviour
{   
    private List<GameObject> popUpHolders;
    private List<List<GameObject>> popUpsInHierarchy;    

    public GameObject prefabPopupHolder;    
    public GameObject[] prefabs;
    private bool[] isInstanced;

    public GameObject interactablePanel;
    public GameObject noneInteractablePanel;

    public GameObject panelHolder;
    private List<panelHolder> panelHolders;   
    public int CurrentViewIndex { get; set; }
    private void Start()
    {
        isInstanced = new bool[prefabs.Length];
        SetPanel();
        prefabPopupHolder.transform.SetAsLastSibling();
    }
    public void Init(List<GameObject> holders)
    {
        popUpHolders = holders;
        popUpsInHierarchy = new List<List<GameObject>>(popUpHolders.Count);

        for (int i = 0; i < popUpHolders.Count; i++)
        {
            List<GameObject> popUps = new List<GameObject>(popUpHolders[i].transform.childCount);
            for (int j = 0; j < popUps.Capacity; j++)
            {
                popUps.Add(popUpHolders[i].transform.GetChild(j).gameObject);
            }
            popUpsInHierarchy.Add(popUps);
        }
    }
    public void SetPanelColor(Color interactable, Color noneInteractable)
    {        
        interactablePanel.GetComponent<RawImage>().color = interactable;
        noneInteractablePanel.GetComponent<RawImage>().color = noneInteractable;
    }
    private void SetPanel()
    {        
        panelHolders = new(popUpHolders.Count);

        for (int i = 0; i < panelHolders.Capacity;i++)
        {            
            GameObject panelHolder = Instantiate(this.panelHolder, popUpHolders[i].transform);            
            panelHolder.transform.SetAsFirstSibling();
            
            var interactable = Instantiate(interactablePanel, panelHolder.transform);            
            var nonInteractable = Instantiate(noneInteractablePanel, panelHolder.transform);

            panelHolders.Add(new panelHolder(interactable, nonInteractable));
        }
    }
    public void ShowPopupInHierarchy(int index)
    {
        var viewIndex = CurrentViewIndex;
        popUpHolders[viewIndex].SetActive(true);
        HidePopups();
        popUpsInHierarchy[viewIndex][index].SetActive(true);
    }
    public void ShowPrefab(int index)
    {        
        prefabPopupHolder.SetActive(true);
        HidePopups();
        if (!isInstanced[index])
        {
            prefabs[index] = Instantiate(prefabs[index], prefabPopupHolder.transform);
            isInstanced[index] = true;
        }
        prefabs[index].SetActive(true);
    }    
    public void ShowPanelInteractable(bool interactable)
    {
        panelHolders[CurrentViewIndex].interactable.SetActive(interactable);
        panelHolders[CurrentViewIndex].noneInteractable.SetActive(!interactable);
    }
    private void HidePanel()
    {
        panelHolders[CurrentViewIndex].HidePanels();

        interactablePanel.SetActive(false);
        noneInteractablePanel.SetActive(false);
    }
    private void HidePopups()
    {
        var viewIndex = CurrentViewIndex;
        for (int i = 0; i < popUpsInHierarchy[viewIndex].Count; i++) 
        {
            popUpsInHierarchy[viewIndex][i].SetActive(false);
        }
        for(int i = 0; i < prefabs.Length; i++)
        {
            prefabs[i].SetActive(false);
        }
    }
    public void ClearPopups()
    {
        prefabPopupHolder.SetActive(false);
        popUpHolders[CurrentViewIndex].SetActive(false);
        HidePanel();
    }   
}
