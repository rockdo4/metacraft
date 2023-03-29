using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialButton : MonoBehaviour
{
    public GameObject outline;
    public GameObject chatWindow;
    public TextMeshProUGUI textObject;
    public TutorialMask tutorialMask;

    public bool isMask = false;
    public Button maskButton;
    public Transform maskButtonParent;

    public void SetText(string txt)
    {
        textObject.text = txt;
    }
    public void OnWindow()
    {
        chatWindow.SetActive(true);
    }
    public void OnOutline()
    {
        if (outline != null)
        {
            outline.SetActive(true);
        }
    }
    public void OffWindow()
    {
        chatWindow.SetActive(false);
    }
    public void OffOutline()
    {
        if (outline != null)
            outline.SetActive(false);
    }
}
