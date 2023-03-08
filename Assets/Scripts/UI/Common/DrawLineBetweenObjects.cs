using System.Net;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class DrawLineBetweenObjects : MonoBehaviour
{
    public Transform startObject;
    public Transform endObject;

    private void Update()
    {
        transform.position = GetBentPoint(startObject.position, endObject.position);
    }

    private Vector2 GetBentPoint(Vector3 start, Vector3 end)
    {
        Vector2 middlePoint = new Vector2(start.x,end.y);
        return middlePoint;
    }
}
