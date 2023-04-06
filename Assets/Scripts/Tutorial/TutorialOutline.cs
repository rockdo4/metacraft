using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorialOutline : MonoBehaviour
{
    private RectTransform rectTr;
    public Button originalButton;

    private void Start()
    {
        if (originalButton == null)
            return;

        RectTransform originalRectTr = originalButton.GetComponent<RectTransform>();

        rectTr = GetComponent<RectTransform>();
        rectTr.anchoredPosition = originalRectTr.anchoredPosition;
    }

    public void SetActiveOutline(bool active)
    {
        gameObject.SetActive(active);
    }

    public void AddEventOriginalButton(UnityAction action)
    {
        if (originalButton == null)
            return;

        originalButton.onClick.AddListener(action);
    }
    public void RemoveEventOriginalButton(UnityAction action)
    {
        if (originalButton == null)
            return;

        originalButton.onClick.RemoveListener(action);
    }
    public void SetRectTrPos(Vector2 anchoredPosition)
    {
        rectTr.anchoredPosition = anchoredPosition;
    }
}
