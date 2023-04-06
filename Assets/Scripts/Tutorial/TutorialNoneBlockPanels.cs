public class TutorialNoneBlockPanels : TutorialBlockPanels
{
    public static new TutorialNoneBlockPanels Instance;

    public TutorialManager tutorialManager;
    public override void Awake()
    {
        Instance = this;

        bottom.color = panelColor;
        left.color = panelColor;
        top.color = panelColor;
        right.color = panelColor;
    }
    public void OnNextTutorialEvent()
    {
        tutorialManager.OnNextTutorialEvent();
        tutorialManager.OnEvent();
    }
}
