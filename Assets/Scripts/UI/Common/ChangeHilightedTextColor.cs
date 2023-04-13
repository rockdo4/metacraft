using UnityEngine;
using TMPro;
public class ChangeHilightedTextColor : MonoBehaviour
{
    private TextMeshProUGUI textMeshProUGUI;

    public Color normalColor;
    public Color highlightedColor;

    private void Awake()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    public void IsSelected(bool isOn)
    {
        if (textMeshProUGUI == null)
            return;

        textMeshProUGUI.color = isOn ? highlightedColor : normalColor;
    }

}
