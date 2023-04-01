using UnityEngine;

public class DroneAnimation : MonoBehaviour
{
    private float startHeight;
    public float floatingMaxRange = 4f;
    public float floatingSpeed = 2f;
    private void Awake()
    {
        startHeight = transform.localPosition.y;
    }
    private void Start()
    {
        floatingSpeed *= Mathf.PI / 3f;
    }

    private void Update()
    {
        float floatingHeight = startHeight + Mathf.Sin(Time.time * floatingSpeed) * floatingMaxRange;
        var localPos = transform.localPosition;
        localPos.y = floatingHeight;
        transform.localPosition = localPos;        
    }
}
