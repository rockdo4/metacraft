using UnityEngine.UI;
public class TestSettingView2 : View
{
    public Button backButton;
    public override void Initialize()
    {
        backButton.onClick.AddListener(() => ViewManager.Show<TestSettingView>());
    }
}
