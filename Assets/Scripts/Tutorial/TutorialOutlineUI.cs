using UnityEngine;

public class TutorialOutlineUI : MonoBehaviour
{
    public Color outlineColor;
    public float outlineWidth;

    private UnityEngine.UI.Outline outline;
    private void Awake()
    {        
        if(!TryGetComponent(out outline))
            outline = gameObject.AddComponent<UnityEngine.UI.Outline>();

        outline.effectColor = outlineColor;
        outline.effectDistance = new Vector2(outlineWidth, outlineWidth);
    }
}
