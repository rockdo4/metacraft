using TMPro;
using UnityEngine;

public class FPS_Check : MonoBehaviour
{
    public TextMeshProUGUI fpsTxt;
    private float deltaTime = 0.0f;

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        fpsTxt.text = $"{msec:0.0} ms ({fps:0.} fps)";
    }
}