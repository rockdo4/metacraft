using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPS_Check : MonoBehaviour
{
    public TextMeshProUGUI fpsTxt;
    string textFormat = "{0:0.0} ms ({1:0.} fps)";

    float deltaTime = 0.0f;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format(textFormat, msec, fps);
        fpsTxt.text = text;
    }
}