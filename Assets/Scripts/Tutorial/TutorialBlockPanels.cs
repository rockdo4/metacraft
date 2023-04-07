using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class TutorialBlockPanels : MonoBehaviour
{   
    public static TutorialBlockPanels Instance { get; private set; }
    public Color panelColor;

    public RawImage bottom;
    public RawImage left;
    public RawImage top;
    public RawImage right;
    public virtual void Awake()
    {
        Instance = this; 
        
        bottom.color = panelColor;
        left.color = panelColor;
        top.color = panelColor;
        right.color = panelColor;
    }
    public void SetPanelsSurroundTarget(Image targetImage)
    {
        gameObject.SetActive(true);

        RectTransform targetRectTransform = targetImage.rectTransform;
        var canvasRect = GetComponent<RectTransform>().GetParentCanvas().GetComponent<RectTransform>();
        Vector2 canvasCenter = canvasRect.sizeDelta * 0.5f;

        float leftOffSet = canvasCenter.x + targetRectTransform.offsetMin.x;
        float rightOffSet = canvasCenter.x - targetRectTransform.offsetMax.x;
        float bottomOffSet = canvasCenter.y + targetRectTransform.offsetMin.y;
        float topOffSet = canvasCenter.y - targetRectTransform.offsetMax.y;

        bottom.rectTransform.sizeDelta = new(bottom.rectTransform.sizeDelta.x, bottomOffSet);
        top.rectTransform.sizeDelta = new(top.rectTransform.sizeDelta.x, topOffSet);

        float height = canvasRect.sizeDelta.y - bottomOffSet - topOffSet;
        float posYadjust = (bottomOffSet - topOffSet) * 0.5f;

        left.rectTransform.sizeDelta = new(leftOffSet, height);
        left.rectTransform.localPosition = new(left.rectTransform.localPosition.x, posYadjust);

        right.rectTransform.sizeDelta = new(rightOffSet, height);
        right.rectTransform.localPosition = new(right.rectTransform.localPosition.x, posYadjust);
    }

    public void SetPanelsSurroundTarget(RawImage targetImage)
    {
        gameObject.SetActive(true);

        RectTransform targetRectTransform = targetImage.rectTransform;
        var canvasRect = GetComponent<RectTransform>().GetParentCanvas().GetComponent<RectTransform>();
        Vector2 canvasCenter = canvasRect.sizeDelta * 0.5f;

        float leftOffSet = canvasCenter.x + targetRectTransform.offsetMin.x;
        float rightOffSet = canvasCenter.x - targetRectTransform.offsetMax.x;
        float bottomOffSet = canvasCenter.y + targetRectTransform.offsetMin.y;
        float topOffSet = canvasCenter.y - targetRectTransform.offsetMax.y;

        bottom.rectTransform.sizeDelta = new(bottom.rectTransform.sizeDelta.x, bottomOffSet);
        top.rectTransform.sizeDelta = new(top.rectTransform.sizeDelta.x, topOffSet);

        float height = canvasRect.sizeDelta.y - bottomOffSet - topOffSet;
        float posYadjust = (bottomOffSet - topOffSet) * 0.5f;

        left.rectTransform.sizeDelta = new(leftOffSet, height);
        left.rectTransform.localPosition = new(left.rectTransform.localPosition.x, posYadjust);

        right.rectTransform.sizeDelta = new(rightOffSet, height);
        right.rectTransform.localPosition = new(right.rectTransform.localPosition.x, posYadjust);
    }
}
