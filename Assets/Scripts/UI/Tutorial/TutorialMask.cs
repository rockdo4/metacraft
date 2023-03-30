using System;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMask : MonoBehaviour
{
    public Sprite circleSprite;
    public Image maskImage;

    public RectTransform back;
    public RectTransform mask;

    public Button maskClickEvent;
    private RectTransform maskClickEventRectTr;

    private void Awake()
    {
        maskClickEventRectTr = maskClickEvent.GetComponent<RectTransform>();
    }

    public void Setting(Button button)
    {
        SetMaskSize(button.GetComponent<RectTransform>().sizeDelta*1.5f, true);
        ConnectEventToMask(button);
        SetManskPosition(button.GetComponent<RectTransform>().anchoredPosition);

        gameObject.SetActive(true);
    }

    public void Setting(Button button, Vector3 size)
    {
        SetMaskSize(size, true);
        ConnectEventToMask(button);
        SetManskPosition(button.GetComponent<RectTransform>().anchoredPosition);

        gameObject.SetActive(true);
    }
    public void SetMaskSize(Vector3 size, bool isCircle)
    {
        if (isCircle)
        {
            maskImage.sprite = circleSprite;
        }
        else
            maskImage.sprite = null;

        mask.sizeDelta = size;
    }
    public void ConnectEventToMask(Button btn)
    {
        var rectTr = btn.GetComponent<RectTransform>();

        maskClickEvent.onClick.RemoveAllListeners();
        maskClickEvent.onClick.AddListener(() => btn.onClick.Invoke());
        maskClickEvent.onClick.AddListener(() => gameObject.SetActive(false));
        maskClickEventRectTr.anchoredPosition = Vector3.zero;
        maskClickEventRectTr.sizeDelta = rectTr.sizeDelta;
    }

    public void SetManskPosition(Vector3 pos)
    {
        mask.localPosition = pos;
        back.localPosition = -mask.localPosition;
    }
}
