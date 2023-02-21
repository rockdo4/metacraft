using UnityEngine;
using UnityEngine.UI;

public class DrawLine : MonoBehaviour
{
    public RectTransform lineStart; // ���� ���� ������ ����Ű�� RectTransform
    public RectTransform lineEnd; // ���� �� ������ ����Ű�� RectTransform
    public Color lineColor = Color.white; // ���� ����
    public float lineWidth = 2f; // ���� �ʺ�

    private RectTransform rectTransform;
    private LineRenderer lineRenderer;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("UI/Default"));
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
    }

    void Update()
    {
        Vector3 startPos = GetWorldPos(lineStart.position);
        Vector3 endPos = GetWorldPos(lineEnd.position);

        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }

    private Vector3 GetWorldPos(Vector3 screenPos)
    {
        Vector3 worldPos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, screenPos, Camera.main, out worldPos);
        return worldPos;
    }
}
