using UnityEngine;
using UnityEngine.UI;

public class ScrollOptimizerVertical : MonoBehaviour
{    
    public GameObject content;
    public GameObject viewport;  
    private (GameObject go, RectTransform rect)[] contentItems;

    Vector3 viewportMin;
    Vector3 viewportMax;

    private void Start()
    {
        SetContentItems();
        viewportMin = viewport.GetComponent<RectTransform>().rect.min;
        viewportMax = viewport.GetComponent<RectTransform>().rect.max;        

    }
    private void OnScrollRectValueChanged(Vector2 _)
    {
        for (int i = 0; i < contentItems.Length; ++i)
        {
            Vector3 contentMin = contentItems[i].rect.offsetMin;
            Vector3 contentMax = contentItems[i].rect.offsetMax;

            var isInViewport = contentMax.y >= viewportMin.y && contentMin.y <= viewportMax.y;
            contentItems[i].go.SetActive(isInViewport);
        }
    }
    private void SetContentItems()
    {
        RectTransform contentTransform = content.GetComponent<RectTransform>();        
        contentItems = new (GameObject go, RectTransform rect)[contentTransform.childCount];
        for (int i = 0; i < contentItems.Length; i++)
        {
            Transform childTransform = contentTransform.GetChild(i);
            GameObject childObject = childTransform.gameObject;
            RectTransform childRect = childObject.GetComponent<RectTransform>();
            contentItems[i].go = childObject;
            contentItems[i].rect = childRect;
        }
    }
    private void OnEnable()
    {
        GetComponent<ScrollRect>().onValueChanged.AddListener(OnScrollRectValueChanged);
    }

    private void OnDisable()
    {
        GetComponent<ScrollRect>().onValueChanged.RemoveListener(OnScrollRectValueChanged);
    }
}
