using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestMainView : View
{
    public Button startButton;
    public override void Initialize()
    {
        startButton.onClick.AddListener(() => ViewManager.Show<TestSettingView>());         
    }
}
