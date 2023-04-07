using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorialOutline : MonoBehaviour
{
    private RectTransform rectTr;
    public GameObject originalButton;

    public bool isRed;

    private Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
        rectTr = GetComponent<RectTransform>();
    }
    private void AdjustOutlinePos()
    {
        if (originalButton == null)
            return;

        RectTransform originalRectTr = originalButton.GetComponent<RectTransform>();
        //Vector2 pivotOffset = new Vector2(originalRectTr.rect.width * (0.5f - originalRectTr.pivot.x), originalRectTr.rect.height * (0.5f - originalRectTr.pivot.y));
        //Vector3 worldPos = originalRectTr.position + new Vector3(pivotOffset.x, pivotOffset.y, 0);
        //Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(originalRectTr.parent.GetComponent<RectTransform>(), screenPos, Camera.main, out Vector2 localPos);
        //Vector2 center = localPos + originalRectTr.rect.center;

        //rectTr = GetComponent<RectTransform>();

        //rectTr.localPosition = center;
        rectTr.anchoredPosition = originalRectTr.anchoredPosition;
    }

    private void OnEnable()
    {
        AdjustOutlinePos();

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
            return;

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
            return;

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
