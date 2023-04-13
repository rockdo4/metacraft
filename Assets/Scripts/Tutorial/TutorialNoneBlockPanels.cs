using UnityEngine;

public class TutorialNoneBlockPanels : TutorialBlockPanels
{
    public static new TutorialNoneBlockPanels Instance;

    public TutorialManager tutorialManager;
    public override void Awake()
    {
        Instance = this;
        if (!GameManager.Instance.playerData.isTutorial)
        {
            bottom.gameObject.SetActive(false);
            left.gameObject.SetActive(false);
            top.gameObject.SetActive(false);
            right.gameObject.SetActive(false);
            return;
        }
        bottom.color = panelColor;
        left.color = panelColor;
        top.color = panelColor;
        right.color = panelColor;
    }
    public void OnNextTutorialEvent()
    {
        if (TutorialManager.currEv == 15)
        {
            tutorialManager.SetTextBoxActive();
            tutorialManager.SetOutlineActive();
            Time.timeScale = 2f;
            return;
        }

        tutorialManager.OnNextTutorialEvent();
        tutorialManager.OnEvent();
    }
}
