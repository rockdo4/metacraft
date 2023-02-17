using UnityEngine.UI;

public class TestSettingView : View
{
    public Button backButton;
    public override void Initialize()
    {
        backButton.onClick.AddListener(() => ViewManager.ShowLast());
    }
}
