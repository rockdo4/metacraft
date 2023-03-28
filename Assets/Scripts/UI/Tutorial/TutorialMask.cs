using System;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class TutorialMask : MonoBehaviour
{
    public Button maskClickEvent;
    public RectTransform back;
    public RectTransform mask;

    public void ConnectEventToMask(GameObject gameObject)
    {
        var btn = gameObject.GetComponent<Button>();
        maskClickEvent.onClick.RemoveAllListeners();
        maskClickEvent.onClick.AddListener(() => btn.onClick.Invoke());
        SetMansk(gameObject.transform);
    }

    public void SetMansk(Transform tr)
    {
        mask.anchoredPosition = tr.transform.position;
        back.localPosition = -mask.transform.position;
    }
    public void SetMansk(Vector3 pos)
    {
        mask.anchoredPosition = pos;
        back.localPosition = -mask.anchoredPosition;
    }
    private void Update()
    {
        back.localPosition = -mask.anchoredPosition;
    }

    [ContextMenu("test1")]
    public void Test1()
    {
        SetMansk(Vector3.zero);
    }
    [ContextMenu("test2")]
    public void Test2()
    {
        SetMansk(new Vector3(100, 100, 0));
    }
}
