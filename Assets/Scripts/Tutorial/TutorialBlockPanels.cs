using UnityEngine;
using UnityEngine.UI;
public class TutorialBlockPanels : MonoBehaviour
{
    private static TutorialBlockPanels _instance;

    public static TutorialBlockPanels Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TutorialBlockPanels>();

                if (_instance == null)
                {
                    GameObject obj = new()
                    {
                        name = typeof(TutorialBlockPanels).Name
                    };
                    _instance = obj.AddComponent<TutorialBlockPanels>();
                }
            }

            return _instance;
        }
    }

    public virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }
    public Color panelColor;

    public RawImage bottom;
    public RawImage left;
    public RawImage top;
    public RawImage right;
    public void SetPanelsSurroundTarget(Image targetImage)
    {
        RectTransform targetRectTransform = targetImage.rectTransform;
        Vector3[] targetCorners = new Vector3[4];
        targetRectTransform.GetWorldCorners(targetCorners);
        
        bottom.rectTransform.sizeDelta = new(bottom.rectTransform.sizeDelta.x, targetCorners[0].y);

        SetSidePanelOffsets(left.rectTransform, targetCorners[0].y, targetCorners[1].y);

        top.rectTransform.sizeDelta = new(top.rectTransform.sizeDelta.x, targetCorners[1].y);

        SetSidePanelOffsets(right.rectTransform, targetCorners[0].y, targetCorners[1].y);
    }

    private void SetSidePanelOffsets(RectTransform rectTransform, float bottom, float top)
    {   
        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, bottom);
        rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, top);
    }
}
