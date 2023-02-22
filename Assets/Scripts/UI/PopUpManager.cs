using UnityEngine;
using UnityEngine.UI;

public class PopUpManager : MonoBehaviour
{
    public static PopUpManager Instance { get; private set; }    
    public GameObject popupHolder;
    public GameObject[] popUpsInHierarchy; 

    public GameObject[] prefabs;
    private bool[] isInstanced;

    public GameObject interactablePanel;
    public GameObject noneInteractablePanel;
    
    public Color interactablePanelColor;
    public Color noneInteractablePanelColor;
    private void Awake()
    {
        Instance = this;
        isInstanced = new bool[prefabs.Length];
        SetPanel();        
        popupHolder.transform.SetAsLastSibling();
    }
    private void Start()
    {
        MovePopupsToHolder();
    }
    private void SetPanel()
    {
        interactablePanel = Instantiate(interactablePanel, transform);
        interactablePanel.GetComponent<RawImage>().color = interactablePanelColor;
        noneInteractablePanel = Instantiate(noneInteractablePanel, transform);
        noneInteractablePanel.GetComponent<RawImage>().color = noneInteractablePanelColor;
    }
    private void MovePopupsToHolder()
    {
        for(int i = 0; i < popUpsInHierarchy.Length; i++)
        {
            popUpsInHierarchy[i].transform.parent = popupHolder.transform;
        }
    }
    public void ShowPopupInHierarchy(int index)
    {
        popupHolder.SetActive(true);
        HidePopups();
        popUpsInHierarchy[index].SetActive(true);        
    }
    public void ShowPrefab(int index)
    {
        popupHolder.SetActive(true);
        HidePopups();
        if (!isInstanced[index])
        {
            prefabs[index] = Instantiate(prefabs[index], popupHolder.transform);
            isInstanced[index] = true;
        }
        prefabs[index].SetActive(true);        
    }
    [Tooltip("체크시 상호작용 가능한 패널생성, 반대는 상호작용 불가")]
    public void ShowPanelInteractableOrNot(bool interactable = true)
    {
        interactablePanel.SetActive(interactable);
        noneInteractablePanel.SetActive(!interactable);
    }
    private void HidePanel()
    {
        interactablePanel.SetActive(false);
        noneInteractablePanel.SetActive(false);
    }
    private void HidePopups()
    {
        foreach(var popup in popUpsInHierarchy)
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
