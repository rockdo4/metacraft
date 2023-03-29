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
    public RectTransform maskClickEventRectTr;

    public void Setting(Button button)
    {
        gameObject.SetActive(true);

        ConnectEventToMask(button);
        SetManskPosition(button);
        SetMaskSize(button.GetComponent<RectTransform>().sizeDelta*1.5f, true);
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
        var btnRectTr = btn.GetComponent<RectTransform>();

        maskClickEvent.onClick.RemoveAllListeners();
        maskClickEvent.onClick.AddListener(() => btn.onClick.Invoke());
        maskClickEvent.GetComponent<RectTransform>().sizeDelta = btnRectTr.sizeDelta;
    }

    public void SetManskPosition(Button button)
    {
        var prevParent = transform.parent;
        int prevParentIdx = transform.GetSiblingIndex();

        transform.parent = button.transform;
        mask.anchoredPosition = Vector3.zero;
        transform.parent = prevParent;
        transform.SetSiblingIndex(prevParentIdx);

        back.anchoredPosition = -mask.anchoredPosition;
    }

    public void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
