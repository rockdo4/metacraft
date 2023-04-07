using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorialOutline : MonoBehaviour
{   
    private RectTransform rectTr;
    public GameObject originalButton;

    public bool isRed;
    
    private void Awake()
    {   
        rectTr = GetComponent<RectTransform>();        
    }
    private void AdjustOutlinePos()
    {
        if (originalButton == null)
            return;

        var buttonCenter = originalButton.GetComponent<RectTransform>().rect.center;
        transform.position = originalButton.transform.TransformPoint(buttonCenter);
    }

    private void OnEnable()
    {
        AdjustOutlinePos();

        var image = GetComponent<Image>();

        if(isRed)
            TutorialBlockPanels.Instance.SetPanelsSurroundTarget(image);
        else
            TutorialNoneBlockPanels.Instance.SetPanelsSurroundTarget(image);
    }
    private void OnDisable()
    {        
        if(isRed)
            TutorialBlockPanels.Instance.gameObject.SetActive(false);
        else
            TutorialNoneBlockPanels.Instance.gameObject.SetActive(false);
    }

    public void SetActiveOutline(bool active)
    {
        gameObject.SetActive(active);
    }

    public void AddEventOriginalButton(UnityAction action)
    {
        Logger.Debug(name + "Add");
        if (!isRed || originalButton == null)
        {
            return;
        }

        if (originalButton.TryGetComponent(out Button button))
        {
            button.onClick.AddListener(action);
            Logger.Debug(name + "Add Success");
        }
    }
    public void RemoveEventOriginalButton(UnityAction action)
    {
        Logger.Debug(name + "Remove");
        if (!isRed || originalButton == null)
        {
            return;
        }

        if (originalButton.TryGetComponent(out Button button))
        {
            button.onClick.RemoveListener(action);
            Logger.Debug(name + "Remove Success");
        }
    }
    public void SetRectTrPos(RectTransform rect)
    {
        rectTr = GetComponent<RectTransform>();
        //rectTr.anchoredPosition = rect.anchoredPosition;
        //rectTr.pivot = rect.pivot;

        //rectTr.anchoredPosition = rect.anchoredPosition;
        //rectTr.anchorMin = rect.anchorMin;
        //rectTr.anchorMax = rect.anchorMax;
        //rectTr.pivot = rect.pivot;

        //Vector3 localPos = rectTr.localPosition;
        //localPos += rect.localPosition - rectTr.localPosition;
        //rectTr.localPosition = localPos;
    }
}
