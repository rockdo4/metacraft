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
    RectTransform maskClickEventRectTr;

    public Button TestEvent;
    private void Awake()
    {
        maskClickEventRectTr = maskClickEvent.GetComponent<RectTransform>();
        Setting();
    }
    [ContextMenu("Setting")]
    private void Setting()
    {
        SetMaskSize(TestEvent.GetComponent<RectTransform>().sizeDelta*1.5f, true);
        ConnectEventToMask(TestEvent);
        SetManskPosition(TestEvent.GetComponent<RectTransform>().anchoredPosition);
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
    public void ConnectEventToMask(GameObject gameObject)
    {
        var btn = gameObject.GetComponent<Button>();
        var rectTr = btn.GetComponent<RectTransform>();

        maskClickEvent.onClick.RemoveAllListeners();
        maskClickEvent.onClick.AddListener(() => btn.onClick.Invoke());
        maskClickEventRectTr.anchoredPosition = Vector3.zero;
        maskClickEventRectTr.sizeDelta = rectTr.sizeDelta;
    }
    public void ConnectEventToMask(Button btn)
    {
        var rectTr = btn.GetComponent<RectTransform>();

        maskClickEvent.onClick.RemoveAllListeners();
        maskClickEvent.onClick.AddListener(() => btn.onClick.Invoke());
        maskClickEventRectTr.anchoredPosition = Vector3.zero;
        maskClickEventRectTr.sizeDelta = rectTr.sizeDelta;
    }

    public void SetManskPosition(Vector3 pos)
    {
        mask.localPosition = pos;
        back.localPosition = -mask.localPosition;
    }
    private void Update()
    {
        back.localPosition = -mask.localPosition;
    }

    [ContextMenu("test1")]
    public void Test1()
    {
        SetManskPosition(Vector3.zero);
    }
    [ContextMenu("test2")]
    public void Test2()
    {
        SetManskPosition(new Vector3(100, 100, 0));
    }
}
