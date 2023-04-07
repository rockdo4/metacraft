using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorialOutline : MonoBehaviour
{   
    public GameObject originalButton;

    public bool isRed;
    
    public void AdjustOutlinePos()
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
            if (originalButton.name == "ExitButton")
                return;

            button.onClick.AddListener(action);
            Logger.Debug(name + "Add Success");
        }
    }
    public void RemoveEventOriginalButton(UnityAction action)
    {
        if (!isRed || originalButton == null)
        {
            return;
        }

        if (originalButton.TryGetComponent(out Button button))
        {
            button.onClick.RemoveListener(action);
        }
    }
}
