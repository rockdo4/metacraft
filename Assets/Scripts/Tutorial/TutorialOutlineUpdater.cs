using UnityEngine;

public class TutorialOutlineUpdater : MonoBehaviour
{
    public TutorialOutline tutorialOutline;

    private void Awake()
    {
        tutorialOutline = GetComponent<TutorialOutline>();
    }

    private void Update()
    {
        tutorialOutline.UpdatePosAndPanels();
    }

}
