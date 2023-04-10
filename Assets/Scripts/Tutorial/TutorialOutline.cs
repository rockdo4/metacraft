using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorialOutline : MonoBehaviour
{
    public GameObject originalButton;

    public Transform cumtomPosHolder;

    public bool isRed;

    public bool useCustomPos;
    public bool NeedAdjustPos { get; set; } = true;
    
    public void AdjustOutlinePos()
    {
        if (useCustomPos)
        {
            transform.position = cumtomPosHolder.position;
            return;
        }

        if (originalButton == null || !NeedAdjustPos)
            return;

        var buttonCenter = originalButton.GetComponent<RectTransform>().rect.center;
        transform.position = originalButton.transform.TransformPoint(buttonCenter);
    }    
    private void OnEnable()
    {
        UpdatePosAndPanels();
    }

    private void OnDisable()
    {
        if (isRed)
            TutorialBlockPanels.Instance.gameObject.SetActive(false);
        else
            TutorialNoneBlockPanels.Instance.gameObject.SetActive(false);
    }

    public void UpdatePosAndPanels()
    {
        AdjustOutlinePos();

        var image = GetComponent<Image>();

        if (isRed)
            TutorialBlockPanels.Instance.SetPanelsSurroundTarget(image);
        else
            TutorialNoneBlockPanels.Instance.SetPanelsSurroundTarget(image);
    }

    public void SetActiveOutline(bool active)
    {
        gameObject.SetActive(active);
    }

    public void AddEventOriginalButton(UnityAction action)
    {
        if (!isRed || originalButton == null)
        {
            return;
        }

        if (originalButton.TryGetComponent(out Button button))
        {
            if (originalButton.name == "ExitButton")
                return;

            button.onClick.AddListener(action);
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
