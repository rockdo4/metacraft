using UnityEngine;
using UnityEngine.UI;

public class UIMaskTest : MonoBehaviour
{    
    public Image targetImage; // �ѷ��� �̹���

    private void Start()
    {
        // �̹��� ������ ������ �г� ����
        CreateTransparentPanels();
    }

    private void CreateTransparentPanels()
    {
        RectTransform targetRectTransform = targetImage.rectTransform;
        Vector3[] targetCorners = new Vector3[4];
        targetRectTransform.GetWorldCorners(targetCorners);

        // �г��� ��ġ, ũ�� ����
        float left = targetCorners[0].x;
        float bottom = targetCorners[0].y;
        float right = targetCorners[2].x;
        float top = targetCorners[2].y;
        float width = right - left;
        float height = top - bottom;

        // �г� ����
        GameObject panelPrefab = Resources.Load<GameObject>("TransparentPanelPrefab");
        for (float x = left; x < right; x += width / 2)
        {
            for (float y = bottom; y < top; y += height / 2)
            {
                GameObject panel = Instantiate(panelPrefab, transform);
                RectTransform panelRectTransform = panel.GetComponent<RectTransform>();

                panelRectTransform.anchorMin = new Vector2(0, 0);
                panelRectTransform.anchorMax = new Vector2(0, 0);
                panelRectTransform.pivot = new Vector2(0, 0);
                panelRectTransform.sizeDelta = new Vector2(width / 2, height / 2);
                panelRectTransform.anchoredPosition = new Vector2(x, y);

                // �г��� ��ġ�� �̹����� ���� �ʵ��� ����
                Vector3[] panelCorners = new Vector3[4];
                panelRectTransform.GetWorldCorners(panelCorners);
                if (panelCorners[2].x > targetCorners[2].x)
                {
                    panelRectTransform.anchoredPosition -= new Vector2(panelCorners[2].x - targetCorners[2].x, 0);
                }
                if (panelCorners[2].y > targetCorners[2].y)
                {
                    panelRectTransform.anchoredPosition -= new Vector2(0, panelCorners[2].y - targetCorners[2].y);
                }
            }
        }
    }


    public void MaskTest()
    {
        Logger.Debug("����");
    }
   
}
