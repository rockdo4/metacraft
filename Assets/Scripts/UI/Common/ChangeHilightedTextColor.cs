using UnityEngine;
using TMPro;
public class ChangeHilightedTextColor : MonoBehaviour
{
    private TextMeshProUGUI textMeshProUGUI;

    public Color normalColor;
    public Color highlightedColor;
    public void IsSelected(bool isOn)
    {    

        textMeshProUGUI ??= GetComponent<TextMeshProUGUI>();

        textMeshProUGUI.color = isOn ? highlightedColor : normalColor;
    }

}
