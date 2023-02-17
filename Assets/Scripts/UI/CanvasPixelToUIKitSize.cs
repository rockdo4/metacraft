using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class CanvasPixelToUIKitSize : MonoBehaviour
{
    private CanvasScaler canvasScaler;

    private void Awake()
    {
        canvasScaler= GetComponent<CanvasScaler>();
        Resize();
    }
    private void Resize()
    {
        if (Application.isEditor)
        {
            return;
        }

        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;

        //�ȵ� #if �ʿ�
        //canvasScaler.scaleFactor = Screen.dpi / 160;
    }
}
