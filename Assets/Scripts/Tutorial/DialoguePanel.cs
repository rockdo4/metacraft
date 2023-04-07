using UnityEngine;

public class DialoguePanel : MonoBehaviour
{
    public TutorialManager tutorialManager;

    public void OnNextTutorialEvent()
    {
        tutorialManager.OnNextTutorialEvent();
        tutorialManager.OnEvent();
        gameObject.SetActive(false);
    }
}
