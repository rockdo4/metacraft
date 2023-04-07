using UnityEngine;

public class DialoguePanel : MonoBehaviour
{
    public TutorialManager tutorialManager;
    public GameObject dialoguePanel;

    public void OnNextTutorialEvent()
    {
        tutorialManager.OnNextTutorialEvent();
        tutorialManager.OnEvent();
        dialoguePanel.SetActive(false);
    }
}
