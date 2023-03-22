using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GaugeDisplayText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Slider slider;

    public void UpdateText()
    {
        text.text = $"{slider.value:.} / {slider.maxValue:.}";
    }
}
