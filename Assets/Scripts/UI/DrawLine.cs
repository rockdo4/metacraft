using UnityEngine;
using UnityEngine.UI;

public class DrawLine : MonoBehaviour
{
    public RectTransform lineStart; // 선의 시작 지점을 가리키는 RectTransform
    public RectTransform lineEnd; // 선의 끝 지점을 가리키는 RectTransform
    public Color lineColor = Color.white; // 선의 색상
    public float lineWidth = 2f; // 선의 너비

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
