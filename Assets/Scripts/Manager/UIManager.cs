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

    public AudioClip viewAudio;
    public AudioClip popupAudio;
    public AudioClip clearAudio;

    private AudioSource audioSource;
    private bool hasAudioSource;

    private void Awake()
    {
        Instance = this;
        viewManager = gameObject.AddComponent<ViewManager>();
        hasAudioSource = TryGetComponent(out audioSource);
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
        viewManager.Show(index);
        popUpManager.CurrentViewIndex = index;

        if (hasAudioSource)
            audioSource.PlayOneShot(viewAudio);       
    }
    public void ShowPopup(int index)
    {
        popUpManager.ShowPopupInHierarchy(index);

        if (hasAudioSource)
            audioSource.PlayOneShot(popupAudio);
    }
    public void ShowPanelInteractable(bool interactable)
    {
        popUpManager.ShowPanelInteractable(interactable);
    }
    public void ClearPopups()
    {
        popUpManager.ClearPopups();

        if (hasAudioSource)
            audioSource.PlayOneShot(clearAudio);
    }
}
