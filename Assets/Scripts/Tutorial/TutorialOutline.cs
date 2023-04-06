using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorialOutline : MonoBehaviour
{
    private RectTransform rectTr;
    public Button originalButton;

    private void Start()
    {
        RectTransform originalRectTr = originalButton.GetComponent<RectTransform>();

        rectTr = GetComponent<RectTransform>();
        rectTr.localPosition = originalRectTr.rect.center;
    }

    public void SetActiveOutline(bool active)
    {
        gameObject.SetActive(active);
    }

    public void AddEventOriginalButton(UnityAction action)
    {
        originalButton.onClick.AddListener(action);
    }
    public void RemoveEventOriginalButton(UnityAction action)
    {
        originalButton.onClick.RemoveListener(action);
    }
}
