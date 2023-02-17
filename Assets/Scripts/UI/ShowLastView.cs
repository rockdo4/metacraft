
public class ShowLastView : View
{
    public override void Init()
    {
        button.onClick.AddListener(() => ViewManager.ShowLast());
    }
}
