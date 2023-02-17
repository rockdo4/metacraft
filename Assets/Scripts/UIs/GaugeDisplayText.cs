using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GaugeDisplayText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Slider slider;

    private void Start()
    {
        slider.value = slider.maxValue;
        UpdateText();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            slider.value += 5f;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            slider.value -= 5f;
        }
    }

    public void UpdateText()
    {
        text.text = $"{slider.value:.} / {slider.maxValue:.}";
    }
}
