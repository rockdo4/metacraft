using System.Net;
using UnityEngine;

public class DrawLineBetweenObjects : MonoBehaviour
{
    public Transform startObject;
    public Transform endObject;
    public Color lineColor = Color.white;
    public float lineWidth = 0.1f;
    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.material.color = lineColor;
    }

    private void Update()
    {
        Play();
    }

    public void Play()
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, endObject.position);
        lineRenderer.SetPosition(1, GetBentPoint(startObject.position,endObject.position));
        lineRenderer.SetPosition(2, startObject.position);
    }
    private Vector3 GetBentPoint(Vector3 start, Vector3 end)
    {
        Vector3 middlePoint = new Vector3(start.x,end.y,0);
        return middlePoint;
    }
}
